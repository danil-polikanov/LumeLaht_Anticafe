#!/bin/bash
# Seed database with 10k users + 50k historical bookings for one architecture.
# Usage: ./seed-db.sh <monolith|separated|microservices>

set -euo pipefail

# Git Bash on Windows rewrites unix-style absolute paths in command args into
# Windows paths (e.g. `/opt/x` -> `C:/Program Files/Git/opt/x`). That breaks
# `docker exec ... /opt/mssql-tools18/bin/sqlcmd` because the path is meant to
# resolve inside the container. Disable the conversion for this script.
export MSYS_NO_PATHCONV=1
export MSYS2_ARG_CONV_EXCL='*'

ARCH="${1:-}"
if [[ -z "$ARCH" ]]; then
    echo "Usage: $0 <monolith|separated|microservices>"
    exit 1
fi

SA_PASSWORD='LumeLaht_Pass123!'
SEED_PASSWORD='SeedPass123!'
N_USERS=10000
N_BOOKINGS=50000

# Generate bcrypt hash for seed password (one hash, all users share it — perf-neutral).
# Use `python` not `python3` for cross-platform: on Windows `python3` is a Microsoft
# Store launcher stub that does not see user-installed packages.
echo "[seed] Generating bcrypt hash for seed password..."
PY=$(command -v python || command -v python3)
HASH=$("$PY" -c "import bcrypt; print(bcrypt.hashpw(b'${SEED_PASSWORD}', bcrypt.gensalt(10)).decode())")
if [[ -z "$HASH" ]]; then
    echo "[seed] ERROR: Python bcrypt not installed. Run: pip install bcrypt"
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

;WITH
L0 AS (SELECT 1 AS c UNION ALL SELECT 1),
L1 AS (SELECT 1 AS c FROM L0 a, L0 b),
L2 AS (SELECT 1 AS c FROM L1 a, L1 b),
L3 AS (SELECT 1 AS c FROM L2 a, L2 b),
L4 AS (SELECT 1 AS c FROM L3 a, L3 b),
Numbers AS (SELECT TOP ${N_USERS} ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS N FROM L4)
INSERT INTO Users (UserId, FirstName, LastName, Email, PasswordHash, Role, Phone, CreatedAt)
SELECT
    NEWID(),
    'SeedFirst' + CAST(N AS NVARCHAR(10)),
    'SeedLast'  + CAST(N AS NVARCHAR(10)),
    'seed_user_' + CAST(N AS NVARCHAR(10)) + '@seed.local',
    @PwHash,
    'Client',
    NULL,
    DATEADD(DAY, -(ABS(CAST(CHECKSUM(NEWID()) AS BIGINT)) % 730), GETUTCDATE())
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
    # Microservices: BookingDb has no FK to Users/Rooms — UserId can be any GUID.
    # Only RoomId needs to match room-db (small list, safe to pass as args).
    echo "[seed] Fetching room IDs from ${ROOM_DB_CONTAINER}..."
    ROOMS_CSV=$(docker exec -i "$ROOM_DB_CONTAINER" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C -x -d "$ROOM_DB_NAME" \
        -Q "SET NOCOUNT ON; SELECT CONVERT(NVARCHAR(36), RoomId) + '|' + CONVERT(NVARCHAR(20), PricePerHour) FROM Rooms" \
        -h -1 -W | grep -v '^$' | head -50)
    ROOM_COUNT=$(echo "$ROOMS_CSV" | wc -l | tr -d ' ')
    echo "[seed] Found ${ROOM_COUNT} rooms"

    # Build #Rooms temp table from small room list (15 rooms — safe as args)
    STAGING_SQL="
SET NOCOUNT ON;
IF OBJECT_ID('tempdb..#Rooms') IS NOT NULL DROP TABLE #Rooms;
IF OBJECT_ID('tempdb..#Nums')  IS NOT NULL DROP TABLE #Nums;
CREATE TABLE #Rooms (idx INT NOT NULL PRIMARY KEY, RoomId UNIQUEIDENTIFIER NOT NULL, PricePerHour DECIMAL(10,2) NOT NULL);
"
    while IFS='|' read -r rid price; do
        rid=$(echo "$rid" | tr -d ' ')
        price=$(echo "$price" | tr -d ' ')
        [[ -z "$rid" ]] && continue
        STAGING_SQL+="INSERT INTO #Rooms (idx, RoomId, PricePerHour) SELECT ISNULL(MAX(idx),0)+1, '$rid', $price FROM #Rooms;"
    done <<< "$ROOMS_CSV"

    STAGING_SQL+="
