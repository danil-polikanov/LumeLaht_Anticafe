#!/bin/bash
# Master orchestrator: runs 5 repetitions × 2 profiles for each architecture.
# Usage: ./run-experiments.sh [all | monolith | separated | microservices]
#
# For each architecture:
#   1. docker compose up -d (with full volume reset)
#   2. Wait for containers healthy
#   3. Run EF migrations if needed (container auto-migrates on startup)
#   4. Seed 10k users + 50k bookings
#   5. Health-check API
#   6. Warmup (2 min)
#   7. 5 reps × constant profile (with reset between reps)
#   8. 5 reps × rampup profile
#   9. docker compose down -v

set -euo pipefail

# ─── Configuration ─────────────────────────────────────────────────────────────
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"
RESULTS_DIR="${REPO_ROOT}/results"
K6_SCRIPT="${REPO_ROOT}/Scripts/k6/load-test.js"
BASE_URL="http://127.0.0.1:3000"
N_REPS=5
PROFILES=("constant" "rampup")

TARGET="${1:-all}"
REPS_OVERRIDE="${2:-}"
if [[ -n "$REPS_OVERRIDE" ]]; then
    N_REPS="$REPS_OVERRIDE"
fi
if [[ "$TARGET" == "all" ]]; then
    ARCHITECTURES=("monolith" "separated" "microservices")
else
    ARCHITECTURES=("$TARGET")
fi

# Smoke test mode: ./run-experiments.sh monolith 1 → 1 rep per profile for fast validation

TIMESTAMP=$(date +%Y%m%d-%H%M%S)
LOG_FILE="${RESULTS_DIR}/run-${TIMESTAMP}.log"
CHECKPOINT_FILE="${RESULTS_DIR}/checkpoint.txt"

mkdir -p "${RESULTS_DIR}"

# ─── Logging ──────────────────────────────────────────────────────────────────
log() {
    local ts; ts=$(date '+%Y-%m-%d %H:%M:%S')
    echo "[${ts}] $*" | tee -a "${LOG_FILE}"
}

# ─── Helpers ──────────────────────────────────────────────────────────────────

start_stack() {
    local arch="$1"
    log "──── [$arch] docker compose up -d ────"
    cd "${REPO_ROOT}"
    docker compose -f "docker-compose.${arch}.yml" down -v 2>/dev/null || true
    docker compose -f "docker-compose.${arch}.yml" up -d --build >> "${LOG_FILE}" 2>&1
}

stop_stack() {
    local arch="$1"
    log "──── [$arch] docker compose down -v ────"
    cd "${REPO_ROOT}"
    docker compose -f "docker-compose.${arch}.yml" down -v >> "${LOG_FILE}" 2>&1 || true
}

wait_for_healthy() {
    local arch="$1"
    local max_wait=180
    log "[$arch] Waiting up to ${max_wait}s for containers to be healthy..."
    bash "${SCRIPT_DIR}/health-check.sh" "${BASE_URL}" "${max_wait}" 2>&1 | tee -a "${LOG_FILE}"
}

seed() {
    local arch="$1"
    log "[$arch] Seeding 10k users + 50k historical bookings..."
    bash "${SCRIPT_DIR}/seed-db.sh" "$arch" 2>&1 | tee -a "${LOG_FILE}"
}

warmup() {
    local arch="$1"
    log "[$arch] Warmup (2 min)..."
    bash "${SCRIPT_DIR}/warmup.sh" "${BASE_URL}" 2>&1 | tee -a "${LOG_FILE}"
}

reset_reps() {
    local arch="$1"
    bash "${SCRIPT_DIR}/reset-between-reps.sh" "$arch" 2>&1 | tee -a "${LOG_FILE}"
}

run_single_test() {
    local arch="$1"
    local profile="$2"
    local rep="$3"
    local tag="${arch}_${profile}_rep${rep}"
    local summary_file="${RESULTS_DIR}/${tag}_summary.json"
    local raw_file="${RESULTS_DIR}/${tag}_raw.json"

    log "[$arch] ▶ ${profile} rep ${rep}/${N_REPS}"

    if ! k6 run \
        --env BASE_URL="${BASE_URL}" \
        --env PROFILE="${profile}" \
        --summary-export="${summary_file}" \
        --out "json=${raw_file}" \
        --tag arch="${arch}" \
        --tag profile="${profile}" \
        --tag rep="${rep}" \
        --quiet \
        "${K6_SCRIPT}" >> "${LOG_FILE}" 2>&1; then
        log "[$arch] ✘ ${profile} rep ${rep} FAILED — continuing to next rep"
    else
        log "[$arch] ✔ ${profile} rep ${rep} → ${summary_file##*/}"
    fi

    # Compress raw JSON to save disk (k6 raw can be hundreds of MB)
    if [[ -f "${raw_file}" ]]; then
        gzip -f "${raw_file}" 2>/dev/null || true
    fi

    # Brief cooldown so next rep starts clean
    sleep 30
}

cooldown_between_reps() {
    local arch="$1"
    reset_reps "$arch"
    sleep 15
}

# ─── Main loop ────────────────────────────────────────────────────────────────
log "════════════════════════════════════════════════════════════════════════"
log "LumeLaht Performance Benchmark — ${TIMESTAMP}"
log "Target: ${TARGET}  |  Architectures: ${ARCHITECTURES[*]}"
log "Profiles: ${PROFILES[*]}  |  Reps per profile: ${N_REPS}"
log "Log: ${LOG_FILE}"
log "════════════════════════════════════════════════════════════════════════"

OVERALL_START=$(date +%s)

for arch in "${ARCHITECTURES[@]}"; do
    ARCH_START=$(date +%s)
    log ""
    log "╔════════════════════════════════════════════════════════════════════╗"
    log "║  Architecture: ${arch}"
    log "╚════════════════════════════════════════════════════════════════════╝"

    start_stack "$arch"
    wait_for_healthy "$arch"
    seed "$arch"
    warmup "$arch"

    for profile in "${PROFILES[@]}"; do
        log ""
        log "── [$arch / $profile] ${N_REPS} repetitions ──"
        for rep in $(seq 1 "${N_REPS}"); do
            run_single_test "$arch" "$profile" "$rep"
            # Reset between reps (not after last of a profile — we switch profile then)
            if (( rep < N_REPS )); then
                cooldown_between_reps "$arch"
            fi
        done
        # Reset before switching profile
        cooldown_between_reps "$arch"
    done

    stop_stack "$arch"

    ARCH_END=$(date +%s)
    ARCH_MIN=$(( (ARCH_END - ARCH_START) / 60 ))
    log "[$arch] ✔ Complete in ${ARCH_MIN} minutes"
    echo "$arch" >> "${CHECKPOINT_FILE}"
done

OVERALL_END=$(date +%s)
TOTAL_MIN=$(( (OVERALL_END - OVERALL_START) / 60 ))

log ""
log "════════════════════════════════════════════════════════════════════════"
log "✔ ALL DONE — ${TOTAL_MIN} minutes total"
log "Results in: ${RESULTS_DIR}"
log "Next step: python3 ${SCRIPT_DIR}/analyze-results.py"
log "════════════════════════════════════════════════════════════════════════"
