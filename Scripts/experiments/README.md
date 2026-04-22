# Experiment Automation — LumeLaht Performance Benchmark

Automated pipeline for running 30 k6 experiments (3 architectures × 2 profiles × 5 repetitions) unattended, with statistical analysis.

## Prerequisites (Mac mini)

```bash
# 1. Docker Desktop running
# 2. k6 installed natively
brew install k6

# 3. Python 3 with bcrypt + scipy for analysis
pip3 install bcrypt scipy numpy

# 4. jq for JSON parsing
brew install jq

# 5. Plug charger in, disable sleep:
caffeinate -dimsu &
# or System Settings → Battery → Never Sleep when plugged in

# 6. Verify 20+ GB free disk
df -h ~
```

## Quick Start

```bash
cd /path/to/LumeLaht_Anticafe

# Run ALL 30 experiments (~6.5 hours):
./Scripts/experiments/run-experiments.sh all

# Or run one architecture at a time:
./Scripts/experiments/run-experiments.sh monolith
./Scripts/experiments/run-experiments.sh separated
./Scripts/experiments/run-experiments.sh microservices

# After completion, generate statistical report:
python3 Scripts/experiments/analyze-results.py
```

## Files

| File | Purpose |
|---|---|
| `run-experiments.sh` | Master orchestrator — runs all 30 experiments sequentially |
| `seed-db.sh` | Populates DB with 10k users + 50k historical bookings |
| `reset-between-reps.sh` | Cleans k6-generated data between repetitions (fast) |
| `health-check.sh` | Verifies API is responding before starting a test |
| `warmup.sh` | 2-min light k6 load to warm JIT/caches |
| `analyze-results.py` | Post-hoc statistical analysis (mean/std/Mann-Whitney/Cliff's δ) |

## Output

All results go into `results/`:
- `results/run-YYYYMMDD-HHMMSS.log` — full run log
- `results/{arch}_{profile}_rep{N}_summary.json` — k6 summary per run
- `results/{arch}_{profile}_rep{N}_raw.json` — k6 raw metrics per run
- `results/analysis.md` — generated tables for thesis (after analyze-results.py)

## Timing

| Phase | Duration |
|---|---|
| Compose up + healthy wait | ~3 min per arch |
| DB seed (10k users + 50k bookings) | ~1 min per arch |
| Warmup | ~2 min per arch |
| Constant rep (5m test + cleanup) | ~6 min × 5 = 30 min per arch |
| Rampup rep (7m test + cleanup) | ~8 min × 5 = 40 min per arch |
| Compose down -v | ~1 min per arch |
| **Per architecture total** | **~1h 20min** |
| **Full run (3 archs)** | **~4-5 hours** |

## Troubleshooting

- **`docker-compose` not found:** on newer Docker Desktop it's `docker compose` (no dash). The scripts use `docker compose`.
- **Port 3000/9090 already taken:** `docker ps` → stop conflicting containers.
- **API health check fails:** check `docker compose -f docker-compose.{arch}.yml logs api` (or `user-service` for microservices).
- **bcrypt hash mismatch:** regenerate via `python3 -c "import bcrypt; print(bcrypt.hashpw(b'SeedPass123!', bcrypt.gensalt(10)).decode())"`.
