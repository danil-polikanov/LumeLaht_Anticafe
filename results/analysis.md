# Statistical Analysis of Performance Experiments


Generated: 2026-05-11T11:13:32.131477


All values reported as **mean ± std** across 5 repetitions per condition.

Coefficient of variation (CV) should be < 5% for reliable measurements.


## CONSTANT load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 255.9 ± 33.1 ms | 23.36 ± 0.16 req/s | 0.00% ± 0.00% | 12.92 | 5 |
| separated | 201.6 ± 40.0 ms | 23.65 ± 0.18 req/s | 0.00% ± 0.00% | 19.83 | 5 |
| microservices | 1485.0 ± 120.5 ms | 20.59 ± 0.18 req/s | 0.00% ± 0.00% | 8.11 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | 382.1 ± 68.3 ms | 347.5 ± 54.7 ms | 98.6 ± 46.0 ms | 159.3 ± 68.8 ms | 128.2 ± 41.4 ms | 155.0 ± 64.8 ms |
| separated | 296.6 ± 64.8 ms | 271.1 ± 61.2 ms | 68.8 ± 47.4 ms | 91.6 ± 61.8 ms | 73.3 ± 47.8 ms | 93.5 ± 63.3 ms |
| microservices | 2547.1 ± 211.6 ms | 2399.4 ± 320.6 ms | 9.6 ± 0.7 ms | 20.1 ± 3.9 ms | 9.4 ± 0.2 ms | 16.1 ± 3.4 ms |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | 0.0556 | ✘ no | 0.76 | large | separated |
| monolith vs microservices | 0.0079 | ✔ yes | -1.0 | large | monolith |
| separated vs microservices | 0.0079 | ✔ yes | -1.0 | large | separated |

## RAMPUP load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 6739.5 ± 219.0 ms | 35.68 ± 0.10 req/s | 0.00% ± 0.00% | 3.25 | 5 |
| separated | 5132.5 ± 168.3 ms | 42.74 ± 0.49 req/s | 0.00% ± 0.00% | 3.28 | 5 |
| microservices | 23383.8 ± 1250.1 ms | 24.58 ± 0.62 req/s | 0.00% ± 0.00% | 5.35 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | 8255.4 ± 301.4 ms | 7355.2 ± 484.4 ms | 5105.0 ± 258.6 ms | 7388.2 ± 188.7 ms | 5121.5 ± 226.7 ms | 5997.6 ± 255.2 ms |
| separated | 6378.1 ± 258.1 ms | 5327.6 ± 261.4 ms | 3866.3 ± 116.3 ms | 5976.0 ± 171.5 ms | 3940.8 ± 229.8 ms | 4877.6 ± 266.9 ms |
| microservices | 35291.2 ± 2848.9 ms | 26602.9 ± 4011.5 ms | 91.9 ± 32.5 ms | 37.3 ± 3.6 ms | 15.8 ± 2.3 ms | 23.3 ± 1.8 ms |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | 0.0079 | ✔ yes | 1.0 | large | separated |
| monolith vs microservices | 0.0079 | ✔ yes | -1.0 | large | monolith |
| separated vs microservices | 0.0079 | ✔ yes | -1.0 | large | separated |

## SPIKE load profile


### Table: Overall response time (p95), throughput, error rate

| Architecture | p95 latency | Throughput | Error rate | CV p95 (%) | n |
|---|---|---|---|---|---|
| monolith | 17534.9 ± 779.0 ms | 25.74 ± 1.30 req/s | 0.00% ± 0.00% | 4.44 | 5 |
| separated | 16356.7 ± 803.1 ms | 27.39 ± 0.97 req/s | 0.00% ± 0.00% | 4.91 | 5 |
| microservices | 31245.5 ± 1208.2 ms | 23.49 ± 1.18 req/s | 0.00% ± 0.00% | 3.87 | 5 |

### Table: Response time by operation (p95, mean ± std)

| Architecture | Register | Login | Rooms | Booking | My Bookings | Cancel |
|---|---|---|---|---|---|---|
| monolith | 19745.1 ± 1292.8 ms | 13150.5 ± 940.2 ms | 10990.2 ± 544.2 ms | 23130.4 ± 918.9 ms | 10512.0 ± 837.3 ms | 15355.1 ± 637.5 ms |
| separated | 18831.7 ± 1010.3 ms | 11888.5 ± 838.0 ms | 10251.1 ± 459.3 ms | 21070.1 ± 287.3 ms | 9979.2 ± 610.1 ms | 13999.8 ± 584.0 ms |
| microservices | 42562.1 ± 4324.7 ms | 27563.3 ± 3286.9 ms | 192.8 ± 71.3 ms | 36.8 ± 8.6 ms | 18.8 ± 4.9 ms | 22.4 ± 3.7 ms |

### Pairwise comparisons (Mann-Whitney U, Cliff's δ)

| Comparison | p-value | Significant (α=0.05) | Cliff's δ | Effect size | Faster |
|---|---|---|---|---|---|
| monolith vs separated | 0.0556 | ✘ no | 0.76 | large | separated |
| monolith vs microservices | 0.0079 | ✔ yes | -1.0 | large | monolith |
| separated vs microservices | 0.0079 | ✔ yes | -1.0 | large | separated |

## Resource consumption (docker stats during benchmark)

