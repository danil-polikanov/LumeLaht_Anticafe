#!/bin/bash
# Verify API is ready: /api/room must return at least 10 rooms
# Usage: ./health-check.sh [base_url] [timeout_seconds]

set -euo pipefail

BASE_URL="${1:-http://127.0.0.1:3000}"
TIMEOUT="${2:-90}"
MIN_ROOMS=10

echo "[health] Waiting for ${BASE_URL}/api/room (timeout ${TIMEOUT}s, min ${MIN_ROOMS} rooms)..."

START=$(date +%s)
while true; do
    NOW=$(date +%s)
    ELAPSED=$((NOW - START))
    if (( ELAPSED > TIMEOUT )); then
        echo "[health] ✘ Timeout after ${TIMEOUT}s"
        exit 1
    fi

    # Try to get rooms count; tolerate failures silently until timeout.
    # Uses grep instead of jq for portability (Git Bash on Windows lacks jq).
    COUNT=$(curl -sf --max-time 5 "${BASE_URL}/api/room" 2>/dev/null | grep -oE '"roomId"' | wc -l | tr -d ' \r\n' || echo "")

    if [[ "$COUNT" =~ ^[0-9]+$ ]] && (( COUNT >= MIN_ROOMS )); then
        echo "[health] ✔ API ready (${COUNT} rooms, ${ELAPSED}s)"
        exit 0
    fi

    sleep 3
done
