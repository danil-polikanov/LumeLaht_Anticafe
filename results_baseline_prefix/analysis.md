# Statistical Analysis of Performance Experiments


Generated: 2026-04-23T00:26:33.746705


All values reported as **mean ± std** across 5 repetitions per condition.

Coefficient of variation (CV) should be < 5% for reliable measurements.


## CONSTANT load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 4860.9 ± 356.0 ms | 11.09 ± 0.40 req/s | 0.00% ± 0.00% | 7.32 | 5 |
| separated | 4684.0 ± 670.5 ms | 11.23 ± 0.61 req/s | 0.00% ± 0.00% | 14.32 | 5 |
| microservices | 7661.9 ± 312.6 ms | 12.36 ± 0.38 req/s | 0.00% ± 0.00% | 4.08 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | n/a | 3422.0 ± 266.4 ms | 1425.0 ± 258.0 ms | 6374.3 ± 662.2 ms | n/a | n/a |
| separated | n/a | 3323.6 ± 349.9 ms | 1538.5 ± 325.2 ms | 6261.5 ± 787.6 ms | n/a | n/a |
| microservices | n/a | 10006.1 ± 385.5 ms | 12.5 ± 0.9 ms | 21.2 ± 1.2 ms | n/a | n/a |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | _insufficient data_ | — | — | — | — |
| monolith vs microservices | _insufficient data_ | — | — | — | — |
| separated vs microservices | _insufficient data_ | — | — | — | — |

## RAMPUP load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 42001.7 ± 24647.3 ms | 7.11 ± 1.18 req/s | 0.00% ± 0.00% | 58.68 | 5 |
| separated | 60001.3 ± 0.4 ms | 2.33 ± 1.20 req/s | 0.00% ± 0.00% | 0.0 | 5 |
| microservices | 24006.3 ± 20122.1 ms | 10.39 ± 1.08 req/s | 0.00% ± 0.00% | 83.82 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | n/a | 42001.5 ± 24647.1 ms | 35828.4 ± 22809.1 ms | 60001.4 ± 1.1 ms | n/a | n/a |
| separated | n/a | 60001.2 ± 0.6 ms | 60001.3 ± 0.4 ms | 60000.6 ± 0.0 ms | n/a | n/a |
| microservices | n/a | 24008.1 ± 20121.3 ms | 6.8 ± 6.9 ms | 24.6 ± 0.0 ms | n/a | n/a |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | _insufficient data_ | — | — | — | — |
| monolith vs microservices | _insufficient data_ | — | — | — | — |
| separated vs microservices | _insufficient data_ | — | — | — | — |

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
