#!/bin/bash
# Fast reset between k6 repetitions — removes only k6-generated data, keeps seed intact.
# k6 users: email ends with @test.com
# k6 bookings: Status IN ('Confirmed', 'Cancelled') (seed uses 'Completed')
# Usage: ./reset-between-reps.sh <monolith|separated|microservices>

set -euo pipefail

ARCH="${1:-}"
SA_PASSWORD='LumeLaht_Pass123!'

case "$ARCH" in
    monolith)
        USER_DB_CONTAINER="lumelaht_anticafe-db-1"; USER_DB_NAME="LumeLaht_Monolith"
        BOOKING_DB_CONTAINER="lumelaht_anticafe-db-1"; BOOKING_DB_NAME="LumeLaht_Monolith"
        ;;
    separated)
        USER_DB_CONTAINER="lumelaht_anticafe-db-1"; USER_DB_NAME="LumeLaht_Separated"
        BOOKING_DB_CONTAINER="lumelaht_anticafe-db-1"; BOOKING_DB_NAME="LumeLaht_Separated"
        ;;
    microservices)
        USER_DB_CONTAINER="lumelaht_anticafe-user-db-1"; USER_DB_NAME="LumeLaht_UserDb"
        BOOKING_DB_CONTAINER="lumelaht_anticafe-booking-db-1"; BOOKING_DB_NAME="LumeLaht_BookingDb"
        ;;
    *) echo "Usage: $0 <monolith|separated|microservices>"; exit 1 ;;
esac

sqlcmd_exec() {
    local container="$1"; local db="$2"; local sql="$3"
    docker exec -i "$container" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C -x -d "$db" -Q "$sql" >/dev/null
}

echo "[reset] Cleaning k6 data in $ARCH..."
# Delete k6 bookings (non-Completed statuses; seed uses 'Completed')
sqlcmd_exec "$BOOKING_DB_CONTAINER" "$BOOKING_DB_NAME" \
    "DELETE FROM Bookings WHERE Status <> 'Completed';"

# Delete ONLY k6-created users (@test.com); seed users and any admin stay intact
sqlcmd_exec "$USER_DB_CONTAINER" "$USER_DB_NAME" \
    "DELETE FROM Users WHERE Email LIKE '%@test.com';"

echo "[reset] ✔ k6 data removed, seed preserved"