DECLARE @RoomCount INT = (SELECT COUNT(*) FROM #Rooms);

CREATE TABLE #Nums (N INT NOT NULL PRIMARY KEY, HrsAgo INT NOT NULL, RoomIdx INT NOT NULL);
;WITH L0 AS(SELECT 1 c UNION ALL SELECT 1),L1 AS(SELECT 1 c FROM L0 a,L0 b),L2 AS(SELECT 1 c FROM L1 a,L1 b),L3 AS(SELECT 1 c FROM L2 a,L2 b),L4 AS(SELECT 1 c FROM L3 a,L3 b)
INSERT INTO #Nums SELECT TOP ${N_BOOKINGS} ROW_NUMBER() OVER(ORDER BY(SELECT NULL)),
    (ABS(CAST(CHECKSUM(NEWID()) AS BIGINT))%17520)+1,
    (ABS(CAST(CHECKSUM(NEWID()) AS BIGINT))%@RoomCount)+1 FROM L4 OPTION(MAXDOP 1);

INSERT INTO Bookings (BookingId, StartTime, EndTime, TotalPrice, Status, CreatedAt, RoomId, UserId)
SELECT NEWID(), DATEADD(HOUR,-n.HrsAgo,GETUTCDATE()), DATEADD(HOUR,-n.HrsAgo+1,GETUTCDATE()),
    r.PricePerHour, 'Completed', DATEADD(HOUR,-n.HrsAgo-1,GETUTCDATE()), r.RoomId, NEWID()
FROM #Nums n JOIN #Rooms r ON r.idx=n.RoomIdx;

SELECT COUNT(*) AS TotalBookings FROM Bookings;
"
    sqlcmd "$BOOKING_DB_CONTAINER" "$BOOKING_DB_NAME" "$STAGING_SQL"
else
    # Monolith/Separated — Rooms and Users in same DB
    BOOKING_SQL="
SET NOCOUNT ON;
IF OBJECT_ID('tempdb..#RoomsIdx') IS NOT NULL DROP TABLE #RoomsIdx;
IF OBJECT_ID('tempdb..#UsersIdx') IS NOT NULL DROP TABLE #UsersIdx;
IF OBJECT_ID('tempdb..#Nums')     IS NOT NULL DROP TABLE #Nums;

CREATE TABLE #RoomsIdx (idx INT NOT NULL PRIMARY KEY, RoomId UNIQUEIDENTIFIER NOT NULL, PricePerHour DECIMAL(10,2) NOT NULL);
CREATE TABLE #UsersIdx (idx INT NOT NULL PRIMARY KEY, UserId UNIQUEIDENTIFIER NOT NULL);
INSERT INTO #RoomsIdx SELECT ROW_NUMBER() OVER (ORDER BY RoomId), RoomId, PricePerHour FROM Rooms;
INSERT INTO #UsersIdx SELECT ROW_NUMBER() OVER (ORDER BY UserId), UserId FROM Users WHERE Email LIKE 'seed_user_%@seed.local';

DECLARE @RoomCount INT = (SELECT COUNT(*) FROM #RoomsIdx);
DECLARE @UserCount INT = (SELECT COUNT(*) FROM #UsersIdx);

CREATE TABLE #Nums (N INT NOT NULL PRIMARY KEY, HrsAgo INT NOT NULL, RoomIdx INT NOT NULL, UserIdx INT NOT NULL);
;WITH L0 AS(SELECT 1 c UNION ALL SELECT 1),L1 AS(SELECT 1 c FROM L0 a,L0 b),L2 AS(SELECT 1 c FROM L1 a,L1 b),L3 AS(SELECT 1 c FROM L2 a,L2 b),L4 AS(SELECT 1 c FROM L3 a,L3 b)
INSERT INTO #Nums SELECT TOP ${N_BOOKINGS} ROW_NUMBER() OVER(ORDER BY(SELECT NULL)),
    (ABS(CAST(CHECKSUM(NEWID()) AS BIGINT))%17520)+1,
    (ABS(CAST(CHECKSUM(NEWID()) AS BIGINT))%@RoomCount)+1,
    (ABS(CAST(CHECKSUM(NEWID()) AS BIGINT))%@UserCount)+1 FROM L4 OPTION(MAXDOP 1);

INSERT INTO Bookings (BookingId, StartTime, EndTime, TotalPrice, Status, CreatedAt, RoomId, UserId)
SELECT NEWID(), DATEADD(HOUR,-n.HrsAgo,GETUTCDATE()), DATEADD(HOUR,-n.HrsAgo+1,GETUTCDATE()),
    r.PricePerHour, 'Completed', DATEADD(HOUR,-n.HrsAgo-1,GETUTCDATE()), r.RoomId, u.UserId
FROM #Nums n JOIN #RoomsIdx r ON r.idx=n.RoomIdx JOIN #UsersIdx u ON u.idx=n.UserIdx;

SELECT COUNT(*) AS TotalBookings FROM Bookings;
"
    sqlcmd "$BOOKING_DB_CONTAINER" "$BOOKING_DB_NAME" "$BOOKING_SQL"
fi

echo "[seed] ✔ Seeding complete for architecture: $ARCH"
