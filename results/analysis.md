# Statistical Analysis of Performance Experiments


Generated: 2026-04-23T08:02:44.011355


All values reported as **mean ± std** across 5 repetitions per condition.

Coefficient of variation (CV) should be < 5% for reliable measurements.


## CONSTANT load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 452.7 ± 145.1 ms | 22.62 ± 0.52 req/s | 0.00% ± 0.00% | 32.06 | 5 |
| separated | 448.8 ± 128.6 ms | 22.59 ± 0.46 req/s | 0.00% ± 0.00% | 28.66 | 5 |
| microservices | 4041.6 ± 1557.7 ms | 16.21 ± 2.57 req/s | 0.00% ± 0.00% | 38.54 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | 761.2 ± 213.4 ms | 693.9 ± 201.3 ms | 214.6 ± 67.6 ms | 279.3 ± 81.0 ms | 229.3 ± 62.6 ms | 263.1 ± 90.8 ms |
| separated | 751.3 ± 266.9 ms | 689.4 ± 255.2 ms | 211.7 ± 90.0 ms | 288.3 ± 112.9 ms | 235.4 ± 115.3 ms | 271.2 ± 119.2 ms |
| microservices | 6413.9 ± 2570.8 ms | 6166.4 ± 2592.9 ms | 12.6 ± 2.8 ms | 18.8 ± 0.6 ms | 13.2 ± 0.4 ms | 13.8 ± 1.5 ms |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | 1.0000 | ✘ no | 0.04 | negligible | separated |
| monolith vs microservices | 0.0079 | ✔ yes | -1.0 | large | monolith |
| separated vs microservices | 0.0079 | ✔ yes | -1.0 | large | separated |

## RAMPUP load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 8389.7 ± 509.8 ms | 31.19 ± 0.45 req/s | 0.00% ± 0.00% | 6.08 | 5 |
| separated | 8858.0 ± 381.8 ms | 30.99 ± 0.40 req/s | 0.00% ± 0.00% | 4.31 | 5 |
| microservices | 36442.8 ± 5322.9 ms | 17.73 ± 3.19 req/s | 0.00% ± 0.00% | 14.61 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | 10635.2 ± 932.4 ms | 9341.6 ± 1054.0 ms | 6242.8 ± 202.6 ms | 8169.1 ± 410.6 ms | 6167.9 ± 205.0 ms | 7359.0 ± 256.9 ms |
| separated | 11257.1 ± 949.1 ms | 10479.5 ± 1254.0 ms | 6446.1 ± 231.9 ms | 8778.5 ± 778.8 ms | 6459.4 ± 249.4 ms | 7269.3 ± 80.7 ms |
| microservices | 53305.6 ± 9310.3 ms | 42798.9 ± 10612.1 ms | 38.0 ± 3.9 ms | 26.0 ± 2.0 ms | 17.5 ± 3.4 ms | 23.5 ± 4.1 ms |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | 0.0952 | ✘ no | -0.68 | large | monolith |
| monolith vs microservices | 0.0079 | ✔ yes | -1.0 | large | monolith |
| separated vs microservices | 0.0079 | ✔ yes | -1.0 | large | separated |

## SPIKE load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | _no data_ | _no data_ | _no data_ | — | 0 |
| separated | _no data_ | _no data_ | _no data_ | — | 0 |
| microservices | _no data_ | _no data_ | _no data_ | — | 0 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ |
| separated | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ |
| microservices | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ | _no data_ |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | _insufficient data_ | — | — | — | — |
| monolith vs microservices | _insufficient data_ | — | — | — | — |
| separated vs microservices | _insufficient data_ | — | — | — | — |

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
