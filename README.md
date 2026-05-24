# LumeLaht Anticafe

Anticafe booking system built as a bachelor's thesis at the University of Tartu, Narva College. The repository contains three implementations of the same product to compare web architectures under identical load: **monolith**, **separated frontend/backend**, and **microservices**.

## Architectures

- **Monolith** — single .NET 10 project that serves the API and the pre-built SPA from `LumeLaht_Anticafe/wwwroot`. Compose: `docker-compose.monolith.yml`.
- **Separated** — standalone .NET 10 API (`LumeLaht_RoomApi`) + React SPA (`frontend/`). Compose: `docker-compose.separated.yml`.
- **Microservices** — three .NET 10 services (Room, Booking, User), an API Gateway, and RabbitMQ, with the same React SPA (`frontend/`). Compose: `docker-compose.microservices.yml`.

All three share the same domain (rooms, bookings, users, auth) and the same React frontend, so benchmarks measure architectural overhead and not feature differences.

## Tech stack

- **Backend:** .NET 10, Clean Architecture (Core / Application / Infrastructure / API), EF Core, SQL Server, JWT auth.
- **Frontend:** React 19, TypeScript, Redux Toolkit, Vite, Feature-Sliced Design.
- **Infra:** Docker Compose, Nginx, RabbitMQ (microservices only), Prometheus + Grafana.
- **Load testing:** k6 with constant / ramp-up / spike profiles.

## Repository layout

```text
LumeLaht_Anticafe/                Monolith
LumeLaht_RoomApi/                 Separated API (entry)
LumeLaht_RoomApi.Application/     Services, DTOs, interfaces
LumeLaht_RoomApi.Core/            Entities, domain interfaces
LumeLaht_RoomApi.Infrastructure/  EF DbContext, repositories
LumeLaht_RoomApi.Tests/           Unit tests
Microservices/
  ApiGateway/                     YARP-based gateway
  RoomService/                    Room catalog
  BookingService/                 Bookings + RabbitMQ consumers
  UserService/                    Auth + user profile
frontend/                         React SPA (used by Separated and Microservices)
Scripts/
  k6/                             Load test scenarios
  experiments/                    Benchmark pipeline + chart scripts
results/                          Benchmark output (committed for reproducibility)
```

## Running locally

Requires Docker Desktop. Pick one architecture and start it:

```bash
docker compose -f docker-compose.monolith.yml       up --build
docker compose -f docker-compose.separated.yml      up --build
docker compose -f docker-compose.microservices.yml  up --build
```

### Ports

All three compose files expose the same host ports, so the same k6 scripts, Grafana dashboards, and frontend URL work against any architecture without changes.

- **3000** — app entry point (SPA + API). Open `http://localhost:3000`.
  - Monolith: served directly by the .NET app.
  - Separated: Nginx serves the SPA and proxies `/api` to the backend container.
  - Microservices: Nginx serves the SPA and proxies `/api` to the API Gateway.
- **9090** — Prometheus (`http://localhost:9090`).
- **3001** — Grafana (`http://localhost:3001`, anonymous viewer enabled).

Backend services and SQL Server instances are reachable only on the internal Docker network — they are not published to the host. In the microservices stack, `api-gateway`, `room-service`, `user-service`, `booking-service`, and `room-db` / `user-db` / `booking-db` all listen on container port `8080` (services) or `1433` (databases) and are addressed by service name within `microservices-net`.

Frontend (when working on the SPA directly):

```bash
cd frontend
npm install
npm run dev
```

Backend tests:

```bash
dotnet test LumeLaht_RoomApi.Tests
dotnet test Microservices/BookingService/BookingService.Tests
dotnet test Microservices/UserService/UserService.Tests
```

## Benchmarks

The benchmark pipeline runs each architecture under three k6 load profiles (constant, ramp-up, spike), 5 repetitions each, and writes JSON summaries + compressed raw samples into `results/`. Charts for the thesis are regenerated from `results/analysis.json`.

```bash
python Scripts/experiments/run_all.py
```

Grafana dashboards are available at `http://localhost:3001` while the stack is running.

## Documentation

- [CLAUDE.md](CLAUDE.md) — repo conventions and AI assistant rules
- [CONTRIBUTING.md](CONTRIBUTING.md) — code style, file-size limits, commit format
- [USER_STORIES.md](USER_STORIES.md) — product scope as user stories
