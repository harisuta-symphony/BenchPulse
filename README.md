# BenchPulse

Internal "Skill LinkedIn" + booking system for benched employees — post skills you're learning, find peers, and book 1-on-1 knowledge-sharing sessions.

---

## Tech Stack

| Layer | Technology |
|---|---|
| API | .NET 8, ASP.NET Core, Clean Architecture |
| Database / Auth | Supabase (PostgreSQL + JWT Auth + Google OAuth 2.0) |
| ORM | Entity Framework Core + Npgsql |
| Frontend | Angular (latest stable), Standalone Components, Angular Signals |
| Tests | Playwright (TypeScript), `@playwright/test` |
| Orchestration | .NET Aspire (AppHost + ServiceDefaults) |
| Containers | Docker + Docker Compose |
| CI/CD | GitHub Actions |

---

## Quick Start

### Option 1 — .NET Aspire (recommended for local dev)

Starts backend, frontend, and the Aspire dashboard in one command.

```bash
dotnet run --project backend/BenchPulse/BenchPulse.AppHost
```

Dashboard: `https://localhost:15888`

### Option 2 — Docker Compose (full stack)

```bash
docker-compose up --build
docker-compose down -v   # teardown + remove volumes
```

### Option 3 — Individual services

**Backend**
```bash
cd backend/BenchPulse
dotnet build BenchPulse.sln
dotnet run --project BenchPulse.Api
# Swagger UI: https://localhost:5001/swagger
```

**Frontend**
```bash
cd frontend
ng serve
# App: http://localhost:4200
```

**Playwright tests**
```bash
cd e2e-tests
npx playwright test                  # full suite
npx playwright test --grep @smoke    # smoke tests only (CI fast gate)
npx playwright show-report
```

**EF Core migrations**
```bash
cd backend/BenchPulse
dotnet ef migrations add <Name> --project BenchPulse.Infrastructure --startup-project BenchPulse.Api
dotnet ef database update           --project BenchPulse.Infrastructure --startup-project BenchPulse.Api
```

---

## Clean Architecture Layers

```
BenchPulse/
├── BenchPulse.Domain          ← Entities + Enums (zero external dependencies)
├── BenchPulse.Application     ← DTOs, interfaces, service logic, AutoMapper profiles
├── BenchPulse.Infrastructure  ← AppDbContext, EF Core repositories, migrations
└── BenchPulse.Api             ← Controllers, Program.cs, JWT middleware, Swagger
```

Dependency rule: each layer only depends on the layer to its left.

| Layer | Responsibility |
|---|---|
| **Domain** | `UserEntity`, `SkillEntity`, `UserSkillEntity`, `BookingEntity`; `SkillStatus` and `BookingStatus` enums |
| **Application** | `IUserService` / `ISkillService` / `IUserSkillService` / `IBookingService` interfaces; repository interfaces; DTOs; AutoMapper; FluentValidation |
| **Infrastructure** | `AppDbContext`, repository implementations (`UserRepository`, etc.), EF Core migrations |
| **API** | `UsersController`, `SkillsController`, `UserSkillsController`, `BookingsController`; Supabase JWT validation; CORS; global exception handling |

---

## Frontend Structure

```
frontend/src/app/
├── core/
│   ├── services/    ← HTTP wrappers (UserService, SkillService, BookingService) + AuthService
│   └── models/      ← TypeScript interfaces mirroring backend DTOs
├── shared/          ← Navbar, LoadingSpinner, ErrorAlert (standalone components)
└── features/
    ├── skills/      ← Skill search (home) + skill detail with teachable peers
    ├── profile/     ← Profile view/edit + skill management
    └── bookings/    ← Booking form + list (requester & provider views)
```

---

## Implementation Phases

| Phase | Description |
|---|---|
| 0 | Monorepo & environment setup |
| 1 | Domain layer (entities + enums) |
| 2 | Application layer (DTOs, interfaces, services) |
| 3 | Infrastructure layer (EF Core, repositories, migrations) |
| 4 | API layer (controllers, auth, Swagger) |
| 5 | Angular scaffolding (structure, services, routing) |
| 6 | Angular features (components, auth, styling) |
| 7 | Playwright setup & Page Object Models |
| 8 | API & integration tests |
| 9 | E2E journey tests |
| 10 | CI/CD (GitHub Actions) |
| 11 | Dockerize the full stack |
| 12 | .NET Aspire local orchestration |

See [`BENCHPULSE_MASTER_PLAN.md`](./BENCHPULSE_MASTER_PLAN.md) for detailed task-by-task instructions.
