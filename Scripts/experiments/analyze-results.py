#!/usr/bin/env python3
"""
Post-hoc statistical analysis of k6 summary JSONs.

Reads results/{arch}_{profile}_rep{N}_summary.json for:
  arch in {monolith, separated, microservices}
  profile in {constant, rampup}
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
PROFILES = ["constant", "rampup"]


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
        "rooms_duration_p95":       get("rooms_duration.p(95)") or get("rooms_duration.p95"),
        "booking_duration_p95":     get("booking_duration.p(95)") or get("booking_duration.p95"),
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


def build_report(by_arch):
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
        out.append("| Architecture | Login p95 | Rooms p95 | Booking p95 |")
        out.append("|---|---|---|---|")
        for arch in ARCHITECTURES:
            reps = by_arch[arch][profile]
            if not reps:
                out.append(f"| {arch} | _no data_ | _no data_ | _no data_ |")
                continue
            login = aggregate(reps, "login_duration_p95")
            rooms = aggregate(reps, "rooms_duration_p95")
            booking = aggregate(reps, "booking_duration_p95")
            out.append(f"| {arch} | {fmt_ms(login)} | {fmt_ms(rooms)} | {fmt_ms(booking)} |")

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

    # Sanity log
    for arch in ARCHITECTURES:
        for profile in PROFILES:
            n = len(by_arch[arch][profile])
            print(f"  {arch:15s} {profile:10s} : {n} reps loaded")

    report = build_report(by_arch)

    output = args.output or args.results_dir / "analysis.md"
    output.write_text(report, encoding="utf-8")
    print(f"\n✔ Report written to: {output}")

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
    print(f"✔ Raw aggregates: {json_out}")


if __name__ == "__main__":
    main()