Sums across all workload containers (monitoring sidecars excluded).

CPU% is normalized: 100% = 1 logical core fully used.


### CONSTANT — observed resource usage

| Architecture | Peak CPU% | Avg CPU% | Peak RAM (MiB) | Avg RAM (MiB) | n |
|---|---|---|---|---|---|
| monolith | 211 ± 5% | 97 ± 3% | 1484 ± 40 | 1446 ± 72 | 5 |
| separated | 190 ± 9% | 89 ± 3% | 1121 ± 55 | 1093 ± 68 | 5 |
| microservices | 176 ± 83% | 86 ± 3% | 4275 ± 117 | 4222 ± 200 | 5 |

#### Per-container peak (first rep)

| Architecture | Container | Peak CPU% | Peak RAM (MiB) |
|---|---|---|---|
| monolith | lumelaht_anticafe-app-1 | 178% | 189 |
| monolith | lumelaht_anticafe-db-1 | 106% | 1225 |
| separated | lumelaht_anticafe-api-1 | 179% | 160 |
| separated | lumelaht_anticafe-db-1 | 98% | 868 |
| separated | lumelaht_anticafe-frontend-1 | 2% | 16 |
| microservices | lumelaht_anticafe-user-service-1 | 109% | 126 |
| microservices | lumelaht_anticafe-room-db-1 | 85% | 1163 |
| microservices | lumelaht_anticafe-booking-db-1 | 77% | 1228 |
| microservices | lumelaht_anticafe-user-db-1 | 75% | 1244 |
| microservices | lumelaht_anticafe-booking-service-1 | 15% | 127 |
| microservices | lumelaht_anticafe-room-service-1 | 12% | 129 |
| microservices | lumelaht_anticafe-api-gateway-1 | 8% | 51 |
| microservices | lumelaht_anticafe-frontend-1 | 1% | 16 |

### RAMPUP — observed resource usage

| Architecture | Peak CPU% | Avg CPU% | Peak RAM (MiB) | Avg RAM (MiB) | n |
|---|---|---|---|---|---|
| monolith | 231 ± 9% | 166 ± 2% | 1631 ± 23 | 1601 ± 30 | 5 |
| separated | 221 ± 2% | 160 ± 1% | 1325 ± 33 | 1268 ± 39 | 5 |
| microservices | 156 ± 9% | 104 ± 1% | 4516 ± 39 | 4466 ± 54 | 5 |

#### Per-container peak (first rep)

| Architecture | Container | Peak CPU% | Peak RAM (MiB) |
|---|---|---|---|
| monolith | lumelaht_anticafe-app-1 | 211% | 279 |
| monolith | lumelaht_anticafe-db-1 | 20% | 1325 |
| separated | lumelaht_anticafe-api-1 | 209% | 306 |
| separated | lumelaht_anticafe-db-1 | 15% | 950 |
| separated | lumelaht_anticafe-frontend-1 | 2% | 19 |
| microservices | lumelaht_anticafe-user-service-1 | 110% | 213 |
| microservices | lumelaht_anticafe-room-service-1 | 22% | 188 |
| microservices | lumelaht_anticafe-user-db-1 | 12% | 1292 |
| microservices | lumelaht_anticafe-booking-service-1 | 12% | 166 |
| microservices | lumelaht_anticafe-room-db-1 | 11% | 1237 |
| microservices | lumelaht_anticafe-booking-db-1 | 11% | 1288 |
| microservices | lumelaht_anticafe-api-gateway-1 | 6% | 71 |
| microservices | lumelaht_anticafe-frontend-1 | 2% | 19 |

### SPIKE — observed resource usage

| Architecture | Peak CPU% | Avg CPU% | Peak RAM (MiB) | Avg RAM (MiB) | n |
|---|---|---|---|---|---|
| monolith | 234 ± 4% | 104 ± 2% | 1689 ± 55 | 1611 ± 93 | 5 |
| separated | 229 ± 4% | 101 ± 1% | 1438 ± 7 | 1367 ± 11 | 5 |
| microservices | 180 ± 21% | 77 ± 2% | 4696 ± 28 | 4633 ± 31 | 5 |

#### Per-container peak (first rep)

| Architecture | Container | Peak CPU% | Peak RAM (MiB) |
|---|---|---|---|
| monolith | lumelaht_anticafe-app-1 | 214% | 380 |
| monolith | lumelaht_anticafe-db-1 | 28% | 1296 |
| separated | lumelaht_anticafe-api-1 | 213% | 412 |
| separated | lumelaht_anticafe-db-1 | 26% | 1014 |
| separated | lumelaht_anticafe-frontend-1 | 4% | 24 |
| microservices | lumelaht_anticafe-user-service-1 | 107% | 277 |
| microservices | lumelaht_anticafe-room-service-1 | 22% | 226 |
| microservices | lumelaht_anticafe-booking-db-1 | 21% | 1319 |
| microservices | lumelaht_anticafe-booking-service-1 | 13% | 177 |
| microservices | lumelaht_anticafe-room-db-1 | 13% | 1244 |
| microservices | lumelaht_anticafe-api-gateway-1 | 7% | 94 |
| microservices | lumelaht_anticafe-user-db-1 | 7% | 1307 |
| microservices | lumelaht_anticafe-frontend-1 | 4% | 23 |

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
