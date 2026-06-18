# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BenchPulse is an internal "Skill LinkedIn" + booking system for benched employees. Users post skills they're learning or can teach, discover peers, and book 1-on-1 knowledge-sharing sessions.

**Current state:** The implementation plan (`BENCHPULSE_MASTER_PLAN.md`) exists but no code has been written yet. Use the plan as the authoritative reference for architecture decisions.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| API | .NET 8, ASP.NET Core, Clean Architecture |
| Database / Auth | Supabase (PostgreSQL + Auth via JWT) |
| ORM | Entity Framework Core + Npgsql |
| Frontend | Angular (latest stable), Standalone Components, Angular Signals |
| Tests | Playwright (TypeScript), `@playwright/test` |
| Orchestration | .NET Aspire (AppHost + ServiceDefaults) |
| Containers | Docker + Docker Compose |

## Commands

### Backend
```bash
cd backend/BenchPulse
dotnet build BenchPulse.sln
dotnet run --project BenchPulse.Api

# EF migrations (run from solution root)
dotnet ef migrations add <Name> --project BenchPulse.Infrastructure --startup-project BenchPulse.Api
dotnet ef database update --project BenchPulse.Infrastructure --startup-project BenchPulse.Api
```

### Frontend
```bash
cd frontend
ng serve            # dev server at localhost:4200
ng build --configuration production
```

### Tests (Playwright)
```bash
cd e2e-tests
npx playwright test                        # full suite
npx playwright test --grep @smoke          # smoke tests only
npx playwright show-report

# Generate Page Object Models from live app
npx playwright-cli snapshot http://localhost:4200
```

### Full stack (one command)
```bash
# .NET Aspire — starts backend + frontend + Aspire dashboard (https://localhost:15888)
dotnet run --project backend/BenchPulse/BenchPulse.AppHost

# OR Docker Compose
docker-compose up --build
docker-compose down -v
```

## Architecture

### Backend — Clean Architecture layers

```
Domain (no external deps) → Application (DTOs, interfaces) → Infrastructure (EF Core) → API (controllers)
```

- **Domain** — `UserEntity`, `SkillEntity`, `UserSkillEntity`, `BookingEntity`; enums `SkillStatus` (Learning/Proficient/Expert) and `BookingStatus` (Pending/Confirmed/Cancelled/Completed). Zero NuGet dependencies.
- **Application** — Service interfaces (`IUserService`, etc.), repository interfaces, DTOs, AutoMapper profiles, FluentValidation validators.
- **Infrastructure** — `AppDbContext`, EF Core repository implementations, migrations.
- **API** — Four controllers: `UsersController`, `SkillsController`, `UserSkillsController`, `BookingsController`. JWT middleware validates Supabase tokens. Swagger at `/swagger`.

### Key domain relationships
- `UserSkillEntity` is the join between a user and a skill — it carries `SkillStatus` and a `CanTeach` boolean.
- `BookingEntity` links a requester user → provider user around a specific skill, with a scheduled date and `BookingStatus`.

### Frontend — Angular feature structure

```
src/app/
├── core/services/    ← HTTP wrappers (one per domain) + Supabase auth service
├── core/models/      ← TypeScript interfaces mirroring backend DTOs
├── shared/           ← Navbar, spinner, error alert (standalone components)
└── features/         ← Lazy-loaded routes
    ├── skills/        ← Search (home page) + detail page showing teachable peers
    ├── profile/       ← View/edit profile + manage skill list
    └── bookings/      ← Book a session form + list (requester & provider views)
```

Angular Signals are used for reactive state. An HTTP Interceptor attaches the Supabase JWT to every API request.

### Testing strategy

- **`tests/api/`** — REST endpoint CRUD + error scenarios; uses fixtures for setup/teardown.
- **`tests/integration/`** — Multi-step API flows (e.g., user skill relationship, booking lifecycle).
- **`tests/e2e/`** — Browser journeys: skill search → peer discovery → book → confirm.
- **`tests/page-objects/`** — Page Object Model classes; generate initial selectors with `playwright-cli snapshot`.
- `@smoke` tag gates critical-path tests for feature branch CI (~30 sec). Full suite runs on PRs and main.

### CI/CD (GitHub Actions — `.github/workflows/ci.yml`)

- **All branches:** backend build, frontend build, smoke tests.
- **PRs / main:** full Playwright suite + Docker build verification.
