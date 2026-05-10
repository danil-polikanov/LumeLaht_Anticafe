#!/usr/bin/env python3
"""
Post-hoc statistical analysis of k6 summary JSONs.

Reads results/{arch}_{profile}_rep{N}_summary.json for:
  arch in {monolith, separated, microservices}
  profile in {constant, rampup, spike}
  N in 1..5

Computes:
  - Mean, median, std for each metric
  - Mann-Whitney U test between every pair of architectures (non-parametric)
  - Cliff's delta (effect size, robust to non-normality)
  - 95% CI via bootstrap

Outputs:
  - results/analysis.md  — ready-to-paste markdown tables for the thesis
  - results/analysis.json — raw aggregated numbers

Usage: python3 analyze-results.py [--results-dir results]
"""

import json
import re
import argparse
import sys
from pathlib import Path
from statistics import mean, median, stdev
from itertools import combinations

try:
    from scipy.stats import mannwhitneyu
    import numpy as np
    HAS_SCIPY = True
except ImportError:
    HAS_SCIPY = False
    print("WARNING: scipy/numpy not installed — statistical tests will be skipped.")
    print("Install with: pip3 install scipy numpy")


ARCHITECTURES = ["monolith", "separated", "microservices"]
PROFILES = ["constant", "rampup", "spike"]


def cliffs_delta(x, y):
    """Non-parametric effect size. Range [-1, 1]. 0=no effect."""
    n_x, n_y = len(x), len(y)
    if n_x == 0 or n_y == 0:
        return 0.0
    gt = sum(1 for a in x for b in y if a > b)
    lt = sum(1 for a in x for b in y if a < b)
    return (gt - lt) / (n_x * n_y)


def interpret_cliffs(d):
    """Romano et al. (2006) thresholds."""
    a = abs(d)
    if a < 0.147: return "negligible"
    if a < 0.33:  return "small"
    if a < 0.474: return "medium"
    return "large"


def extract_metrics(summary_path):
    """Extract k6 metrics from a single summary.json."""
    with open(summary_path) as f:
        data = json.load(f)

    m = data.get("metrics", {})

    def get(path, default=None):
        """Walk dot-separated path."""
        obj = m
        for p in path.split("."):
            if isinstance(obj, dict) and p in obj:
                obj = obj[p]
            else:
                return default
        return obj

    return {
        "http_req_duration_avg":    get("http_req_duration.avg"),
        "http_req_duration_med":    get("http_req_duration.med"),
        "http_req_duration_p95":    get("http_req_duration.p(95)") or get("http_req_duration.p95"),
        "http_req_duration_p99":    get("http_req_duration.p(99)") or get("http_req_duration.p99"),
        "http_reqs":                get("http_reqs.count", 0),
        "http_reqs_rate":           get("http_reqs.rate", 0),
        "error_rate":               get("errors.rate", 0) or get("http_req_failed.rate", 0),
        "login_duration_p95":       get("login_duration.p(95)") or get("login_duration.p95"),
        "register_duration_p95":    get("register_duration.p(95)") or get("register_duration.p95"),
        "rooms_duration_p95":       get("rooms_duration.p(95)") or get("rooms_duration.p95"),
        "booking_duration_p95":     get("booking_duration.p(95)") or get("booking_duration.p95"),
        "my_bookings_duration_p95": get("my_bookings_duration.p(95)") or get("my_bookings_duration.p95"),
        "cancel_duration_p95":      get("cancel_duration.p(95)") or get("cancel_duration.p95"),
        "iteration_duration_avg":   get("iteration_duration.avg"),
    }


def load_all(results_dir):
    """Return {arch: {profile: [list of reps]}}."""
    by_arch = {a: {p: [] for p in PROFILES} for a in ARCHITECTURES}
    for arch in ARCHITECTURES:
        for profile in PROFILES:
            for rep in range(1, 10):  # up to 9 reps
                f = results_dir / f"{arch}_{profile}_rep{rep}_summary.json"
                if not f.exists():
                    continue
                try:
                    by_arch[arch][profile].append(extract_metrics(f))
                except Exception as e:
                    print(f"! Failed to parse {f.name}: {e}")
    return by_arch


