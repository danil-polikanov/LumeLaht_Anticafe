#!/bin/bash
# Seed database with 10k users + 50k historical bookings for one architecture.
# Usage: ./seed-db.sh <monolith|separated|microservices>

set -euo pipefail

ARCH="${1:-}"
if [[ -z "$ARCH" ]]; then
    echo "Usage: $0 <monolith|separated|microservices>"
    exit 1
fi

SA_PASSWORD='LumeLaht_Pass123!'
SEED_PASSWORD='SeedPass123!'
N_USERS=10000
N_BOOKINGS=50000

# Generate bcrypt hash for seed password (one hash, all users share it — perf-neutral)
echo "[seed] Generating bcrypt hash for seed password..."
HASH=$(python3 -c "import bcrypt; print(bcrypt.hashpw(b'${SEED_PASSWORD}', bcrypt.gensalt(10)).decode())")
if [[ -z "$HASH" ]]; then
    echo "[seed] ERROR: Python bcrypt not installed. Run: pip3 install bcrypt"
    exit 1
fi
echo "[seed] Hash: ${HASH:0:7}...${HASH: -7}"

# Determine DB topology by architecture
case "$ARCH" in
    monolith)
        DB_CONTAINERS=("lumelaht_anticafe-db-1")
        USER_DB_CONTAINER="lumelaht_anticafe-db-1"
        USER_DB_NAME="LumeLaht_Monolith"
        BOOKING_DB_CONTAINER="lumelaht_anticafe-db-1"
        BOOKING_DB_NAME="LumeLaht_Monolith"
        ROOM_DB_CONTAINER="lumelaht_anticafe-db-1"
        ROOM_DB_NAME="LumeLaht_Monolith"
        ;;
    separated)
        DB_CONTAINERS=("lumelaht_anticafe-db-1")
        USER_DB_CONTAINER="lumelaht_anticafe-db-1"
        USER_DB_NAME="LumeLaht_Separated"
        BOOKING_DB_CONTAINER="lumelaht_anticafe-db-1"
        BOOKING_DB_NAME="LumeLaht_Separated"
        ROOM_DB_CONTAINER="lumelaht_anticafe-db-1"
        ROOM_DB_NAME="LumeLaht_Separated"
        ;;
    microservices)
        DB_CONTAINERS=("lumelaht_anticafe-user-db-1" "lumelaht_anticafe-booking-db-1" "lumelaht_anticafe-room-db-1")
        USER_DB_CONTAINER="lumelaht_anticafe-user-db-1"
        USER_DB_NAME="LumeLaht_UserDb"
        BOOKING_DB_CONTAINER="lumelaht_anticafe-booking-db-1"
        BOOKING_DB_NAME="LumeLaht_BookingDb"
        ROOM_DB_CONTAINER="lumelaht_anticafe-room-db-1"
        ROOM_DB_NAME="LumeLaht_RoomDb"
        ;;
    *)
        echo "[seed] ERROR: unknown architecture '$ARCH'"
        exit 1
        ;;
esac

sqlcmd() {
    local container="$1"
    local db="$2"
    local sql="$3"
    # -x disables $(var) substitution so bcrypt hashes containing $ signs work literally
    docker exec -i "$container" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C -x -d "$db" -Q "$sql"
}

# ---- 1. Seed users ----
echo "[seed] Seeding ${N_USERS} users into ${USER_DB_NAME}@${USER_DB_CONTAINER}..."
USER_SQL="
SET NOCOUNT ON;
DECLARE @PwHash NVARCHAR(256) = '${HASH}';

;WITH Numbers AS (
    SELECT TOP ${N_USERS} ROW_NUMBER() OVER (ORDER BY a.object_id) AS N
    FROM sys.all_columns a CROSS JOIN sys.all_columns b
)
INSERT INTO Users (UserId, FirstName, LastName, Email, PasswordHash, Role, Phone, CreatedAt)
SELECT
    NEWID(),
    'SeedFirst' + CAST(N AS NVARCHAR(10)),
    'SeedLast'  + CAST(N AS NVARCHAR(10)),
    'seed_user_' + CAST(N AS NVARCHAR(10)) + '@seed.local',
    @PwHash,
    'Client',
    NULL,
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 730, GETUTCDATE())
FROM Numbers;

