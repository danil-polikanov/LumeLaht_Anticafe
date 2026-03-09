# Claude Instructions — LumeLaht Anticafe

## Project Overview

LumeLaht is an anticafe booking system built as a bachelor's thesis comparing three web architectures:
- **Monolithic** — single .NET 10 project serving API + static frontend
- **Separated** — React SPA (CRA, FSD architecture) + standalone .NET 10 API
- **Microservices** — React SPA + multiple .NET 10 services + API Gateway + RabbitMQ

Stack: React 19 / TypeScript / Redux Toolkit / Bootstrap (frontend), .NET 10 / Clean Architecture / EF Core / SQL Server (backend), Docker Compose, k6, Prometheus, Grafana.

## Repository Layout

```
LumeLaht_Anticafe/          ← Monolith project (planned)
LumeLaht_RoomApi/           ← API layer (Separated architecture)
LumeLaht_RoomApi.Application/  ← Services, DTOs, Interfaces
LumeLaht_RoomApi.Core/      ← Entities, Domain interfaces
LumeLaht_RoomApi.Infrastructure/  ← Repositories, EF DbContext
LumeLaht_RoomApi.Tests/     ← Unit tests
frontend/                   ← React SPA (FSD architecture)
Scripts/                    ← k6 load test scripts, DB scripts
```

## Git Flow

- `main` — stable, protected
- `development` — active development branch
- `feature/<issue-number>-<short-description>` → merge into `development`
- `fix/<issue-number>-<short-description>` → merge into `development`
- `architecture/<issue-number>-<short-description>` → merge into `development`
- `development` → `main` at sprint end

## Commit Format

```
<type>(<scope>): <Subject> #<issue-number>

<body — 1-2 sentences MAX, describe WHAT changed>

Closes #<issue-number>
```

**Types:** `feat` | `fix` | `refactor` | `test` | `chore` | `docs` | `perf` | `ci`

**Scopes:** `api` | `rooms` | `booking` | `auth` | `frontend` | `docker` | `testing` | `infra`

**Examples:**
```
feat(api): add room filtering by activity type #14

Adds POST /api/room/filters endpoint with pagination and sort support.

Closes #14
```
```
fix(api): correct Guid constraint on Delete route #N

Changes route constraint from {id:int} to {id:guid}.
```

## My Rules (Claude)

### Never do without explicit request
- Never commit (`git commit`)
- Never push (`git push`)
- Never create or close GitHub issues/PRs
- Never modify `docker-compose.yml` or migration files without discussion
- Never break Clean Architecture layer boundaries (e.g., Infrastructure → Core is OK; Core → Infrastructure is NOT)

### Always do
- Read a file before editing it
- Follow the commit format above when asked to commit
- Write code in English (variable names, comments, method names)
- Communicate in Russian in chat
- Check for unused `using` statements when editing C# files
- Use `async/await` for all I/O operations in C#
- Use TypeScript interfaces (not `any`) in frontend code

### Architecture rules
- Controllers call services only — no business logic in controllers
- Services call repositories only — no direct DbContext in services
- Entities have no dependencies on Application or Infrastructure layers
- Do not add new NuGet packages without discussing the reason

## Code Standards Summary

See `CONTRIBUTING.md` for full details.

**C# file size limits:**
- Controller: 100 lines max
- Service: 150 lines max
- Repository: 120 lines max
- Any method: 30 lines max

**Frontend file size limits:**
- Component: 300 lines max
- Page component: 150 lines max
- Custom hook: 100 lines max
- Redux slice: 200 lines max
