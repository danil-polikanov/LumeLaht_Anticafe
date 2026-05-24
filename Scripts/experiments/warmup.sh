#!/bin/bash
# 2-minute warmup: 5 VU constant load, output discarded.
# Purpose: warm JIT compilation, SQL Server buffer pool, TCP connection pools.
# Usage: ./warmup.sh [base_url]

set -euo pipefail

BASE_URL="${1:-http://127.0.0.1:3000}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K6_SCRIPT="${SCRIPT_DIR}/../k6/load-test.js"

echo "[warmup] Running 2-min warmup against ${BASE_URL}..."

k6 run \
    --env BASE_URL="${BASE_URL}" \
    --env PROFILE=constant \
    --duration 2m \
    --vus 5 \
    --no-summary \
    --quiet \
    "${K6_SCRIPT}" >/dev/null 2>&1 || true

echo "[warmup] ✔ Warmup complete"