SELECT COUNT(*) AS TotalUsers FROM Users;
"
sqlcmd "$USER_DB_CONTAINER" "$USER_DB_NAME" "$USER_SQL"

# ---- 2. Seed bookings ----
# For monolith/separated: all 3 tables in one DB, so JOIN to Rooms + Users works directly
# For microservices: Bookings DB doesn't have Rooms/Users tables, so we need to
#   (a) read room IDs from RoomService DB
#   (b) read user IDs from UserService DB
#   (c) insert bookings with those IDs as "foreign keys without constraints"
echo "[seed] Seeding ${N_BOOKINGS} bookings..."

if [[ "$ARCH" == "microservices" ]]; then
    # Get room IDs + prices from room-db
    echo "[seed] Fetching room IDs from ${ROOM_DB_CONTAINER}..."
    ROOMS_CSV=$(docker exec -i "$ROOM_DB_CONTAINER" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C -x -d "$ROOM_DB_NAME" \
        -Q "SET NOCOUNT ON; SELECT CONVERT(NVARCHAR(36), RoomId) + '|' + CONVERT(NVARCHAR(20), PricePerHour) FROM Rooms" \
        -h -1 -W | grep -v '^$' | head -50)
    ROOM_COUNT=$(echo "$ROOMS_CSV" | wc -l | tr -d ' ')
    echo "[seed] Found ${ROOM_COUNT} rooms"

    # Get user IDs from user-db
    echo "[seed] Fetching user IDs from ${USER_DB_CONTAINER}..."
    USERS_CSV=$(docker exec -i "$USER_DB_CONTAINER" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C -x -d "$USER_DB_NAME" \
        -Q "SET NOCOUNT ON; SELECT CONVERT(NVARCHAR(36), UserId) FROM Users WHERE Email LIKE 'seed_user_%@seed.local'" \
        -h -1 -W | grep -v '^$')
    USER_COUNT=$(echo "$USERS_CSV" | wc -l | tr -d ' ')
    echo "[seed] Found ${USER_COUNT} seed users"

    # Build staging table in booking DB with room/user IDs, then insert bookings
    # Use a temp table approach
    STAGING_SQL="
SET NOCOUNT ON;
IF OBJECT_ID('tempdb..#Rooms') IS NOT NULL DROP TABLE #Rooms;
IF OBJECT_ID('tempdb..#Users') IS NOT NULL DROP TABLE #Users;
CREATE TABLE #Rooms (idx INT IDENTITY(1,1), RoomId UNIQUEIDENTIFIER, PricePerHour DECIMAL(10,2));
CREATE TABLE #Users (idx INT IDENTITY(1,1), UserId UNIQUEIDENTIFIER);
"
    # Append INSERT statements for each room
    while IFS='|' read -r rid price; do
        rid=$(echo "$rid" | tr -d ' ')
        price=$(echo "$price" | tr -d ' ')
        [[ -z "$rid" ]] && continue
        STAGING_SQL+="INSERT INTO #Rooms (RoomId, PricePerHour) VALUES ('$rid', $price);"
    done <<< "$ROOMS_CSV"

    # Users in chunks to avoid massive single SQL statement
    USERS_VALUES=""
    COUNT=0
    while IFS= read -r uid; do
        uid=$(echo "$uid" | tr -d ' ')
        [[ -z "$uid" ]] && continue
        if [[ -n "$USERS_VALUES" ]]; then USERS_VALUES+=","; fi
        USERS_VALUES+="('$uid')"
        COUNT=$((COUNT + 1))
        # Flush every 1000 rows
        if (( COUNT % 1000 == 0 )); then
            STAGING_SQL+="INSERT INTO #Users (UserId) VALUES $USERS_VALUES;"
            USERS_VALUES=""
        fi
    done <<< "$USERS_CSV"
    if [[ -n "$USERS_VALUES" ]]; then
        STAGING_SQL+="INSERT INTO #Users (UserId) VALUES $USERS_VALUES;"
    fi

    STAGING_SQL+="
