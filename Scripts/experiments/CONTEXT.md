# Thesis Benchmark Context — Handoff Document

**Purpose:** Self-contained context document so any Claude session can pick up work on this benchmark pipeline without prior conversation history. Show this file to Claude with a message like *"read Scripts/experiments/CONTEXT.md and let's continue"*.

**Last updated:** 2026-04-22

---

## What this project is

Bachelor's thesis at TalTech: **"Comparative Analysis of Web Application Architectures"**.

Compares three architectures of the LumeLaht anti-cafe booking system:
- **Monolith** — one .NET 10 app serving API + static React build
- **Separated** — React SPA + standalone .NET 10 API (separate Docker containers)
- **Microservices** — React SPA + 3 .NET services (Room/User/Booking) + YARP API Gateway

Stack: .NET 10, React 19, SQL Server 2022, Docker Compose, k6 load testing, Prometheus + Grafana.

**Supervisor:** Andre Sääsk, MA.
**Defense language:** Estonian (slides). **Thesis text:** English (`Polikanov_prediploma_2026.docx`, gitignored).
**Pre-defense (prediploma):** April–May 2026.

---

## Current state (what's done / what's pending)

### ✅ Done

- All 3 docker-compose stacks built and tested manually
- k6 load test script at [Scripts/k6/load-test.js](../k6/load-test.js) (4 profiles: constant/rampup/spike/soak)
- Full automation pipeline in [Scripts/experiments/](.) — see "Pipeline files" below
- Russian draft of thesis text (newer than English, but English is canonical)
- English `.docx` exists with full structure: Summary, Intro, ch.1–3, Kokkuvõte, References

### ⏳ Pending — to do AFTER overnight run completes on Mac mini

1. Execute pipeline on Mac (instructions: [README.md](README.md)) → produces `results/analysis.md`
2. Update **English `.docx`** with real numbers in Tables 1–4 (currently empty headers)
3. Add explicit research questions (RQ1/RQ2/RQ3) to introduction
4. Add explicit hypothesis to introduction
5. Cite **Blinowski et al. (2022)** in references and Section 1.2 (mandatory — see "Known weaknesses" below)
6. Fix Azure VM vs Mac mini M2 inconsistency (Section 2.1 says Azure, Section 3.1 says Mac mini — must be Mac mini everywhere)
7. Run thesis formatting Python scripts (also gitignored, in `Scripts/`): `add_sisukord.py`, `add_license_authorship.py`, `create_kokkuvote_resumee.py`, `update_chapter3_real_results.py`
8. Build Estonian defense slides (10–12 slides) from updated thesis
9. Verify with supervisor that English text is acceptable

---

## Pipeline files (in this directory)

| File | What it does |
|---|---|
| [README.md](README.md) | User-facing instructions for running on Mac |
| [prepare-mac.sh](prepare-mac.sh) | Pre-flight: checks docker, k6, jq, python+bcrypt+scipy, disk, RAM, ports |
| [smoke-test.sh](smoke-test.sh) | 1-rep validation (~20 min) — MUST pass before overnight run |
| [run-experiments.sh](run-experiments.sh) | Master orchestrator: 3 archs × 2 profiles × 5 reps = 30 experiments, ~5 hours |
| [seed-db.sh](seed-db.sh) | Seeds 10k users (`@seed.local`, shared bcrypt hash via Python) + 50k historical Completed bookings |
| [reset-between-reps.sh](reset-between-reps.sh) | Removes only k6-created data (`@test.com` users + non-Completed bookings); preserves seed |
| [health-check.sh](health-check.sh) | Polls `/api/room` until ≥10 rooms returned; up to 180s timeout |
| [warmup.sh](warmup.sh) | 2-min light load (5 VU) before measurements |
| [analyze-results.py](analyze-results.py) | Generates `results/analysis.md` with mean±std, Mann-Whitney U, Cliff's δ |

---

## Database topology (critical for SQL operations)

| Architecture | Container name | Database name | Tables present |
|---|---|---|---|
| Monolith | `lumelaht_anticafe-db-1` | `LumeLaht_Monolith` | Users, Bookings, Rooms |
| Separated | `lumelaht_anticafe-db-1` | `LumeLaht_Separated` | Users, Bookings, Rooms |
| Microservices — UserService | `lumelaht_anticafe-user-db-1` | `LumeLaht_UserDb` | Users only |
| Microservices — BookingService | `lumelaht_anticafe-booking-db-1` | `LumeLaht_BookingDb` | Bookings only (no FK to Users/Rooms) |
| Microservices — RoomService | `lumelaht_anticafe-room-db-1` | `LumeLaht_RoomDb` | Rooms, Addresses, Activities |

**SA password:** `LumeLaht_Pass123!`

