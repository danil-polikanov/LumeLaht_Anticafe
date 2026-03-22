# Contributing Guidelines — LumeLaht Anticafe

## Git Flow

```
main
 └── development
      ├── feature/16-room-booking-api
      ├── fix/11-jwt-token-expiration
      └── architecture/19-room-microservice
```

- Work in `feature/*`, `fix/*`, or `architecture/*` branches off `development`
- Merge into `development` via PR
- `development` → `main` at end of each sprint

## Branch Naming

```
feature/<issue-number>-<short-description>
fix/<issue-number>-<short-description>
architecture/<issue-number>-<short-description>
chore/<issue-number>-<short-description>
```

Examples:
- `feature/16-room-booking-api`
- `fix/11-jwt-token-expiration`
- `architecture/19-room-microservice`
- `chore/7-update-dependencies`

## Commit Message Format

```
<type>(<scope>): <Subject> #<issue-number>

<body>

<footer>
```

### Types
| Type | When to use |
|------|-------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `refactor` | Code change without new feature or fix |
| `test` | Adding or updating tests |
| `chore` | Maintenance, dependencies, build |
| `docs` | Documentation only |
| `perf` | Performance improvement |
| `ci` | CI/CD changes |

### Scopes
`api` | `rooms` | `booking` | `auth` | `frontend` | `docker` | `testing` | `infra`

### Rules
- Subject: imperative mood, capitalize first letter, no period, max 72 chars
- Body: 1-2 sentences MAX — describe WHAT changed, not why
- Always include `#<issue-number>`
- Use `Closes #N` in footer when the issue is resolved

### Examples

```
feat(api): add room filtering by activity type #14

Adds POST /api/room/filters with pagination and sort options.

Closes #14
```

```
fix(api): correct Guid constraint on room delete route #N

Changes {id:int} to {id:guid} in HttpDelete attribute.
```

```
refactor(rooms): move exception classes to dedicated files #N

Separates NotFoundException, ConflictException, ValidationException
into individual files under Application/Exceptions/.
```

---

## PR Checklist (self-review before merge)

```
- [ ] Code compiles without errors
- [ ] All tests pass
- [ ] No console.log / Debug.WriteLine / TODO left in code
- [ ] No unused `using` statements (C#)
- [ ] Issue number referenced in commits and PR description
- [ ] Branch targets `development`, not `main`
- [ ] File size limits respected (see below)
- [ ] No hardcoded connection strings or secrets
```

PR description must include: `Closes #<issue-number>`

---

## C# Coding Standards

### File Size Limits
| File Type | Max Lines |
|-----------|-----------|
| Controller | 100 |
| Service | 150 |
| Repository | 120 |
| Any method | 30 |
| Entity / DTO | no limit |

If a file exceeds the limit — split it: extract methods, create separate services, split controller by resource.

### Layer Rules (Clean Architecture)
```
Core        ← no dependencies on other layers
Application ← depends on Core only
Infrastructure ← depends on Core and Application
API (Web)   ← depends on Application only, registers Infrastructure via DI
```

- Controllers call services only — no business logic, no direct repository calls
- Services contain all business logic — no DbContext, no HTTP context
- Repositories handle data access only — no business rules
- Entities have no framework dependencies

### Naming
- Classes, methods, properties: `PascalCase`
- Private fields: `_camelCase`
- Local variables, parameters: `camelCase`
- Interfaces: prefix with `I` → `IRoomService`
- Async methods: suffix with `Async` → `GetRoomByIdAsync`

### General Rules
- `async/await` for all I/O operations
- Pass `CancellationToken` through all async methods
- Use `enum` for fixed sets of values (not plain strings)
- No `#pragma warning disable` — fix the root cause
- No suppressed nullable warnings — use `?`, `!`, or null guards
- Return `IReadOnlyList<T>` or `IEnumerable<T>` from services when collection is not modified by caller
- Domain exceptions go in `Application/Exceptions/` — one class per file
- CORS origins and connection strings go in `appsettings.json` / environment variables, never hardcoded

---

## Frontend Coding Standards

### File Size Limits
| File Type | Max Lines |
|-----------|-----------|
| Component | 300 |
| Page component | 150 |
| Custom hook | 100 |
| Redux slice | 200 |
| Utility/helper | 150 |
| Type definition file | 400 |

### FSD Layer Rules
```
app → pages → widgets → features → entities → shared
```
- A layer can only import from layers **below** it
- Never import from a higher layer
- Shared code goes in `shared/` only when used in 2+ places

### Naming
- Components, pages, widgets: `PascalCase.tsx`
- Hooks: `useFeatureName.ts` (camelCase with `use` prefix)
- Utilities: `camelCase.ts`
- Types: `types.ts` or `featureTypes.ts`

### General Rules
- Functional components only (no class components)
- Always define prop types with TypeScript interfaces (no `any`)
- Explicit return types on all functions
- Event handlers: `handleSubmit`, `handleClick` (not inline `onClick={() => ...}` for complex logic)
- Return objects from hooks, not arrays
- No `console.log` in committed code
- Use `interface` for object shapes, `type` for unions/intersections

### Prettier Config (from `.prettierrc`)
```json
{
  "semi": true,
  "trailingComma": "all",
  "singleQuote": true,
  "jsxSingleQuote": false,
  "bracketSpacing": true,
  "bracketSameLine": false,
  "arrowParens": "always"
}
```

---

## Testing

- Unit test file naming: `ClassName.Tests.cs` (C#) / `featureName.test.ts` (frontend)
- Each test: Arrange → Act → Assert
- Test method naming: `MethodName_Scenario_ExpectedResult`
  - Example: `GetRoomById_RoomNotFound_ReturnsNull`
- Mock external dependencies (repositories, services) — no real DB in unit tests
