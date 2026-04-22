#!/bin/bash
# Smoke test before full overnight run.
# Runs monolith with 1 rep per profile (constant + rampup) in ~20 min.
# If this works, the full run will work.
#
# Usage: ./smoke-test.sh [monolith|separated|microservices]

set -euo pipefail

ARCH="${1:-monolith}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "════════════════════════════════════════════════════════"
echo "SMOKE TEST: $ARCH (1 rep per profile, ~20 min)"
echo "════════════════════════════════════════════════════════"
echo ""
echo "Checks to validate:"
echo "  ✓ Docker compose builds and starts"
echo "  ✓ API health check passes"
echo "  ✓ DB seed populates 10k users + 50k bookings"
echo "  ✓ k6 runs successfully for both profiles"
echo "  ✓ JSON output files are produced"
echo "  ✓ Reset between reps works"
echo ""

bash "${SCRIPT_DIR}/run-experiments.sh" "$ARCH" 1

echo ""
echo "════════════════════════════════════════════════════════"
echo "SMOKE TEST DONE. Check results/ for JSONs."
echo "If everything looks fine, run full suite:"
echo "    ./Scripts/experiments/run-experiments.sh all"
echo "════════════════════════════════════════════════════════"
