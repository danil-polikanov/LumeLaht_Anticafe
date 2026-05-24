#!/usr/bin/env python3
"""Generate thesis graphs from k6 summary JSONs (no Prometheus needed)."""

import json
from pathlib import Path
import numpy as np
import matplotlib.pyplot as plt

ARCHS = ["monolith", "separated", "microservices"]
PROFILES = ["constant", "rampup", "spike"]
ENDPOINTS = [
    ("register_duration", "Register"),
    ("login_duration", "Login"),
    ("rooms_duration", "Rooms"),
    ("booking_duration", "Booking"),
    ("my_bookings_duration", "MyBookings"),
    ("cancel_duration", "Cancel"),
]
COLORS = {"monolith": "#2E86AB", "separated": "#A23B72", "microservices": "#F18F01"}

RESULTS = Path(__file__).resolve().parent.parent.parent / "results"
OUT = Path(__file__).resolve().parent.parent.parent / "graphs"
OUT.mkdir(exist_ok=True)


def load_metric(arch, profile, rep, key, stat="p(95)"):
    p = RESULTS / f"{arch}_{profile}_rep{rep}_summary.json"
    if not p.exists():
        return None
    d = json.loads(p.read_text())
    m = d.get("metrics", {}).get(key)
    if not m:
        return None
    v = m.get("values", m)
    return v.get(stat)


def collect(arch, profile, key, stat="p(95)"):
    return [v for r in range(1, 6) if (v := load_metric(arch, profile, r, key, stat)) is not None]


# ========== GRAPH 1: p95 per endpoint, rampup, log scale ==========
fig, ax = plt.subplots(figsize=(11, 6))
x = np.arange(len(ENDPOINTS))
w = 0.27
for i, arch in enumerate(ARCHS):
    means = [np.mean(collect(arch, "rampup", k)) for k, _ in ENDPOINTS]
    stds = [np.std(collect(arch, "rampup", k)) for k, _ in ENDPOINTS]
    ax.bar(x + (i - 1) * w, means, w, yerr=stds, label=arch,
           color=COLORS[arch], capsize=4, edgecolor="black", linewidth=0.5)
ax.set_yscale("log")
ax.set_xticks(x)
ax.set_xticklabels([n for _, n in ENDPOINTS])
ax.set_ylabel("p95 latency (ms, log scale)")
ax.set_title("p95 latency per endpoint under rampup load (10→200 VU, 7 min, n=5)\n"
             "Microservices show SPLIT PATTERN: auth ops 1000x slower, isolated ops 100x faster",
             fontsize=11)
ax.legend(loc="upper right", framealpha=0.9)
ax.grid(axis="y", alpha=0.3, which="both")
plt.tight_layout()
plt.savefig(OUT / "01_p95_per_endpoint_rampup.png", dpi=150)
plt.close()

# ========== GRAPH 2: overall p95 + throughput, both profiles ==========
fig, axes = plt.subplots(1, 2, figsize=(13, 5))
for ax, profile, title in [(axes[0], "constant", "CONSTANT (50 VU, 5 min)"),
                            (axes[1], "rampup", "RAMPUP (10→200 VU, 7 min)")]:
    means_p95, stds_p95, means_tp, stds_tp = [], [], [], []
    for arch in ARCHS:
        p95 = collect(arch, profile, "http_req_duration")
        tp = collect(arch, profile, "http_reqs", "rate")
        means_p95.append(np.mean(p95)); stds_p95.append(np.std(p95))
        means_tp.append(np.mean(tp)); stds_tp.append(np.std(tp))
    xp = np.arange(3)
    bars = ax.bar(xp, means_p95, yerr=stds_p95, color=[COLORS[a] for a in ARCHS],
                  capsize=6, edgecolor="black", linewidth=0.5)
    for b, v in zip(bars, means_p95):
        ax.text(b.get_x() + b.get_width() / 2, v + max(means_p95) * 0.02,
                f"{v:.0f} ms", ha="center", fontsize=9)
    ax.set_xticks(xp)
    ax.set_xticklabels(ARCHS)
    ax.set_ylabel("Overall p95 (ms)")
    ax.set_title(title)
    ax.grid(axis="y", alpha=0.3)
    ax2 = ax.twinx()
    ax2.plot(xp, means_tp, "ko-", linewidth=2, markersize=8, label="Throughput (req/s)")
    ax2.errorbar(xp, means_tp, yerr=stds_tp, fmt="none", ecolor="black", capsize=4)
    for xi, yi in zip(xp, means_tp):
        ax2.annotate(f"{yi:.1f} req/s", (xi, yi), textcoords="offset points",
                     xytext=(8, -12), fontsize=9)
    ax2.set_ylabel("Throughput (req/s)")
    ax2.legend(loc="upper left")
