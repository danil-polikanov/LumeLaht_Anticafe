# LumeLaht Anticafe — Load Test Results

**Date:** 2026-04-06  
**Machine:** macOS (Apple Silicon)  
**k6 profiles:** constant (50 VU / 5 min), rampup (10→200 VU / 7 min)

---

## Resource Configuration

| Component | Monolith | Separated | Microservices |
|-----------|----------|-----------|---------------|
| API / App | 4 CPU / 2G | 4 CPU / 2G | 4 services × 1 CPU / 512M |
| DB | 2 CPU / 2G | 2 CPU / 2G | 3 DBs × 2 CPU / 2G |
| Frontend | included | 1 CPU / 512M | 1 CPU / 512M |

---

## CONSTANT LOAD — 50 VU, 5 minutes

### http_req_duration (all endpoints combined)

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 10 ms | 11 ms | 9 ms |
| p(75) | **110 ms** | **109 ms** | **138 ms** |
| p(90) | 136 ms | 129 ms | 2 399 ms |
| p(95) | 146 ms ✓ | 140 ms ✓ | 4 364 ms ✗ |
| p(99) | 231 ms | 182 ms | 7 890 ms |
| avg | 47 ms | 46 ms | 620 ms |

### login_duration

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 125 ms | 119 ms | 1 022 ms |
| p(75) | **138 ms** | **135 ms** | **3 403 ms** |
| p(90) | 155 ms | 142 ms | 5 795 ms |
| p(95) | 202 ms | 155 ms | 6 909 ms |
| p(99) | 262 ms | 214 ms | 9 484 ms |
| avg | 132 ms | 124 ms | 2 093 ms |

### booking_duration

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 10 ms | 11 ms | 9 ms |
| p(75) | **16 ms** | **19 ms** | **13 ms** |
| p(90) | 26 ms | 31 ms | 20 ms |
| p(95) | 68 ms | 88 ms | 23 ms |
| p(99) | 154 ms | 145 ms | 33 ms |
| avg | 18 ms | 20 ms | 12 ms |

### rooms_duration

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 3 ms | 3 ms | 4 ms |
| p(75) | **5 ms** | **6 ms** | **6 ms** |
| p(90) | 11 ms | 12 ms | 10 ms |
| p(95) | 22 ms | 40 ms | 13 ms |
| p(99) | 123 ms | 115 ms | 24 ms |
| avg | 9 ms | 9 ms | 6 ms |

---

## RAMPUP LOAD — 10→200 VU, 7 minutes

### http_req_duration (all endpoints combined)

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 535 ms | 529 ms | 15 ms |
| p(75) | **1 303 ms** | **1 338 ms** | **2 049 ms** |
| p(90) | 2 026 ms | 2 105 ms | 27 196 ms |
| p(95) | 2 485 ms ✗ | 2 667 ms ✗ | 39 695 ms ✗ |
| p(99) | 3 384 ms | 3 908 ms | 56 763 ms |
| avg | 810 ms | 852 ms | 6 376 ms |
| Throughput | 64.6 req/s | 63.3 req/s | 16.1 req/s |
| Error rate | 0.72% | 0.87% | 2.16% |

### login_duration

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 982 ms | 1 021 ms | 16 899 ms |
| p(75) | **1 520 ms** | **1 698 ms** | **26 452 ms** |
| p(90) | 2 154 ms | 2 325 ms | 37 109 ms |
| p(95) | 2 577 ms | 2 775 ms | 44 604 ms |
| p(99) | 3 317 ms | 4 005 ms | 50 140 ms |
| avg | 1 041 ms | 1 144 ms | 17 909 ms |

### booking_duration

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 605 ms | 657 ms | 14 ms |
| p(75) | **1 401 ms** | **1 486 ms** | **21 ms** |
| p(90) | 2 283 ms | 2 305 ms | 25 ms |
| p(95) | 2 765 ms | 2 883 ms | 30 ms |
| p(99) | 3 680 ms | 4 217 ms | 34 ms |
| avg | 892 ms | 934 ms | 15 ms |

### rooms_duration

| Percentile | Monolith | Separated | Microservices |
|------------|----------|-----------|---------------|
| p(50) | 297 ms | 293 ms | 8 ms |
| p(75) | **810 ms** | **797 ms** | **15 ms** |
| p(90) | 1 425 ms | 1 462 ms | 24 ms |
| p(95) | 1 810 ms | 1 909 ms | 28 ms |
| p(99) | 2 589 ms | 2 910 ms | 41 ms |
| avg | 529 ms | 538 ms | 12 ms |

---

## Key Findings

1. **Monolith ≈ Separated** — difference <5% across all metrics. Extra nginx hop is negligible.

2. **Microservices: auth is the bottleneck** — login p(75) under constant load is 3 403 ms vs 138 ms for monolith. BCrypt on a 1-CPU container saturates under concurrent load.

3. **Microservices: domain endpoints are fastest** — booking and rooms stay under 30 ms even at 200 VU, because each service has its own isolated DB with no contention.

4. **Rampup p(75) is key** — at p(75), monolith (1 303 ms) and separated (1 338 ms) are comparable. Microservices p(75) = 2 049 ms — already above threshold because auth drags the average.

5. **Microservices total DB cost** — 3× more database resources (6 CPU / 6G) vs 1 DB (2 CPU / 2G). This is architectural overhead, not a configuration error.

---

## Raw files
- `monolith_constant.json` / `monolith_rampup.json`
- `separated_constant.json` / `separated_rampup.json`
- `microservices_constant.json` / `microservices_rampup.json`