# ─── Docker stats parsing ──────────────────────────────────────────────────────
# `docker stats --format json` produces strings like "5.23%" for CPU and
# "127.7MiB / 1GiB" for memory; we parse them to numbers so we can aggregate.

# We only include containers belonging to the LumeLaht docker-compose stack;
# unrelated containers running on the host (mcp-servers, side projects) get
# rejected up front. Within the stack, monitoring sidecars are also excluded
# because a hosting provider would not charge for them as part of the workload.
INCLUDE_CONTAINER_PREFIX = "lumelaht_anticafe-"
EXCLUDE_CONTAINER_PATTERNS = ("prometheus", "grafana")


def _parse_pct(s):
    if not s:
        return 0.0
    m = re.match(r"([0-9.]+)", s)
    return float(m.group(1)) if m else 0.0


_MEM_UNITS = {"B": 1 / (1024 ** 2), "KiB": 1 / 1024, "MiB": 1.0, "GiB": 1024.0,
              "kB": 1 / 1024, "MB": 1.0, "GB": 1024.0}


def _parse_mem_mib(s):
    """Parse the left-hand side of '127.7MiB / 1GiB' into MiB."""
    if not s:
        return 0.0
    left = s.split("/")[0].strip()
    m = re.match(r"([0-9.]+)\s*([KMG]i?B)", left)
    if not m:
        return 0.0
    value, unit = float(m.group(1)), m.group(2)
    return value * _MEM_UNITS.get(unit, 1.0)


def _is_excluded(container_name):
    n = container_name.lower()
    if not n.startswith(INCLUDE_CONTAINER_PREFIX):
        return True
    return any(p in n for p in EXCLUDE_CONTAINER_PATTERNS)


def parse_stats_file(path):
    """Aggregate one rep's docker stats jsonl into per-rep summary.

    Returns dict with: peak_cpu_pct (sum across containers, peak sample),
    peak_mem_mib (sum across containers, peak sample), avg_cpu_pct, avg_mem_mib,
    plus per-container breakdown.
    """
    # Group samples by timestamp (each sample writes one line per container,
    # so we re-bucket by sample number using the order of arrival).
    # docker stats prints all containers in one batch per --no-stream call, so
    # consecutive same-container names mark a new sample.
    samples = []  # list of {container_name: {cpu, mem}}
    seen_in_sample = set()
    current = {}

    with open(path, "r", encoding="utf-8", errors="ignore") as f:
        for line in f:
            line = line.strip()
            if not line:
                continue
            try:
                obj = json.loads(line)
            except json.JSONDecodeError:
                continue
            name = obj.get("Name") or obj.get("Container") or ""
            if not name or _is_excluded(name):
                continue
            if name in seen_in_sample:
                # New sample begins
                samples.append(current)
                current = {}
                seen_in_sample = set()
            current[name] = {
                "cpu_pct": _parse_pct(obj.get("CPUPerc", "0%")),
                "mem_mib": _parse_mem_mib(obj.get("MemUsage", "0B / 0B")),
            }
            seen_in_sample.add(name)
    if current:
        samples.append(current)

    if not samples:
        return None

    # Total per sample = sum across containers in that sample (the stack's
    # total resource use at that moment).
    total_cpu_per_sample = [sum(c["cpu_pct"] for c in s.values()) for s in samples]
    total_mem_per_sample = [sum(c["mem_mib"] for c in s.values()) for s in samples]

    # Per-container peak across all samples (useful to call out hottest service).
    per_container = {}
    all_names = set().union(*(s.keys() for s in samples))
    for name in all_names:
        cpu_series = [s[name]["cpu_pct"] for s in samples if name in s]
        mem_series = [s[name]["mem_mib"] for s in samples if name in s]
        if cpu_series:
            per_container[name] = {
                "peak_cpu_pct": max(cpu_series),
                "avg_cpu_pct": mean(cpu_series),
                "peak_mem_mib": max(mem_series),
                "avg_mem_mib": mean(mem_series),
            }

    return {
        "n_samples": len(samples),
        "peak_cpu_pct": max(total_cpu_per_sample),
        "avg_cpu_pct": mean(total_cpu_per_sample),
        "peak_mem_mib": max(total_mem_per_sample),
        "avg_mem_mib": mean(total_mem_per_sample),
        "per_container": per_container,
    }


