# Statistical Analysis of Performance Experiments


Generated: 2026-04-22T21:19:49.830517


All values reported as **mean ± std** across 5 repetitions per condition.

Coefficient of variation (CV) should be < 5% for reliable measurements.


## CONSTANT load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 4842.0 ± 381.1 ms | 11.15 ± 0.51 req/s | 0.00% ± 0.00% | 7.87 | 5 |
| separated | 4684.0 ± 670.5 ms | 11.23 ± 0.61 req/s | 0.00% ± 0.00% | 14.32 | 5 |
| microservices | 7661.9 ± 312.6 ms | 12.36 ± 0.38 req/s | 0.00% ± 0.00% | 4.08 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Login p95 | Rooms p95 | Booking p95 |
|---|---|---|---|
| monolith | 3463.7 ± 209.9 ms | 1379.3 ± 178.2 ms | 6339.1 ± 685.6 ms |
| separated | 3323.6 ± 349.9 ms | 1538.5 ± 325.2 ms | 6261.5 ± 787.6 ms |
| microservices | 10006.1 ± 385.5 ms | 12.5 ± 0.9 ms | 21.2 ± 1.2 ms |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | 0.8413 | ✘ no | 0.12 | negligible | separated |
| monolith vs microservices | 0.0079 | ✔ yes | -1.0 | large | monolith |
| separated vs microservices | 0.0079 | ✔ yes | -1.0 | large | separated |

## RAMPUP load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 42001.7 ± 24647.3 ms | 7.11 ± 1.18 req/s | 0.00% ± 0.00% | 58.68 | 5 |
| separated | 60001.3 ± 0.4 ms | 2.33 ± 1.20 req/s | 0.00% ± 0.00% | 0.0 | 5 |
| microservices | 24006.3 ± 20122.1 ms | 10.39 ± 1.08 req/s | 0.00% ± 0.00% | 83.82 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Login p95 | Rooms p95 | Booking p95 |
|---|---|---|---|
| monolith | 42001.5 ± 24647.1 ms | 35828.4 ± 22809.1 ms | 60001.4 ± 1.1 ms |
| separated | 60001.2 ± 0.6 ms | 60001.3 ± 0.4 ms | 60000.6 ± 0.0 ms |
| microservices | 24008.1 ± 20121.3 ms | 6.8 ± 6.9 ms | 24.6 ± 0.0 ms |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | 0.6905 | ✘ no | -0.2 | small | monolith |
| monolith vs microservices | 0.8413 | ✘ no | 0.12 | negligible | microservices |
| separated vs microservices | 0.1508 | ✘ no | 0.6 | large | microservices |

## How to read this report

- **CV (Coefficient of Variation):** measurement stability. < 5% is good, > 10% suggests noisy environment.
- **Mann-Whitney U:** non-parametric test (no normality assumption). p < 0.05 = statistically significant difference.
- **Cliff's δ:** non-parametric effect size (Romano et al., 2006):
  - |δ| < 0.147 → negligible (statistical significance without practical relevance)
  - 0.147 ≤ |δ| < 0.33  → small
  - 0.33  ≤ |δ| < 0.474 → medium
  - |δ| ≥ 0.474 → large

For thesis defense: a significant p-value with large δ is a strong finding.
A significant p-value with negligible δ means 'the difference exists but doesn't matter in practice'.