DECLARE @RoomCount INT = (SELECT COUNT(*) FROM #Rooms);
DECLARE @UserCount INT = (SELECT COUNT(*) FROM #Users);

;WITH Numbers AS (
    SELECT TOP ${N_BOOKINGS} ROW_NUMBER() OVER (ORDER BY a.object_id) AS N
    FROM sys.all_columns a CROSS JOIN sys.all_columns b
),
Generated AS (
    SELECT
        N,
        DATEADD(HOUR, -(ABS(CHECKSUM(NEWID())) % 17520) - 1, GETUTCDATE()) AS StartTime,
        (ABS(CHECKSUM(NEWID())) % @RoomCount) + 1 AS RoomIdx,
        (ABS(CHECKSUM(NEWID())) % @UserCount) + 1 AS UserIdx
    FROM Numbers
)
INSERT INTO Bookings (BookingId, StartTime, EndTime, TotalPrice, Status, CreatedAt, RoomId, UserId)
SELECT
    NEWID(),
    g.StartTime,
    DATEADD(HOUR, 1, g.StartTime),
    r.PricePerHour,
    'Completed',
    DATEADD(HOUR, -1, g.StartTime),
    r.RoomId,
    u.UserId
FROM Generated g
JOIN #Rooms r ON r.idx = g.RoomIdx
JOIN #Users u ON u.idx = g.UserIdx;

SELECT COUNT(*) AS TotalBookings FROM Bookings;
"
    sqlcmd "$BOOKING_DB_CONTAINER" "$BOOKING_DB_NAME" "$STAGING_SQL"
else
    # Monolith/Separated — Rooms and Users in same DB, direct JOIN works
    BOOKING_SQL="
SET NOCOUNT ON;

DECLARE @RoomCount INT = (SELECT COUNT(*) FROM Rooms);
DECLARE @UserCount INT = (SELECT COUNT(*) FROM Users WHERE Email LIKE 'seed_user_%@seed.local');

;WITH RoomsIdx AS (
    SELECT RoomId, PricePerHour, ROW_NUMBER() OVER (ORDER BY RoomId) AS idx
    FROM Rooms
),
UsersIdx AS (
    SELECT UserId, ROW_NUMBER() OVER (ORDER BY UserId) AS idx
    FROM Users WHERE Email LIKE 'seed_user_%@seed.local'
),
Numbers AS (
    SELECT TOP ${N_BOOKINGS} ROW_NUMBER() OVER (ORDER BY a.object_id) AS N
    FROM sys.all_columns a CROSS JOIN sys.all_columns b
),
Generated AS (
    SELECT
        N,
        DATEADD(HOUR, -(ABS(CHECKSUM(NEWID())) % 17520) - 1, GETUTCDATE()) AS StartTime,
        (ABS(CHECKSUM(NEWID())) % @RoomCount) + 1 AS RoomIdx,
        (ABS(CHECKSUM(NEWID())) % @UserCount) + 1 AS UserIdx
    FROM Numbers
)
INSERT INTO Bookings (BookingId, StartTime, EndTime, TotalPrice, Status, CreatedAt, RoomId, UserId)
SELECT
    NEWID(),
    g.StartTime,
    DATEADD(HOUR, 1, g.StartTime),
    r.PricePerHour,
    'Completed',
    DATEADD(HOUR, -1, g.StartTime),
    r.RoomId,
    u.UserId
FROM Generated g
JOIN RoomsIdx r ON r.idx = g.RoomIdx
JOIN UsersIdx u ON u.idx = g.UserIdx;

SELECT COUNT(*) AS TotalBookings FROM Bookings;
"
    sqlcmd "$BOOKING_DB_CONTAINER" "$BOOKING_DB_NAME" "$BOOKING_SQL"
fi

echo "[seed] ✔ Seeding complete for architecture: $ARCH"