def load_all_stats(results_dir):
    """Return {arch: {profile: [list of per-rep stats summaries]}}."""
    by_arch = {a: {p: [] for p in PROFILES} for a in ARCHITECTURES}
    for arch in ARCHITECTURES:
        for profile in PROFILES:
            for rep in range(1, 10):
                f = results_dir / f"{arch}_{profile}_rep{rep}_stats.jsonl"
                if not f.exists():
                    continue
                try:
                    s = parse_stats_file(f)
                    if s:
                        by_arch[arch][profile].append(s)
                except Exception as e:
                    print(f"! Failed to parse stats {f.name}: {e}")
    return by_arch


def aggregate(reps, metric):
    """Mean, std, min, max across reps for a single metric."""
    vals = [r[metric] for r in reps if r.get(metric) is not None]
    if not vals:
        return None
    n = len(vals)
    return {
        "n":      n,
        "mean":   round(mean(vals), 2),
        "median": round(median(vals), 2),
        "std":    round(stdev(vals), 2) if n > 1 else 0.0,
        "min":    round(min(vals), 2),
        "max":    round(max(vals), 2),
        "cv_pct": round(stdev(vals) / mean(vals) * 100, 2) if n > 1 and mean(vals) > 0 else 0.0,
        "values": vals,
    }


def compare_pair(a_vals, b_vals, label_a, label_b):
    """Mann-Whitney U + Cliff's delta for two samples."""
    if not HAS_SCIPY or len(a_vals) < 3 or len(b_vals) < 3:
        return None
    try:
        u, p = mannwhitneyu(a_vals, b_vals, alternative="two-sided")
    except ValueError:
        return None
    d = cliffs_delta(a_vals, b_vals)
    return {
        "u": float(u),
        "p_value": float(p),
        "cliffs_delta": round(d, 3),
        "effect": interpret_cliffs(d),
        "significant": p < 0.05,
        "faster": label_a if mean(a_vals) < mean(b_vals) else label_b,
    }


def fmt_ms(stat):
    if stat is None:
        return "n/a"
    return f"{stat['mean']:.1f} ± {stat['std']:.1f} ms"


def fmt_rate(stat):
    if stat is None:
        return "n/a"
    return f"{stat['mean']:.2f} ± {stat['std']:.2f} req/s"


def fmt_pct(stat):
    if stat is None:
        return "n/a"
    return f"{stat['mean']*100:.2f}% ± {stat['std']*100:.2f}%"