**IMPORTANT for sqlcmd:** ALWAYS use `-x` flag — disables `$(var)` substitution so bcrypt hashes (containing literal `$2b$10$`) are not misinterpreted as variables.

---

## k6 conventions (matter for cleanup logic)

- k6 creates users with emails `user_<VU>_<ITER>_<timestamp>@test.com`
- Seed creates users with emails `seed_user_<N>@seed.local`
- k6 bookings have Status `Confirmed` (default) or `Cancelled` (after k6 calls DELETE)
- Seed bookings have Status `Completed` (historical, in past)
- All architectures expose API at `http://127.0.0.1:3000/api/*` (nginx proxies for separated/microservices, direct for monolith)

---

## Known thesis weaknesses (must address before defense)

### Critical

1. **Blinowski et al. (2022)** *"Monolithic vs. Microservice Architecture: A Performance and Scalability Evaluation"*, IEEE Access 10, 20357–20374, DOI 10.1109/ACCESS.2022.3152803 — **direct competitor** that already compared monolith vs microservices on C#.NET. **MUST be cited** and differentiated. Differentiators are:
   - This work compares **three** architectures (Blinowski did two)
   - Adds **separated (SPA + API)** as a distinct third category — novel framing
   - Modern stack (.NET 10 from 2025 vs Blinowski's 2022 C#.NET ~5/6)
   - Local Docker (full ops control) vs their Azure (vendor confounds)
   - Quantified bcrypt CPU bottleneck formula as practical recommendation

2. **n must be ≥ 5** for Mann-Whitney U to mathematically reach p < 0.05 (n=4 → minimum 2-tailed p = 0.057). Pipeline already configured for 5 reps; do not reduce.

3. **UserService 1 CPU bottleneck must be reframed** — not as "microservices are slower" but as "microservices require careful CPU allocation; misallocation of even one service degrades the whole system 30×". Cite industry bcrypt benchmarks (e.g., cybersierra.co): bcrypt cost=10 needs ~3.25 CPU at 50 RPS.

### Less critical but visible

4. Tables 1–4 in English `.docx` are empty headers — fill from `results/analysis.md` after run
5. Fix "Azure VM 4 CPU/16 GB" reference in Section 2.1 → should be "Mac mini M2 8-core/24 GB" (Section 3.1 already says this)
6. Add explicit hypothesis to introduction (currently implicit only)
7. Add explicit RQ1/RQ2/RQ3 to introduction
8. k6 version reference: actual is v1.7.1 (was v1.7.0 in earlier draft; either is fine, just be consistent with what was actually used)
9. TechEmpower was archived 2026-03-24 — useful talking point about why independent benchmarks are needed now

### Deferred to final diploma (NOT prediploma)

- ❌ RabbitMQ (already in "future work" sections; do not implement now)
- ❌ Spike load profile
- ❌ Additional metrics (GC, disk I/O, network)
- ❌ Distributed tracing (Jaeger/Zipkin)
- ❌ Migration to x86 hardware

---

## Defense Q&A cheat sheet

| Likely question | Short answer |
|---|---|
| What is your hypothesis? | "Microservices outperform monolith under load" — was **falsified** at this scale (strong, valid finding) |
| Why only 5 reps? | Mann-Whitney U requires n≥5 for p<0.05; balances rigor and 5-hour overnight time budget |
| Why bcrypt? | OWASP standard, identical across all 3 architectures → fair comparison |
| Why Mac mini, not production hardware? | Goal is **relative** comparison, not absolute throughput. Relative ratios hold on x86 |
| What's your contribution vs Blinowski 2022? | Three architectures (not two), separated as distinct category, .NET 10 stack, quantified bcrypt CPU formula |
| Why no statistical significance test in earlier draft? | Earlier draft had n=1 (limitation acknowledged); current data has n=5 with Mann-Whitney U + Cliff's δ |

---

## Resume phrases for new Claude sessions

After running pipeline on Mac, paste any of these to Claude (after sharing this file):

- *"read Scripts/experiments/CONTEXT.md and let's continue — overnight run done"*
- *"smoke test failed with this output: [paste]"*
- *"results/analysis.md is ready, help me update Tables 1–4 in the English .docx"*
- *"need to draft 10 Estonian defense slides from updated thesis"*

---

## Repo etiquette

From [CLAUDE.md](../../CLAUDE.md) at repo root:
- Branch: `feature/microservices-architecture` (current)
- Commit format: `<type>(<scope>): <Subject>` — types: feat/fix/refactor/test/chore/docs/perf/ci; scopes: api/rooms/booking/auth/frontend/docker/testing/infra
- Communicate with user in Russian; write code/comments in English
- Never commit, push, or create PRs/issues without explicit user request
- Read before edit; respect Clean Architecture layer boundaries