fig.suptitle("Overall p95 latency (bars) and throughput (line) per architecture",
             fontsize=12, y=1.02)
plt.tight_layout()
plt.savefig(OUT / "02_overall_p95_throughput.png", dpi=150, bbox_inches="tight")
plt.close()

# ========== GRAPH 3: variability across reps (box plot) ==========
fig, axes = plt.subplots(1, 2, figsize=(13, 5))
for ax, profile, title in [(axes[0], "constant", "CONSTANT (50 VU)"),
                            (axes[1], "rampup", "RAMPUP (10→200 VU)")]:
    data = [collect(arch, profile, "http_req_duration") for arch in ARCHS]
    bp = ax.boxplot(data, labels=ARCHS, patch_artist=True, widths=0.5)
    for patch, arch in zip(bp["boxes"], ARCHS):
        patch.set_facecolor(COLORS[arch])
        patch.set_alpha(0.7)
    ax.set_ylabel("p95 (ms)")
    ax.set_title(title)
    ax.grid(axis="y", alpha=0.3)
fig.suptitle("Variability across 5 repetitions per condition (box plot)",
             fontsize=12, y=1.02)
plt.tight_layout()
plt.savefig(OUT / "03_variability_boxplot.png", dpi=150, bbox_inches="tight")
plt.close()

# ========== GRAPH 4: split pattern visualization (microservices only) ==========
fig, ax = plt.subplots(figsize=(10, 5))
auth_ops = ["Login", "Register"]
isolated_ops = ["Rooms", "Booking", "MyBookings", "Cancel"]
auth_keys = ["login_duration", "register_duration"]
iso_keys = ["rooms_duration", "booking_duration", "my_bookings_duration", "cancel_duration"]
auth_vals = [np.mean(collect("microservices", "rampup", k)) for k in auth_keys]
iso_vals = [np.mean(collect("microservices", "rampup", k)) for k in iso_keys]
positions = list(range(len(auth_ops + isolated_ops)))
colors = ["#C0392B"] * len(auth_ops) + ["#27AE60"] * len(isolated_ops)
ax.bar(positions, auth_vals + iso_vals, color=colors,
       edgecolor="black", linewidth=0.5)
ax.set_xticks(positions)
ax.set_xticklabels(auth_ops + isolated_ops)
ax.set_yscale("log")
ax.set_ylabel("p95 latency (ms, log scale)")
ax.set_title("Microservices SPLIT PATTERN under rampup load\n"
             "Red = CPU-bound (UserService bcrypt bottleneck) | Green = isolated DB ops",
             fontsize=11)
for i, v in enumerate(auth_vals + iso_vals):
    ax.text(i, v * 1.2, f"{v:.0f}", ha="center", fontsize=9)
ax.grid(axis="y", alpha=0.3, which="both")
plt.tight_layout()
plt.savefig(OUT / "04_microservices_split_pattern.png", dpi=150)
plt.close()

print(f"Saved 4 graphs to: {OUT}")
for f in sorted(OUT.glob("*.png")):
    print(f"  - {f.name}")