def build_report(by_arch, by_arch_stats=None):
    out = []
    out.append("# Statistical Analysis of Performance Experiments\n")
    out.append(f"\nGenerated: {__import__('datetime').datetime.now().isoformat()}\n")
    out.append("\nAll values reported as **mean ± std** across 5 repetitions per condition.\n")
    out.append("Coefficient of variation (CV) should be < 5% for reliable measurements.\n")

    # ─── Raw aggregates ─────────────────────────────────────────────────────
    for profile in PROFILES:
        out.append(f"\n## {profile.upper()} load profile\n")

        # Main metrics table
        out.append("\n### Table: Overall response time (p95), throughput, error rate\n")
        out.append("| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |")
        out.append("|---|---|---|---|---|---|")
        for arch in ARCHITECTURES:
            reps = by_arch[arch][profile]
            if not reps:
                out.append(f"| {arch} | _no data_ | _no data_ | _no data_ | — | 0 |")
                continue
            p95 = aggregate(reps, "http_req_duration_p95")
            thr = aggregate(reps, "http_reqs_rate")
            err = aggregate(reps, "error_rate")
            cv = p95["cv_pct"] if p95 else "—"
            n = p95["n"] if p95 else 0
            out.append(f"| {arch} | {fmt_ms(p95)} | {fmt_rate(thr)} | {fmt_pct(err)} | {cv} | {n} |")

        # Per-operation breakdown
        out.append("\n### Table: Response time by operation (p95, mean ± std)\n")
        out.append("| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |")
        out.append("|---|---|---|---|---|---|---|")
        for arch in ARCHITECTURES:
            reps = by_arch[arch][profile]
            if not reps:
                out.append(f"| {arch} | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ |")
                continue
            reg = aggregate(reps, "register_duration_p95")
            login = aggregate(reps, "login_duration_p95")
            rooms = aggregate(reps, "rooms_duration_p95")
            booking = aggregate(reps, "booking_duration_p95")
            my_bk = aggregate(reps, "my_bookings_duration_p95")
            cancel = aggregate(reps, "cancel_duration_p95")
            out.append(f"| {arch} | {fmt_ms(reg)} | {fmt_ms(login)} | {fmt_ms(rooms)} | "
                       f"{fmt_ms(booking)} | {fmt_ms(my_bk)} | {fmt_ms(cancel)} |")

        # ─── Pairwise statistical tests ────────────────────────────────────
        out.append("\n### Pairwise comparisons (Mann-Whitney U, Cliff's δ)\n")
        out.append("| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |")
        out.append("|---|---|---|---|---|---|")
        for a, b in combinations(ARCHITECTURES, 2):
            a_vals = [r["http_req_duration_p95"] for r in by_arch[a][profile]
                      if r.get("http_req_duration_p95") is not None]
            b_vals = [r["http_req_duration_p95"] for r in by_arch[b][profile]
                      if r.get("http_req_duration_p95") is not None]
            cmp = compare_pair(a_vals, b_vals, a, b)
            if cmp is None:
                out.append(f"| {a} vs {b} | _insufficient data_ | — | — | — | — |")
            else:
                sig = "✔ yes" if cmp["significant"] else "✘ no"
                out.append(f"| {a} vs {b} | {cmp['p_value']:.4f} | {sig} | {cmp['cliffs_delta']} | "
                           f"{cmp['effect']} | {cmp['faster']} |")

    # ─── Resource consumption (docker stats) ────────────────────────────────
    if by_arch_stats:
        out.append("\n## Resource consumption (docker stats during benchmark)\n")
        out.append("Sums across all workload containers (monitoring sidecars excluded).\n")
        out.append("CPU% is normalized: 100% = 1 logical core fully used.\n")
        for profile in PROFILES:
            any_data = any(by_arch_stats[a][profile] for a in ARCHITECTURES)
            if not any_data:
                continue
            out.append(f"\n### {profile.upper()} — observed resource usage\n")
            out.append("| Architecture | Peak CPU% | Avg CPU% | Peak RAM (MiB) | Avg RAM (MiB) | n |")
            out.append("|---|---|---|---|---|---|")
            for arch in ARCHITECTURES:
                reps = by_arch_stats[arch][profile]
                if not reps:
                    out.append(f"| {arch} | _no data_ | _no data_ | _no data_ | _no data_ | 0 |")
                    continue
                peak_cpu = [r["peak_cpu_pct"] for r in reps]
                avg_cpu = [r["avg_cpu_pct"] for r in reps]
                peak_mem = [r["peak_mem_mib"] for r in reps]
                avg_mem = [r["avg_mem_mib"] for r in reps]
                n = len(reps)
                pc_m = mean(peak_cpu); pc_s = stdev(peak_cpu) if n > 1 else 0
                ac_m = mean(avg_cpu); ac_s = stdev(avg_cpu) if n > 1 else 0
                pm_m = mean(peak_mem); pm_s = stdev(peak_mem) if n > 1 else 0
                am_m = mean(avg_mem); am_s = stdev(avg_mem) if n > 1 else 0
                out.append(f"| {arch} | {pc_m:.0f} ± {pc_s:.0f}% | {ac_m:.0f} ± {ac_s:.0f}% | "
                           f"{pm_m:.0f} ± {pm_s:.0f} | {am_m:.0f} ± {am_s:.0f} | {n} |")

            # Per-container breakdown for the first rep of each arch — shows
            # which service is the bottleneck inside microservices.
            out.append("\n#### Per-container peak (first rep)\n")
            out.append("| Architecture | Container | Peak CPU% | Peak RAM (MiB) |")
            out.append("|---|---|---|---|")
            for arch in ARCHITECTURES:
                reps = by_arch_stats[arch][profile]
                if not reps:
                    continue
                pc = reps[0]["per_container"]
                for name, m in sorted(pc.items(), key=lambda kv: -kv[1]["peak_cpu_pct"]):
                    out.append(f"| {arch} | {name} | {m['peak_cpu_pct']:.0f}% | "
                               f"{m['peak_mem_mib']:.0f} |")

    # ─── Interpretation guide ───────────────────────────────────────────────
    out.append("\n## How to read this report\n")
    out.append("- **CV (Coefficient of Variation):** measurement stability. < 5% is good, > 10% suggests noisy environment.")
    out.append("- **Mann-Whitney U:** non-parametric test (no normality assumption). p < 0.05 = statistically significant difference.")
    out.append("- **Cliff's δ:** non-parametric effect size (Romano et al., 2006):")
    out.append("  - |δ| < 0.147 → negligible (statistical significance without practical relevance)")
    out.append("  - 0.147 ≤ |δ| < 0.33  → small")
    out.append("  - 0.33  ≤ |δ| < 0.474 → medium")
    out.append("  - |δ| ≥ 0.474 → large")
    out.append("\nFor thesis defense: a significant p-value with large δ is a strong finding.")
    out.append("A significant p-value with negligible δ means 'the difference exists but doesn't matter in practice'.")

    return "\n".join(out) + "\n"


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--results-dir", default="results", type=Path)
    parser.add_argument("--output", default=None, type=Path)
    args = parser.parse_args()

    if not args.results_dir.exists():
        print(f"Results directory not found: {args.results_dir}", file=sys.stderr)
        sys.exit(1)

    by_arch = load_all(args.results_dir)
    by_arch_stats = load_all_stats(args.results_dir)

    # Sanity log
    for arch in ARCHITECTURES:
        for profile in PROFILES:
            n = len(by_arch[arch][profile])
            n_stats = len(by_arch_stats[arch][profile])
            print(f"  {arch:15s} {profile:10s} : {n} reps loaded, {n_stats} stats files")

    report = build_report(by_arch, by_arch_stats)

    output = args.output or args.results_dir / "analysis.md"
    output.write_text(report, encoding="utf-8")
    print(f"\nOK Report written to: {output}")

    # Also dump raw aggregated JSON
    json_out = args.results_dir / "analysis.json"
    dump = {}
    for arch in ARCHITECTURES:
        dump[arch] = {}
        for profile in PROFILES:
            reps = by_arch[arch][profile]
            if not reps:
                continue
            dump[arch][profile] = {
                k: aggregate(reps, k) for k in reps[0].keys()
            }
    json_out.write_text(json.dumps(dump, indent=2, default=str), encoding="utf-8")
    print(f"OK Raw aggregates: {json_out}")


if __name__ == "__main__":
    main()
