#!/bin/bash
# Pre-flight check for Mac mini before overnight run.
# Verifies all dependencies + sets up the system for unattended testing.
#
# Usage: ./prepare-mac.sh

set -euo pipefail

RED=$'\033[0;31m'
GREEN=$'\033[0;32m'
YELLOW=$'\033[1;33m'
NC=$'\033[0m'

OK=0
FAIL=0

check() {
    local name="$1"; local cmd="$2"
    if eval "$cmd" >/dev/null 2>&1; then
        echo "${GREEN}✓${NC} ${name}"
        OK=$((OK+1))
    else
        echo "${RED}✗${NC} ${name} — MISSING"
        FAIL=$((FAIL+1))
    fi
}

echo "════════════════════════════════════════════════════════"
echo "Pre-flight check for overnight benchmark run"
echo "════════════════════════════════════════════════════════"

echo ""
echo "── Required tools ──"
check "docker"              "command -v docker"
check "docker compose v2"   "docker compose version"
check "k6"                  "command -v k6"
check "jq"                  "command -v jq"
check "python3"             "command -v python3"
check "python bcrypt"       "python3 -c 'import bcrypt'"
check "python scipy"        "python3 -c 'import scipy'"
check "python numpy"        "python3 -c 'import numpy'"

echo ""
echo "── System resources ──"

# Disk space check (need ≥ 20GB in repo dir and home)
AVAIL_GB=$(df -g "$(pwd)" 2>/dev/null | awk 'NR==2 {print $4}' || df -k "$(pwd)" | awk 'NR==2 {printf "%d", $4/1024/1024}')
if (( AVAIL_GB >= 20 )); then
    echo "${GREEN}✓${NC} Disk free: ${AVAIL_GB} GB (need ≥ 20 GB)"
    OK=$((OK+1))
else
    echo "${RED}✗${NC} Disk free: ${AVAIL_GB} GB — need ≥ 20 GB for Docker + results"
    FAIL=$((FAIL+1))
fi

# RAM check
TOTAL_GB=$(sysctl -n hw.memsize | awk '{printf "%d", $1/1024/1024/1024}')
if (( TOTAL_GB >= 16 )); then
    echo "${GREEN}✓${NC} RAM: ${TOTAL_GB} GB"
    OK=$((OK+1))
else
    echo "${YELLOW}⚠${NC} RAM: ${TOTAL_GB} GB — microservices needs ≥ 8 GB for containers"
fi

# Check Docker is running
if docker info >/dev/null 2>&1; then
    echo "${GREEN}✓${NC} Docker daemon responding"
    OK=$((OK+1))
else
    echo "${RED}✗${NC} Docker daemon not responding — start Docker Desktop"
    FAIL=$((FAIL+1))
fi

# Battery / charger
if command -v pmset >/dev/null 2>&1; then
    if pmset -g batt | grep -q 'AC Power'; then
        echo "${GREEN}✓${NC} AC power connected"
        OK=$((OK+1))
    else
        echo "${YELLOW}⚠${NC} Running on battery — plug in charger before overnight run"
    fi
fi

echo ""
echo "── Port availability (need 3000, 3001, 9090 free) ──"
for port in 3000 3001 9090; do
    if lsof -i ":$port" -sTCP:LISTEN >/dev/null 2>&1; then
        echo "${YELLOW}⚠${NC} Port $port is in use:"
        lsof -i ":$port" -sTCP:LISTEN | awk 'NR>1 {print "    "$1" (PID "$2")"}'
    else
        echo "${GREEN}✓${NC} Port $port free"
    fi
done

echo ""
echo "── Containers from previous runs ──"
EXISTING=$(docker ps -a --filter "name=lumelaht_anticafe" --format "{{.Names}}" || true)
if [[ -n "$EXISTING" ]]; then
    echo "${YELLOW}⚠${NC} Existing LumeLaht containers found:"
    echo "$EXISTING" | sed 's/^/    /'
    echo "    → will be removed by run-experiments.sh (docker compose down -v)"
else
    echo "${GREEN}✓${NC} No leftover LumeLaht containers"
fi

echo ""
echo "════════════════════════════════════════════════════════"
if (( FAIL == 0 )); then
    echo "${GREEN}✓ READY${NC} — ${OK} checks passed"
    echo ""
    echo "Recommended next steps:"
    echo "  1. Disable system sleep:       caffeinate -dimsu &"
    echo "  2. Quick smoke test (~20 min): ./Scripts/experiments/smoke-test.sh"
    echo "  3. Full overnight run (~5 h):  ./Scripts/experiments/run-experiments.sh all"
    echo "  4. After run completes:        python3 Scripts/experiments/analyze-results.py"
else
    echo "${RED}✗ NOT READY${NC} — ${FAIL} failed, ${OK} passed"
    echo ""
    echo "Install missing dependencies:"
    echo "  brew install k6 jq"
    echo "  pip3 install bcrypt scipy numpy"
    exit 1
fi
echo "════════════════════════════════════════════════════════"
