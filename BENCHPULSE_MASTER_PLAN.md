# BenchPulse Skill Exchange — Master Implementation Plan

> **Target:** 3 weeks | **Stack:** .NET 8 Clean Architecture · Angular (latest stable, Standalone) · Supabase (PostgreSQL) · Playwright (TypeScript) · Docker · .NET Aspire
> **Goal:** Internal "Skill LinkedIn" + booking system for benched employees — post skills you're learning, find peers, book 1-on-1 knowledge-sharing sessions.

---

## Quick Reference

| Layer | Tech | Key Libraries / Tools |
|---|---|---|
| API | .NET 8, ASP.NET Core, C# | Entity Framework Core, AutoMapper, FluentValidation, Swashbuckle |
| Database / Auth | Supabase (PostgreSQL) | EF Core Migrations, Npgsql, Supabase Auth, Google OAuth 2.0 |
| Frontend | Angular (latest stable), TypeScript | Standalone Components, Angular Signals, HttpClient, Angular Router |
| Tests | Playwright, TypeScript | @playwright/test, playwright-cli (snapshots → POMs), Playwright AI Agents |
| CI/CD | GitHub Actions | .yml workflows, dotnet CLI, npm CI, @smoke tag gating |
| Containers | Docker, Docker Compose | Multi-stage Dockerfiles, local full-stack compose |
| Orchestration | .NET Aspire | AppHost, ServiceDefaults, health dashboard |

---

## Directory Blueprint

```
benchpulse/                                       ← repo root
├── .gitignore
├── README.md
├── BENCHPULSE_MASTER_PLAN.md
├── docker-compose.yml
├── .github/
│   └── workflows/
│       └── ci.yml
├── backend/
│   └── BenchPulse/
│       ├── BenchPulse.sln
│       ├── BenchPulse.AppHost/                   ← .NET Aspire orchestrator
│       │   ├── BenchPulse.AppHost.csproj
│       │   └── Program.cs
│       ├── BenchPulse.ServiceDefaults/           ← .NET Aspire shared defaults
│       │   ├── BenchPulse.ServiceDefaults.csproj
│       │   └── Extensions.cs
│       ├── BenchPulse.Domain/                    ← No external dependencies
│       │   ├── BenchPulse.Domain.csproj
│       │   ├── Entities/
│       │   │   ├── UserEntity.cs
│       │   │   ├── SkillEntity.cs
│       │   │   ├── UserSkillEntity.cs            ← join: user ↔ skill + status
│       │   │   └── BookingEntity.cs
│       │   └── Enums/
│       │       ├── SkillStatus.cs                ← Learning / Proficient / Expert
│       │       └── BookingStatus.cs              ← Pending / Confirmed / Cancelled
│       ├── BenchPulse.Application/
│       │   ├── BenchPulse.Application.csproj
│       │   ├── dtos/
│       │   │   ├── UserDto.cs
│       │   │   ├── CreateUserDto.cs
│       │   │   ├── SkillDto.cs
│       │   │   ├── CreateSkillDto.cs
│       │   │   ├── UserSkillDto.cs
│       │   │   ├── CreateUserSkillDto.cs
│       │   │   ├── BookingDto.cs
│       │   │   └── CreateBookingDto.cs
│       │   ├── interfaces/
│       │   │   ├── IUserRepository.cs
│       │   │   ├── ISkillRepository.cs
│       │   │   ├── IUserSkillRepository.cs
│       │   │   ├── IBookingRepository.cs
│       │   │   ├── IUserService.cs
│       │   │   ├── ISkillService.cs
│       │   │   ├── IUserSkillService.cs
│       │   │   └── IBookingService.cs
│       │   └── services/
│       │       ├── UserService.cs
│       │       ├── SkillService.cs
│       │       ├── UserSkillService.cs
│       │       └── BookingService.cs
│       ├── BenchPulse.Infrastructure/
│       │   ├── BenchPulse.Infrastructure.csproj
│       │   ├── Data/
│       │   │   ├── AppDbContext.cs
│       │   │   └── Migrations/
│       │   └── Repositories/
│       │       ├── UserRepository.cs
│       │       ├── SkillRepository.cs
│       │       ├── UserSkillRepository.cs
│       │       └── BookingRepository.cs
│       └── BenchPulse.Api/
│           ├── BenchPulse.Api.csproj
│           ├── Program.cs
│           ├── appsettings.json
│           ├── appsettings.Development.json
│           └── Controllers/
│               ├── UsersController.cs
│               ├── SkillsController.cs
│               ├── UserSkillsController.cs
│               └── BookingsController.cs
├── frontend/
│   ├── angular.json
│   ├── package.json
│   ├── tsconfig.json
│   └── src/
│       ├── main.ts
│       └── app/
│           ├── app.config.ts
│           ├── app.routes.ts
│           ├── app.component.ts
│           ├── core/
│           │   ├── services/
│           │   │   ├── user.service.ts
│           │   │   ├── skill.service.ts
│           │   │   ├── user-skill.service.ts
│           │   │   └── booking.service.ts
│           │   └── models/
│           │       ├── user.model.ts
│           │       ├── skill.model.ts
│           │       ├── user-skill.model.ts
│           │       └── booking.model.ts
│           ├── shared/
│           │   └── components/
│           │       └── navbar/
│           │           ├── navbar.component.ts
│           │           └── navbar.component.html
│           └── features/
│               ├── profile/
│               │   ├── profile-view/
│               │   └── profile-edit/
│               ├── skills/
│               │   ├── skill-search/
│               │   └── skill-detail/
│               └── bookings/
│                   ├── booking-form/
│                   └── booking-list/
└── e2e-tests/
    ├── playwright.config.ts
    ├── package.json
    ├── tsconfig.json
    └── tests/
        ├── global.setup.ts
        ├── api/
        │   ├── users.api.spec.ts
        │   ├── skills.api.spec.ts
        │   └── bookings.api.spec.ts
        ├── integration/
        │   ├── user-skills.integration.spec.ts
        │   └── booking-flow.integration.spec.ts
        ├── e2e/
        │   ├── profile-management.journey.spec.ts
        │   ├── skill-search.journey.spec.ts
        │   └── booking-flow.journey.spec.ts
        ├── fixtures/
        │   └── test.fixtures.ts
        ├── utils/
        │   ├── apiHelpers.ts
        │   └── testData.ts
        └── page-objects/
            ├── pages/
            │   ├── ProfilePage.ts
            │   ├── SkillSearchPage.ts
            │   ├── SkillDetailPage.ts
            │   └── BookingPage.ts
            └── components/
                ├── Navbar.ts
                └── SkillCard.ts
```

---

## Progress Tracker

```
Phase 0  — Monorepo Setup                    [ 0 / 5  ]
Phase 1  — Domain Layer                      [ 0 / 7  ]
Phase 2  — Application Layer                 [ 0 / 10 ]
Phase 3  — Infrastructure Layer              [ 0 / 8  ]
Phase 4  — API Layer (Controllers)           [ 0 / 9  ]
Phase 5  — Angular Scaffolding               [ 0 / 10 ]
Phase 6  — Angular Features                  [ 0 / 12 ]
Phase 7  — Playwright Setup & POMs           [ 0 / 10 ]
Phase 8  — API & Integration Tests           [ 0 / 10 ]
Phase 9  — E2E Journey Tests                 [ 0 / 8  ]
Phase 10 — CI/CD (GitHub Actions)            [ 0 / 5  ]
Phase 11 — Dockerize the Full Stack          [ 0 / 9  ]
Phase 12 — .NET Aspire (Local Orchestration) [ 0 / 7  ]
```

---

## Phase 0 — Monorepo & Environment Setup
**Estimated time:** 1–2 hours | **Week 1, Day 1**

---

### Task 0.1 — Initialize Git repository

**Goal:** Create a versioned root for the entire monorepo.

**Steps:**
```bash
mkdir benchpulse && cd benchpulse
git init
git branch -M main
```

**Done when:** `git status` shows empty repo on branch `main`.

---

### Task 0.2 — Create monorepo directory skeleton

**Goal:** Establish top-level folders so all relative paths are consistent.

**Steps:**
```bash
mkdir -p backend frontend e2e-tests .github/workflows
```

**Done when:** All four directories exist at repo root.

---

### Task 0.3 — Create root `.gitignore`

**Goal:** Prevent secrets, build artifacts, and IDE files from being committed.

**File:** `.gitignore` (repo root)

```gitignore
# .NET
backend/**/bin/
backend/**/obj/
backend/**/*.user
backend/**/appsettings.Development.json

# Node / Angular / Playwright
frontend/node_modules/
frontend/dist/
frontend/.angular/
e2e-tests/node_modules/
e2e-tests/playwright-report/
e2e-tests/test-results/
e2e-tests/.auth/
e2e-tests/.env

# Secrets
*.env
.env.local

# OS / IDE
.DS_Store
Thumbs.db
.idea/
.vscode/
```

**Done when:** File exists; `git status` shows it untracked (not ignored).

---

### Task 0.4 — Create root `README.md`

**Goal:** Single entry-point document for the project.

**Sections:**
- Project name + one-line description
- Tech stack table
- Quick-start commands (Docker, Aspire, individual services)
- Clean Architecture layer overview
- Links to phase docs

**Done when:** `README.md` exists and renders correctly on GitHub.

---

### Task 0.5 — Create environment config stubs

**Goal:** Provide local config values without committing secrets.

**File:** `backend/BenchPulse/BenchPulse.Api/appsettings.Development.json` *(gitignored)*
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR_SUPABASE_PASSWORD"
  },
  "Supabase": {
    "Url": "https://YOUR_PROJECT.supabase.co",
    "AnonKey": "YOUR_ANON_KEY"
  },
  "AllowedOrigins": ["http://localhost:4200"]
}
```

Also commit `appsettings.Development.json.example` with placeholder values.

**Done when:** Both files exist; real `.json` is gitignored; `.example` is tracked.

---

## Phase 1 — Domain Layer
**Estimated time:** 2–3 hours | **Week 1, Day 1**

> **Clean Architecture rule:** `BenchPulse.Domain` has **zero** external NuGet dependencies. Plain C# classes and enums only. No EF Core attributes here.

---

### Task 1.1 — Create .NET solution and Domain project

**Goal:** Scaffold the Clean Architecture solution skeleton.

**Steps:**
```bash
cd backend
mkdir BenchPulse && cd BenchPulse
dotnet new sln -n BenchPulse

dotnet new classlib -n BenchPulse.Domain       -o BenchPulse.Domain
dotnet new classlib -n BenchPulse.Application  -o BenchPulse.Application
dotnet new classlib -n BenchPulse.Infrastructure -o BenchPulse.Infrastructure
dotnet new webapi   -n BenchPulse.Api          -o BenchPulse.Api

dotnet sln add BenchPulse.Domain/BenchPulse.Domain.csproj
dotnet sln add BenchPulse.Application/BenchPulse.Application.csproj
dotnet sln add BenchPulse.Infrastructure/BenchPulse.Infrastructure.csproj
dotnet sln add BenchPulse.Api/BenchPulse.Api.csproj
```

Add project references (Clean Architecture dependency rule):
```bash
dotnet add BenchPulse.Application/BenchPulse.Application.csproj reference BenchPulse.Domain/BenchPulse.Domain.csproj
dotnet add BenchPulse.Infrastructure/BenchPulse.Infrastructure.csproj reference BenchPulse.Application/BenchPulse.Application.csproj
dotnet add BenchPulse.Api/BenchPulse.Api.csproj reference BenchPulse.Application/BenchPulse.Application.csproj
dotnet add BenchPulse.Api/BenchPulse.Api.csproj reference BenchPulse.Infrastructure/BenchPulse.Infrastructure.csproj
```

**Done when:** `dotnet build BenchPulse.sln` succeeds with 0 errors.

---

### Task 1.2 — Create Enums

**Goal:** Define shared status enumerations used across entities.

**File:** `BenchPulse.Domain/Enums/SkillStatus.cs`
```csharp
namespace BenchPulse.Domain.Enums;

public enum SkillStatus
{
    Learning = 0,
    Proficient = 1,
    Expert = 2
}
```

**File:** `BenchPulse.Domain/Enums/BookingStatus.cs`
```csharp
namespace BenchPulse.Domain.Enums;

public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Completed = 3
}
```

**Done when:** Both files compile with no warnings.

---

### Task 1.3 — Create `UserEntity`

**Goal:** Represent a benched employee with their profile.

**File:** `BenchPulse.Domain/Entities/UserEntity.cs`
```csharp
namespace BenchPulse.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<UserSkillEntity> UserSkills { get; set; } = new List<UserSkillEntity>();
    public ICollection<BookingEntity> BookingsAsRequester { get; set; } = new List<BookingEntity>();
    public ICollection<BookingEntity> BookingsAsProvider { get; set; } = new List<BookingEntity>();
}
```

**Done when:** File compiles; no EF Core or external references present.

---

### Task 1.4 — Create `SkillEntity`

**Goal:** Represent a skill/technology that users can learn or teach.

**File:** `BenchPulse.Domain/Entities/SkillEntity.cs`
```csharp
namespace BenchPulse.Domain.Entities;

public class SkillEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;       // e.g. "Angular", "Playwright"
    public string? Category { get; set; }                   // e.g. "Frontend", "Testing"
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<UserSkillEntity> UserSkills { get; set; } = new List<UserSkillEntity>();
}
```

**Done when:** File compiles; no external references.

---

### Task 1.5 — Create `UserSkillEntity` (join table)

**Goal:** Link users to skills with a learning status.

**File:** `BenchPulse.Domain/Entities/UserSkillEntity.cs`
```csharp
using BenchPulse.Domain.Enums;

namespace BenchPulse.Domain.Entities;

public class UserSkillEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SkillId { get; set; }
    public SkillStatus Status { get; set; }
    public bool IsTeachable { get; set; }   // true = willing to run a session
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public UserEntity User { get; set; } = null!;
    public SkillEntity Skill { get; set; } = null!;
}
```

**Done when:** File compiles; enum reference resolves from same project.

---

### Task 1.6 — Create `BookingEntity`

**Goal:** Represent a 1-on-1 session request between two users around a skill.

**File:** `BenchPulse.Domain/Entities/BookingEntity.cs`
```csharp
using BenchPulse.Domain.Enums;

namespace BenchPulse.Domain.Entities;

public class BookingEntity
{
    public Guid Id { get; set; }
    public Guid RequesterId { get; set; }   // who wants to learn
    public Guid ProviderId { get; set; }    // who will teach
    public Guid SkillId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public BookingStatus Status { get; set; }
    public string? Message { get; set; }
    public string? MeetingLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public UserEntity Requester { get; set; } = null!;
    public UserEntity Provider { get; set; } = null!;
    public SkillEntity Skill { get; set; } = null!;
}
```

**Done when:** File compiles; navigation properties reference correct entity types.

---

### Task 1.7 — Verify Domain layer integrity

**Goal:** Confirm the Domain project has no forbidden dependencies.

**Steps:**
```bash
cd backend/BenchPulse
dotnet build BenchPulse.Domain/BenchPulse.Domain.csproj
cat BenchPulse.Domain/BenchPulse.Domain.csproj
```

**Done when:** Build succeeds; `.csproj` contains no `<PackageReference>` entries.

---

## Phase 2 — Application Layer
**Estimated time:** 3–4 hours | **Week 1, Day 2**

> Application layer depends only on Domain. It defines the "what" (interfaces + DTOs) and "how" (service logic) without caring about infrastructure details.

---

### Task 2.1 — Add NuGet packages to Application

**Goal:** Add AutoMapper and FluentValidation for mapping and validation.

**Steps:**
```bash
cd backend/BenchPulse/BenchPulse.Application
dotnet add package AutoMapper
dotnet add package FluentValidation
```

**Done when:** `dotnet build` succeeds.

---

### Task 2.2 — Create User DTOs

**Goal:** Define data shapes for User API input/output.

**File:** `BenchPulse.Application/dtos/UserDto.cs`
```csharp
namespace BenchPulse.Application.dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Department { get; set; }
}

public class UpdateUserDto
{
    public string? FullName { get; set; }
    public string? Bio { get; set; }
    public string? Department { get; set; }
    public string? AvatarUrl { get; set; }
}
```

**Done when:** File compiles with no warnings.

---

### Task 2.3 — Create Skill DTOs

**File:** `BenchPulse.Application/dtos/SkillDto.cs`
```csharp
namespace BenchPulse.Application.dtos;

public class SkillDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }
    public int LearnerCount { get; set; }
}

public class CreateSkillDto
{
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Description { get; set; }
}
```

**Done when:** File compiles.

---

### Task 2.4 — Create UserSkill DTOs

**File:** `BenchPulse.Application/dtos/UserSkillDto.cs`
```csharp
using BenchPulse.Domain.Enums;

namespace BenchPulse.Application.dtos;

public class UserSkillDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public Guid SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public SkillStatus Status { get; set; }
    public bool IsTeachable { get; set; }
    public string? Notes { get; set; }
}

public class CreateUserSkillDto
{
    public Guid UserId { get; set; }
    public Guid SkillId { get; set; }
    public SkillStatus Status { get; set; }
    public bool IsTeachable { get; set; }
    public string? Notes { get; set; }
}
```

**Done when:** File compiles; enum import from Domain resolves.

---

### Task 2.5 — Create Booking DTOs

**File:** `BenchPulse.Application/dtos/BookingDto.cs`
```csharp
using BenchPulse.Domain.Enums;

namespace BenchPulse.Application.dtos;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid RequesterId { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public Guid ProviderId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public Guid SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public BookingStatus Status { get; set; }
    public string? Message { get; set; }
    public string? MeetingLink { get; set; }
}

public class CreateBookingDto
{
    public Guid RequesterId { get; set; }
    public Guid ProviderId { get; set; }
    public Guid SkillId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string? Message { get; set; }
}

public class UpdateBookingStatusDto
{
    public BookingStatus Status { get; set; }
    public string? MeetingLink { get; set; }
}
```

**Done when:** File compiles.

---

### Task 2.6 — Create Repository Interfaces

**Goal:** Define the data-access contract that Infrastructure must implement.

**File:** `BenchPulse.Application/interfaces/IUserRepository.cs`
```csharp
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task DeleteAsync(Guid id);
}
```

**File:** `BenchPulse.Application/interfaces/ISkillRepository.cs`
```csharp
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface ISkillRepository
{
    Task<IEnumerable<SkillEntity>> GetAllAsync();
    Task<IEnumerable<SkillEntity>> SearchAsync(string query);
    Task<SkillEntity?> GetByIdAsync(Guid id);
    Task<SkillEntity> CreateAsync(SkillEntity skill);
    Task DeleteAsync(Guid id);
}
```

**File:** `BenchPulse.Application/interfaces/IUserSkillRepository.cs`
```csharp
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface IUserSkillRepository
{
    Task<IEnumerable<UserSkillEntity>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserSkillEntity>> GetBySkillIdAsync(Guid skillId);
    Task<UserSkillEntity?> GetByIdAsync(Guid id);
    Task<UserSkillEntity> CreateAsync(UserSkillEntity userSkill);
    Task<UserSkillEntity> UpdateAsync(UserSkillEntity userSkill);
    Task DeleteAsync(Guid id);
}
```

**File:** `BenchPulse.Application/interfaces/IBookingRepository.cs`
```csharp
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface IBookingRepository
{
    Task<IEnumerable<BookingEntity>> GetByRequesterIdAsync(Guid requesterId);
    Task<IEnumerable<BookingEntity>> GetByProviderIdAsync(Guid providerId);
    Task<BookingEntity?> GetByIdAsync(Guid id);
    Task<BookingEntity> CreateAsync(BookingEntity booking);
    Task<BookingEntity> UpdateAsync(BookingEntity booking);
    Task DeleteAsync(Guid id);
}
```

**Done when:** All four interfaces compile with no errors.

---

### Task 2.7 — Create Service Interfaces

**File:** `BenchPulse.Application/interfaces/IUserService.cs`
```csharp
using BenchPulse.Application.dtos;

namespace BenchPulse.Application.interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto);
    Task DeleteAsync(Guid id);
}
```

Create similarly-structured `ISkillService.cs`, `IUserSkillService.cs`, and `IBookingService.cs` in the same folder, mirroring their repository counterparts but using DTOs as the contract.

**Done when:** All four service interfaces compile.

---

### Task 2.8 — Implement `UserService`

**File:** `BenchPulse.Application/services/UserService.cs`
```csharp
using AutoMapper;
using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var entity = _mapper.Map<UserEntity>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<UserDto>(created);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");
        _mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        var updated = await _repo.UpdateAsync(entity);
        return _mapper.Map<UserDto>(updated);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
}
```

Follow the same pattern to create `SkillService.cs`, `UserSkillService.cs`, and `BookingService.cs`.

**Done when:** All four service implementations compile.

---

### Task 2.9 — Create AutoMapper Profile

**File:** `BenchPulse.Application/MappingProfile.cs`
```csharp
using AutoMapper;
using BenchPulse.Application.dtos;
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<UserEntity, UserDto>();
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UpdateUserDto, UserEntity>()
            .ForAllMembers(opt => opt.Condition((_, _, src) => src != null));

        // Skill
        CreateMap<SkillEntity, SkillDto>()
            .ForMember(d => d.LearnerCount, opt => opt.MapFrom(s => s.UserSkills.Count));
        CreateMap<CreateSkillDto, SkillEntity>();

        // UserSkill
        CreateMap<UserSkillEntity, UserSkillDto>()
            .ForMember(d => d.UserFullName, opt => opt.MapFrom(s => s.User.FullName))
            .ForMember(d => d.SkillName, opt => opt.MapFrom(s => s.Skill.Name));
        CreateMap<CreateUserSkillDto, UserSkillEntity>();

        // Booking
        CreateMap<BookingEntity, BookingDto>()
            .ForMember(d => d.RequesterName, opt => opt.MapFrom(s => s.Requester.FullName))
            .ForMember(d => d.ProviderName, opt => opt.MapFrom(s => s.Provider.FullName))
            .ForMember(d => d.SkillName, opt => opt.MapFrom(s => s.Skill.Name));
        CreateMap<CreateBookingDto, BookingEntity>();
    }
}
```

**Done when:** File compiles; no "missing member" warnings in build output.

---

### Task 2.10 — Verify Application layer build

**Steps:**
```bash
cd backend/BenchPulse
dotnet build BenchPulse.Application/BenchPulse.Application.csproj
```

**Done when:** 0 errors, 0 warnings.

---

## Phase 3 — Infrastructure Layer
**Estimated time:** 3–4 hours | **Week 1, Day 3**

---

### Task 3.1 — Add NuGet packages to Infrastructure

**Goal:** Wire in EF Core + Npgsql for Supabase/PostgreSQL.

**Steps:**
```bash
cd backend/BenchPulse/BenchPulse.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
```

**Done when:** `dotnet build` succeeds.

---

### Task 3.2 — Create `AppDbContext`

**File:** `BenchPulse.Infrastructure/Data/AppDbContext.cs`
```csharp
using BenchPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BenchPulse.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<SkillEntity> Skills => Set<SkillEntity>();
    public DbSet<UserSkillEntity> UserSkills => Set<UserSkillEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(320);
        });

        modelBuilder.Entity<SkillEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Name).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<UserSkillEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.SkillId }).IsUnique();
            e.HasOne(x => x.User)
             .WithMany(u => u.UserSkills)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Skill)
             .WithMany(s => s.UserSkills)
             .HasForeignKey(x => x.SkillId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BookingEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Requester)
             .WithMany(u => u.BookingsAsRequester)
             .HasForeignKey(x => x.RequesterId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Provider)
             .WithMany(u => u.BookingsAsProvider)
             .HasForeignKey(x => x.ProviderId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
```

**Done when:** File compiles with no EF Core configuration errors.

---

### Task 3.3 — Implement `UserRepository`

**File:** `BenchPulse.Infrastructure/Repositories/UserRepository.cs`
```csharp
using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;
using BenchPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BenchPulse.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;
    public UserRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<UserEntity>> GetAllAsync() =>
        await _ctx.Users.AsNoTracking().ToListAsync();

    public async Task<UserEntity?> GetByIdAsync(Guid id) =>
        await _ctx.Users.Include(u => u.UserSkills)
                        .ThenInclude(us => us.Skill)
                        .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<UserEntity?> GetByEmailAsync(string email) =>
        await _ctx.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task<UserEntity> UpdateAsync(UserEntity user)
    {
        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _ctx.Users.FindAsync(id);
        if (user is not null) { _ctx.Users.Remove(user); await _ctx.SaveChangesAsync(); }
    }
}
```

Follow the same pattern to implement `SkillRepository.cs`, `UserSkillRepository.cs`, and `BookingRepository.cs`.

**Done when:** All four repository classes compile and implement their respective interfaces fully.

---

### Task 3.4 — Create Infrastructure DI Extension

**Goal:** Expose a single `AddInfrastructure()` call that Api layer can use.

**File:** `BenchPulse.Infrastructure/DependencyInjection.cs`
```csharp
using BenchPulse.Application.interfaces;
using BenchPulse.Infrastructure.Data;
using BenchPulse.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BenchPulse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IUserSkillRepository, UserSkillRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }
}
```

**Done when:** File compiles; method is callable from the Api project.

---

### Task 3.5 — Create Application DI Extension

**File:** `BenchPulse.Application/DependencyInjection.cs`
```csharp
using BenchPulse.Application.interfaces;
using BenchPulse.Application.services;
using Microsoft.Extensions.DependencyInjection;

namespace BenchPulse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IUserSkillService, UserSkillService>();
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }
}
```

**Done when:** File compiles.

---

### Task 3.6 — Add EF Core Design tools to API project

**Goal:** Enable `dotnet ef` migrations from the Api project.

**Steps:**
```bash
cd backend/BenchPulse/BenchPulse.Api
dotnet add package Microsoft.EntityFrameworkCore.Design
```

**Done when:** `dotnet ef --version` runs successfully from the solution root.

---

### Task 3.7 — Create Initial Migration

**Goal:** Generate the SQL schema for the Supabase database.

**Steps:**
```bash
cd backend/BenchPulse
dotnet ef migrations add InitialCreate \
  --project BenchPulse.Infrastructure \
  --startup-project BenchPulse.Api \
  --output-dir Data/Migrations
```

**Done when:** A `Data/Migrations/` folder with three generated files appears in the Infrastructure project.

---

### Task 3.8 — Apply Migration to Supabase

**Goal:** Push schema to the live Supabase PostgreSQL database.

**Steps:**
1. Set connection string in `appsettings.Development.json` to your Supabase DB URL.
2. Run:
```bash
dotnet ef database update \
  --project BenchPulse.Infrastructure \
  --startup-project BenchPulse.Api
```

**Done when:** Supabase Dashboard → Table Editor shows `users`, `skills`, `user_skills`, `bookings` tables.

---

## Phase 4 — API Layer (Controllers)
**Estimated time:** 3–4 hours | **Week 1, Day 4**

---

### Task 4.1 — Configure `Program.cs`

**File:** `BenchPulse.Api/Program.cs`
```csharp
using BenchPulse.Application;
using BenchPulse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BenchPulse API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");
app.UseAuthorization();
app.MapControllers();
app.Run();
```

**Done when:** `dotnet run` starts without exceptions; `https://localhost:5001/swagger` loads.

---

### Task 4.2 — Create `UsersController`

**File:** `BenchPulse.Api/Controllers/UsersController.cs`
```csharp
using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BenchPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto) =>
        Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
```

Create `SkillsController.cs`, `UserSkillsController.cs`, and `BookingsController.cs` using the same pattern, wiring to their respective services.

**Done when:** All controllers compile; no DI resolution errors at startup.

---

### Task 4.3 — Add `SkillsController` with search endpoint

**Goal:** Enable keyword search for skills — core feature of the app.

**Key extra endpoint** in `SkillsController.cs`:
```csharp
[HttpGet("search")]
public async Task<IActionResult> Search([FromQuery] string q) =>
    Ok(await _service.SearchAsync(q));
```

**Done when:** `GET /api/skills/search?q=angular` returns matching skills via Swagger.

---

### Task 4.4 — Add `BookingsController` with status update

**Key endpoint** in `BookingsController.cs`:
```csharp
[HttpPatch("{id:guid}/status")]
public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateBookingStatusDto dto) =>
    Ok(await _service.UpdateStatusAsync(id, dto));
```

**Done when:** `PATCH /api/bookings/{id}/status` returns 200 via Swagger.

---

### Task 4.5 — Add seed data endpoint (dev only)

**Goal:** Quickly populate the DB with test users and skills for frontend/test development.

Add a `SeedController.cs` gated behind `app.Environment.IsDevelopment()`. It should insert 5 sample users, 10 skills, and a few user-skill associations.

**Done when:** `POST /api/seed` returns 200; Supabase shows populated tables.

---

### Task 4.6 — Wire Supabase Auth (JWT validation)

**Goal:** Validate Supabase-issued JWTs (including Google OAuth tokens) on protected endpoints.

**Steps:**
```bash
cd backend/BenchPulse/BenchPulse.Api
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

In `Program.cs` add:
```csharp
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Supabase:Url"] + "/auth/v1";
        options.Audience = "authenticated";
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });
```

Decorate controllers with `[Authorize]` except the seed endpoint.

**Done when:** Unauthenticated requests to protected endpoints return `401 Unauthorized`.

---

### Task 4.6a — Enforce company email domain at API level

**Goal:** Reject tokens from users outside the allowed company domain (e.g. `@symphony.is`) even if they authenticated via Google.

Add a middleware or action filter that reads the `email` claim from the JWT and checks the domain:

```csharp
// BenchPulse.Api/Filters/CompanyDomainFilter.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BenchPulse.Api.Filters;

public class CompanyDomainFilter : IAuthorizationFilter
{
    private readonly string _allowedDomain;

    public CompanyDomainFilter(IConfiguration config)
    {
        _allowedDomain = config["Auth:AllowedEmailDomain"] ?? "@symphony.is";
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var email = context.HttpContext.User.FindFirstValue(ClaimTypes.Email)
                    ?? context.HttpContext.User.FindFirstValue("email");

        if (email == null || !email.EndsWith(_allowedDomain, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ForbidResult();
        }
    }
}
```

Register globally in `Program.cs`:
```csharp
builder.Services.AddControllers(options =>
    options.Filters.Add<CompanyDomainFilter>());
builder.Services.AddScoped<CompanyDomainFilter>();
```

Add to `appsettings.json`:
```json
"Auth": {
  "AllowedEmailDomain": "@symphony.is"
}
```

**Done when:** A valid Google-authenticated user with a non-company email receives `403 Forbidden`; a company email user proceeds normally.

---

### Task 4.7 — Validate all controllers via Swagger

**Steps:**
1. `dotnet run --project BenchPulse.Api`
2. Open `https://localhost:5001/swagger`
3. Test each endpoint: GET all, GET by ID, POST, PUT, DELETE for users, skills, user-skills, bookings.

**Done when:** All 16+ endpoints return expected status codes with correct JSON.

---

### Task 4.8 — Add global exception handler

**File:** `BenchPulse.Api/Middleware/GlobalExceptionMiddleware.cs`

Register a simple middleware that catches unhandled exceptions and returns a standard `ProblemDetails` response with `500 Internal Server Error`.

In `Program.cs`:
```csharp
app.UseExceptionHandler("/error");
```

**Done when:** Deliberately throwing an exception in a controller returns a clean JSON error (not a HTML stack trace).

---

### Task 4.9 — Verify full solution build

```bash
cd backend/BenchPulse
dotnet build BenchPulse.sln
```

**Done when:** 0 errors, 0 warnings across all four projects.

---

## Phase 5 — Angular Scaffolding
**Estimated time:** 2–3 hours | **Week 1, Day 5**

---

### Task 5.1 — Scaffold Angular app

**Goal:** Create the Angular application with standalone component configuration.

**Steps:**
```bash
cd frontend
npx @angular/cli@latest new benchpulse-ui \
  --standalone \
  --routing \
  --style css \
  --skip-git
mv benchpulse-ui/* . && mv benchpulse-ui/.* . 2>/dev/null; rmdir benchpulse-ui
```

**Done when:** `ng serve` starts and `http://localhost:4200` shows the default Angular page.

---

### Task 5.2 — Create feature directory structure

```bash
cd frontend/src/app
mkdir -p core/services core/models shared/components/navbar
mkdir -p features/profile/profile-view features/profile/profile-edit
mkdir -p features/skills/skill-search features/skills/skill-detail
mkdir -p features/bookings/booking-form features/bookings/booking-list
```

**Done when:** Directory tree matches the Directory Blueprint.

---

### Task 5.3 — Create TypeScript models

**File:** `src/app/core/models/user.model.ts`
```typescript
export interface User {
  id: string;
  fullName: string;
  email: string;
  avatarUrl?: string;
  bio?: string;
  department?: string;
  createdAt: string;
}

export interface CreateUser {
  fullName: string;
  email: string;
  bio?: string;
  department?: string;
}
```

Create `skill.model.ts`, `user-skill.model.ts`, and `booking.model.ts` matching the DTOs from Phase 2.

**Done when:** All model files compile with `ng build`.

---

### Task 5.4 — Configure `app.config.ts`

**File:** `src/app/app.config.ts`
```typescript
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
  ]
};
```

**Done when:** `ng serve` starts without DI errors.

---

### Task 5.5 — Configure `environment.ts`

**File:** `src/environments/environment.ts`
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5001/api',
  supabaseUrl: 'https://YOUR_PROJECT.supabase.co',
  supabaseAnonKey: 'YOUR_ANON_KEY'
};
```

**Done when:** Environment file is importable in services.

---

### Task 5.6 — Create `UserService`

**File:** `src/app/core/services/user.service.ts`
```typescript
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, CreateUser } from '../models/user.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/users`;

  getAll(): Observable<User[]> { return this.http.get<User[]>(this.base); }
  getById(id: string): Observable<User> { return this.http.get<User>(`${this.base}/${id}`); }
  create(dto: CreateUser): Observable<User> { return this.http.post<User>(this.base, dto); }
  update(id: string, dto: Partial<User>): Observable<User> { return this.http.put<User>(`${this.base}/${id}`, dto); }
  delete(id: string): Observable<void> { return this.http.delete<void>(`${this.base}/${id}`); }
}
```

Create `skill.service.ts`, `user-skill.service.ts`, and `booking.service.ts` following the same pattern.

**Done when:** All four services inject without errors; `ng build` passes.

---

### Task 5.7 — Create Navbar component

**File:** `src/app/shared/components/navbar/navbar.component.ts`
```typescript
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html'
})
export class NavbarComponent {}
```

**File:** `navbar.component.html`
```html
<nav>
  <a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{exact:true}">Home</a>
  <a routerLink="/skills" routerLinkActive="active">Skills</a>
  <a routerLink="/bookings" routerLinkActive="active">Bookings</a>
  <a routerLink="/profile" routerLinkActive="active">Profile</a>
</nav>
```

**Done when:** Navbar renders on `ng serve`.

---

### Task 5.8 — Configure `app.routes.ts`

**File:** `src/app/app.routes.ts`
```typescript
import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./features/skills/skill-search/skill-search.component').then(m => m.SkillSearchComponent) },
  { path: 'skills/:id', loadComponent: () => import('./features/skills/skill-detail/skill-detail.component').then(m => m.SkillDetailComponent) },
  { path: 'profile', loadComponent: () => import('./features/profile/profile-view/profile-view.component').then(m => m.ProfileViewComponent) },
  { path: 'profile/edit', loadComponent: () => import('./features/profile/profile-edit/profile-edit.component').then(m => m.ProfileEditComponent) },
  { path: 'bookings', loadComponent: () => import('./features/bookings/booking-list/booking-list.component').then(m => m.BookingListComponent) },
  { path: 'bookings/new', loadComponent: () => import('./features/bookings/booking-form/booking-form.component').then(m => m.BookingFormComponent) },
  { path: '**', redirectTo: '' }
];
```

**Done when:** Lazy-loaded routes render without console errors.

---

### Task 5.9 — Update `AppComponent`

**File:** `src/app/app.component.ts`
```typescript
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  template: `<app-navbar /><router-outlet />`
})
export class AppComponent {}
```

**Done when:** Navbar + router outlet visible on `http://localhost:4200`.

---

### Task 5.10 — Verify Angular build

```bash
cd frontend
ng build --configuration development
```

**Done when:** Build completes with 0 errors.

---

## Phase 6 — Angular Features
**Estimated time:** 4–6 hours | **Week 2, Days 1–2**

---

### Task 6.1 — Implement `SkillSearchComponent`

**Goal:** Main landing page — search bar + results grid of skills and learners.

**File:** `features/skills/skill-search/skill-search.component.ts`
```typescript
import { Component, OnInit, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SkillService } from '../../../core/services/skill.service';
import { Skill } from '../../../core/models/skill.model';

@Component({
  selector: 'app-skill-search',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './skill-search.component.html'
})
export class SkillSearchComponent implements OnInit {
  private skillService = inject(SkillService);

  query = signal('');
  skills = signal<Skill[]>([]);
  loading = signal(false);

  ngOnInit() { this.loadAll(); }

  loadAll() {
    this.loading.set(true);
    this.skillService.getAll().subscribe({
      next: s => { this.skills.set(s); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  search() {
    if (!this.query()) { this.loadAll(); return; }
    this.loading.set(true);
    this.skillService.search(this.query()).subscribe({
      next: s => { this.skills.set(s); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}
```

**Template** (`skill-search.component.html`):
```html
<div class="skill-search">
  <h1>Find a Skill Peer</h1>
  <input [(ngModel)]="query" placeholder="Search skills... e.g. Angular, Playwright"
         (keyup.enter)="search()" />
  <button (click)="search()">Search</button>

  @if (loading()) { <p>Loading...</p> }

  <div class="skills-grid">
    @for (skill of skills(); track skill.id) {
      <a [routerLink]="['/skills', skill.id]" class="skill-card">
        <h3>{{ skill.name }}</h3>
        <p>{{ skill.category }}</p>
        <span>{{ skill.learnerCount }} learners</span>
      </a>
    }
  </div>
</div>
```

**Done when:** Skills load on page open; search filters results in real time.

---

### Task 6.2 — Implement `SkillDetailComponent`

**Goal:** Show all users who have this skill and are willing to teach, with a "Book Session" CTA.

Key logic:
- Route param `id` → `skillService.getById(id)` + `userSkillService.getBySkillId(id)`
- Filter `UserSkill[]` where `isTeachable === true`
- Each result shows user name, status badge, and a "Book Session" button linking to `/bookings/new?providerId=X&skillId=Y`

**Done when:** Navigating to `/skills/:id` shows the skill and its teachable users.

---

### Task 6.3 — Implement `ProfileViewComponent`

**Goal:** Display current user's profile and their skill list with edit access.

Key logic:
- Load current user from Supabase session (`supabase.auth.getUser()`)
- Fetch user details via `userService.getById(userId)`
- Fetch user skills via `userSkillService.getByUserId(userId)`
- Show skill badges grouped by status (Learning / Proficient / Expert)
- "Edit Profile" button → `/profile/edit`

**Done when:** Profile page renders user info and skills correctly.

---

### Task 6.4 — Implement `ProfileEditComponent`

**Goal:** Edit profile fields and manage skill list.

Key logic:
- Pre-populate form with current user data
- Reactive form with `name`, `bio`, `department` fields
- Add/remove skills via search-and-select
- `PUT /api/users/:id` on save

**Done when:** Editing and saving reflects changes on profile view page.

---

### Task 6.5 — Implement `BookingFormComponent`

**Goal:** Book a 1-on-1 session with a skill provider.

**Route:** `/bookings/new?providerId=X&skillId=Y`

Key logic:
- Pre-fill provider and skill from query params
- Date/time picker for `scheduledAt`
- Duration selector (15, 30, 45, 60 min)
- Optional message field
- `POST /api/bookings` on submit

**Done when:** Submitting form creates a booking and redirects to `/bookings`.

---

### Task 6.6 — Implement `BookingListComponent`

**Goal:** Show all bookings for the current user (both as requester and provider).

Key logic:
- Two tabs: "My Requests" and "Incoming Requests"
- Status badges (Pending / Confirmed / Cancelled / Completed)
- Provider tab: "Confirm" / "Cancel" action buttons → `PATCH /api/bookings/:id/status`

**Done when:** Bookings list shows correctly; status changes update immediately.

---

### Task 6.7 — Add Supabase Auth integration with Google OAuth in Angular

**Goal:** Authenticate users exclusively via Google Sign-In, restricting access to company email accounts.

**Steps:**

1. In **Supabase Dashboard → Authentication → Providers**, enable Google. Paste in your Google Cloud OAuth 2.0 Client ID and Secret.
2. In **Google Cloud Console**, create an OAuth 2.0 credential. Add `https://YOUR_PROJECT.supabase.co/auth/v1/callback` as an authorized redirect URI.
3. In **Supabase Dashboard → Authentication → URL Configuration**, add `http://localhost:4200` to Redirect URLs.

```bash
cd frontend
npm install @supabase/supabase-js
```

**File:** `src/app/core/services/auth.service.ts`
```typescript
import { Injectable } from '@angular/core';
import { createClient, SupabaseClient, User } from '@supabase/supabase-js';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private supabase: SupabaseClient = createClient(
    environment.supabaseUrl, environment.supabaseAnonKey
  );

  // Google OAuth — opens the Google consent screen
  signInWithGoogle() {
    return this.supabase.auth.signInWithOAuth({
      provider: 'google',
      options: {
        redirectTo: `${window.location.origin}/auth/callback`,
        // Hint: restrict to company domain in Google Cloud Console
        // under "Authorized domains" for an extra layer of control
      }
    });
  }

  signOut() { return this.supabase.auth.signOut(); }

  getUser(): Promise<User | null> {
    return this.supabase.auth.getUser().then(r => r.data.user);
  }

  getSession() { return this.supabase.auth.getSession(); }

  // Listen to auth state changes (useful for auth guard)
  onAuthStateChange(callback: (event: string, session: any) => void) {
    return this.supabase.auth.onAuthStateChange(callback);
  }
}
```

**Done when:** Calling `signInWithGoogle()` redirects to the Google consent screen and returns to the app with a valid session.

---

### Task 6.8 — Create HTTP interceptor for auth token

**File:** `src/app/core/interceptors/auth.interceptor.ts`
```typescript
import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { from, switchMap } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  return from(auth.getSession()).pipe(
    switchMap(({ data }) => {
      const token = data.session?.access_token;
      if (!token) return next(req);
      return next(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }));
    })
  );
};
```

Register in `app.config.ts`:
```typescript
provideHttpClient(withInterceptors([authInterceptor]))
```

**Done when:** API calls include the `Authorization: Bearer <token>` header in DevTools Network tab.

---

### Task 6.9 — Create `LoginComponent` with Google Sign-In

**File:** `features/auth/login/login.component.ts`
```typescript
import { Component, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  template: `
    <div class="login-page">
      <h1>BenchPulse</h1>
      <p>Sign in with your company Google account to continue.</p>
      <button (click)="signIn()" class="btn-google">
        <img src="assets/google-logo.svg" alt="Google" />
        Sign in with Google
      </button>
    </div>
  `
})
export class LoginComponent {
  private auth = inject(AuthService);

  async signIn() {
    await this.auth.signInWithGoogle();
    // Supabase handles the redirect; user returns to /auth/callback
  }
}
```

**File:** `features/auth/callback/auth-callback.component.ts` — handles the OAuth redirect from Supabase:
```typescript
import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({ selector: 'app-auth-callback', standalone: true, template: '<p>Signing you in...</p>' })
export class AuthCallbackComponent implements OnInit {
  private auth = inject(AuthService);
  private router = inject(Router);

  async ngOnInit() {
    // Supabase automatically picks up the token from the URL hash
    const user = await this.auth.getUser();
    if (user) {
      this.router.navigate(['/']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}
```

Add routes:
```typescript
{ path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
{ path: 'auth/callback', loadComponent: () => import('./features/auth/callback/auth-callback.component').then(m => m.AuthCallbackComponent) },
```

Add `AuthGuard` to all protected routes — redirect to `/login` if no session.

**Done when:** Clicking "Sign in with Google" redirects to Google, returns to the app, and non-company emails are blocked with a friendly error message.

---

### Task 6.10 — Add loading states and error handling

**Goal:** Every async operation shows a spinner and surfaces errors gracefully.

- Create a shared `LoadingSpinnerComponent` (standalone).
- Create a shared `ErrorAlertComponent` for API errors.
- Use Angular Signals for loading/error state in every feature component.

**Done when:** Network tab shows requests; spinner appears during loads; errors show a dismissible alert.

---

### Task 6.11 — Style the app (minimal)

**Goal:** A clean, usable UI — not a design masterpiece, just functional.

Apply basic styles to:
- `styles.css` — CSS variables for colors, font-family, button styles
- Skill cards with hover state
- Status badge colors (Learning=blue, Proficient=green, Expert=gold)
- Responsive grid layout for skills

**Done when:** App looks presentable at 1280px and 375px (mobile).

---

### Task 6.12 — Verify full front-to-back flow

**Goal:** End-to-end manual smoke test before automation.

Steps:
1. Register a user via Supabase Auth
2. Create a profile and add two skills
3. Search for a skill and find a peer
4. Book a session
5. Switch to provider account and confirm

**Done when:** Full user journey completes without errors in the browser console.

---

## Phase 7 — Playwright Setup & POMs
**Estimated time:** 2–3 hours | **Week 2, Day 3**

---

### Task 7.1 — Initialize Playwright

**Steps:**
```bash
cd e2e-tests
npm init -y
npm install -D @playwright/test
npx playwright install chromium
```

**Done when:** `npx playwright --version` prints a version number.

---

### Task 7.2 — Create `playwright.config.ts`

**File:** `e2e-tests/playwright.config.ts`
```typescript
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  reporter: 'html',
  use: {
    baseURL: 'http://localhost:4200',
    trace: 'on-first-retry',
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
  ],
  webServer: {
    command: 'cd ../frontend && ng serve',
    url: 'http://localhost:4200',
    reuseExistingServer: !process.env.CI,
  },
});
```

**Done when:** `npx playwright test --list` runs without error (even if 0 tests found).

---

### Task 7.3 — Install `playwright-cli`

**Goal:** Use CLI snapshots to generate accurate accessibility trees for POM creation.

**Steps:**
```bash
npm install -D playwright-cli
```

**Done when:** `npx playwright-cli --version` prints a version.

---

### Task 7.4 — Snapshot the Skill Search page

**Goal:** Capture the accessibility tree to know exact locators for the Search POM.

**Steps:**
1. Ensure Angular app is running: `ng serve`
2. Run:
```bash
npx playwright-cli open http://localhost:4200
# In the opened browser, navigate to the skill search page
# Then in the terminal:
npx playwright-cli snapshot http://localhost:4200
```
3. Copy the accessibility tree output to a file: `e2e-tests/snapshots/skill-search.txt`

Use the snapshot to identify: search input role/label, skill card roles, button labels.

**Done when:** `skill-search.txt` exists with a readable tree; locators are clear.

---

### Task 7.5 — Snapshot the Booking Form page

```bash
npx playwright-cli snapshot http://localhost:4200/bookings/new
```

Save to `e2e-tests/snapshots/booking-form.txt`.

**Done when:** Snapshot reveals all form field labels and button text.

---

### Task 7.6 — Snapshot the Profile page

```bash
npx playwright-cli snapshot http://localhost:4200/profile
```

Save to `e2e-tests/snapshots/profile.txt`.

**Done when:** Snapshot is saved; user info and skill badge locators are identifiable.

---

### Task 7.7 — Create `SkillSearchPage` POM

**File:** `e2e-tests/tests/page-objects/pages/SkillSearchPage.ts`
```typescript
import { Page, Locator } from '@playwright/test';

export class SkillSearchPage {
  readonly searchInput: Locator;
  readonly searchButton: Locator;
  readonly skillCards: Locator;

  constructor(private page: Page) {
    // Locators derived from playwright-cli snapshot (Task 7.4)
    this.searchInput = page.getByRole('textbox', { name: /search skills/i });
    this.searchButton = page.getByRole('button', { name: /search/i });
    this.skillCards = page.locator('.skill-card');
  }

  async goto() { await this.page.goto('/'); }

  async search(query: string) {
    await this.searchInput.fill(query);
    await this.searchButton.click();
  }

  async getSkillNames(): Promise<string[]> {
    return this.skillCards.locator('h3').allTextContents();
  }
}
```

**Done when:** POM class compiles with no TypeScript errors.

---

### Task 7.8 — Create `BookingPage` POM

**File:** `e2e-tests/tests/page-objects/pages/BookingPage.ts`

Using locators from snapshot (Task 7.5), create a POM with methods:
- `goto(providerId, skillId)` — navigates to the booking form with query params
- `fillForm(date, duration, message)` — fills in all fields
- `submit()` — clicks the submit button
- `getConfirmationMessage()` — returns success message text

**Done when:** POM compiles; methods are callable.

---

### Task 7.9 — Create `ProfilePage` POM

**File:** `e2e-tests/tests/page-objects/pages/ProfilePage.ts`

Methods:
- `goto()`
- `getUserName()` — returns displayed full name
- `getSkillBadges()` — returns array of skill badge texts
- `clickEditProfile()` — navigates to edit page

**Done when:** POM compiles.

---

### Task 7.10 — Create `Navbar` component POM

**File:** `e2e-tests/tests/page-objects/components/Navbar.ts`
```typescript
import { Page } from '@playwright/test';

export class Navbar {
  constructor(private page: Page) {}

  async navigateTo(section: 'skills' | 'bookings' | 'profile') {
    await this.page.getByRole('navigation').getByRole('link', { name: new RegExp(section, 'i') }).click();
  }
}
```

**Done when:** Navbar POM compiles and can navigate between sections.

---

## Phase 8 — API & Integration Tests
**Estimated time:** 3–4 hours | **Week 2, Day 4**

> This phase uses Playwright AI Agents: **planner** (defines test scenarios), **generator** (writes test code), and where needed, **healer** (fixes broken locators/assertions).

---

### Task 8.1 — Initialize Playwright AI Agent session (Planner)

**Goal:** Use the Playwright planner agent to define test scenarios for each API endpoint.

**Steps:**
1. Open your AI agent tool (e.g., VS Code Playwright extension or CLI).
2. Run the planner against the running API:
```bash
# With Playwright MCP / AI agent tooling:
npx playwright-mcp plan --target http://localhost:5001/api \
  --context "BenchPulse REST API: users, skills, user-skills, bookings. Supabase JWT auth."
```
3. Review the generated test plan. It should cover:
   - CRUD happy paths for all four resources
   - 404 for missing resources
   - 400 for invalid payloads
   - 401 for unauthenticated requests

**Done when:** Planner outputs a structured test scenario list (save to `e2e-tests/docs/api-test-plan.md`).

---

### Task 8.2 — Generate Users API tests

**Goal:** Use the generator agent to produce test code from the planner output.

**Steps:**
```bash
npx playwright-mcp generate \
  --plan e2e-tests/docs/api-test-plan.md \
  --section "Users" \
  --output e2e-tests/tests/api/users.api.spec.ts
```

Manually verify the generated file includes:
```typescript
import { test, expect } from '@playwright/test';

test.describe('Users API', () => {
  test('@smoke GET /api/users returns 200', async ({ request }) => {
    const res = await request.get('/api/users');
    expect(res.status()).toBe(200);
    expect(await res.json()).toBeInstanceOf(Array);
  });

  test('POST /api/users creates a user', async ({ request }) => {
    const res = await request.post('/api/users', {
      data: { fullName: 'Test User', email: `test_${Date.now()}@example.com` }
    });
    expect(res.status()).toBe(201);
    const body = await res.json();
    expect(body.id).toBeDefined();
  });

  // ... generated CRUD tests
});
```

**Done when:** `npx playwright test tests/api/users.api.spec.ts` runs; all tests pass.

---

### Task 8.3 — Generate Skills API tests (with search)

Generate `tests/api/skills.api.spec.ts`. Key test to verify:

```typescript
test('@smoke GET /api/skills/search?q=angular returns matching skills', async ({ request }) => {
  const res = await request.get('/api/skills/search?q=angular');
  expect(res.status()).toBe(200);
  const skills = await res.json();
  expect(skills.every((s: any) => s.name.toLowerCase().includes('angular'))).toBe(true);
});
```

**Done when:** All skills API tests pass, including the search test.

---

### Task 8.4 — Generate Bookings API tests

Generate `tests/api/bookings.api.spec.ts`. Ensure tests cover:
- Creating a booking
- Fetching bookings by requester ID
- Updating booking status (Pending → Confirmed)
- 400 when scheduling in the past

**Done when:** All booking API tests pass.

---

### Task 8.5 — Create test fixtures

**File:** `e2e-tests/tests/fixtures/test.fixtures.ts`
```typescript
import { test as base, request as baseRequest } from '@playwright/test';

type TestFixtures = {
  apiUrl: string;
  testUserId: string;
  testSkillId: string;
};

export const test = base.extend<TestFixtures>({
  apiUrl: async ({}, use) => use(process.env.API_URL || 'http://localhost:5001'),

  testUserId: async ({ request }, use) => {
    const res = await request.post('/api/users', {
      data: { fullName: 'Fixture User', email: `fixture_${Date.now()}@test.com` }
    });
    const user = await res.json();
    await use(user.id);
    await request.delete(`/api/users/${user.id}`);
  },

  testSkillId: async ({ request }, use) => {
    const res = await request.post('/api/skills', {
      data: { name: `TestSkill_${Date.now()}`, category: 'Testing' }
    });
    const skill = await res.json();
    await use(skill.id);
    await request.delete(`/api/skills/${skill.id}`);
  },
});
```

**Done when:** Fixtures compile and are importable in test files.

---

### Task 8.6 — Create `user-skills.integration.spec.ts`

**Goal:** Test the full user ↔ skill relationship flow end-to-end at the API level.

```typescript
test('User can add a skill and mark it as teachable', async ({ request, testUserId, testSkillId }) => {
  const res = await request.post('/api/userskills', {
    data: { userId: testUserId, skillId: testSkillId, status: 1, isTeachable: true }
  });
  expect(res.status()).toBe(201);
  const userSkill = await res.json();
  expect(userSkill.isTeachable).toBe(true);
});
```

**Done when:** Integration test passes; relationship is visible in the database.

---

### Task 8.7 — Create `booking-flow.integration.spec.ts`

**Goal:** Test the complete booking lifecycle: create → confirm → complete.

**Steps:**
1. Create two users (requester, provider)
2. Add a shared skill
3. Create a booking
4. Update status to Confirmed
5. Update status to Completed
6. Assert final status

**Done when:** All three status transitions pass in sequence.

---

### Task 8.8 — Tag all smoke tests

**Goal:** Ensure `@smoke`-tagged tests can be run in isolation for CI feature branch checks.

**Convention:** Prefix critical path tests with `@smoke`:
```typescript
test('@smoke GET /api/skills returns 200', ...)
```

Run smoke tests:
```bash
npx playwright test --grep @smoke
```

**Done when:** `--grep @smoke` runs 6–8 tests in under 30 seconds; all pass.

---

### Task 8.9 — Run full API test suite

```bash
cd e2e-tests
npx playwright test tests/api/ tests/integration/
```

**Done when:** All tests pass; HTML report opens with 0 failures.

---

### Task 8.10 — Use Healer agent for any broken tests

**Goal:** If any generated test has broken locators or wrong assertions, use the Playwright healer agent.

**Steps:**
```bash
npx playwright-mcp heal \
  --spec e2e-tests/tests/api/users.api.spec.ts \
  --error "AssertionError: expected 404 to equal 200"
```

The healer analyses the error, current API response, and proposes a fix. Review and apply.

**Done when:** All previously failing tests are fixed and green.

---

## Phase 9 — E2E Journey Tests
**Estimated time:** 3–4 hours | **Week 2, Day 5**

---

### Task 9.1 — Plan E2E scenarios with Playwright Planner agent

**Goal:** Generate a structured E2E test plan covering the three main user journeys.

```bash
npx playwright-mcp plan \
  --target http://localhost:4200 \
  --context "BenchPulse Angular SPA: skill search, profile management, session booking" \
  --journeys "search for a skill and find a peer, edit profile and add a skill, book a session and confirm it"
```

Save output to `e2e-tests/docs/e2e-test-plan.md`.

**Done when:** Plan covers login, skill search, profile edit, and booking confirmation scenarios.

---

### Task 9.2 — Generate `skill-search.journey.spec.ts`

```bash
npx playwright-mcp generate \
  --plan e2e-tests/docs/e2e-test-plan.md \
  --section "Skill Search Journey" \
  --output e2e-tests/tests/e2e/skill-search.journey.spec.ts
```

Ensure the generated spec uses `SkillSearchPage` POM. Verify key test:
```typescript
test('@smoke user can search for Angular and see results', async ({ page }) => {
  const searchPage = new SkillSearchPage(page);
  await searchPage.goto();
  await searchPage.search('Angular');
  const names = await searchPage.getSkillNames();
  expect(names.length).toBeGreaterThan(0);
  expect(names.some(n => n.toLowerCase().includes('angular'))).toBe(true);
});
```

**Done when:** Journey test passes end-to-end in a real browser.

---

### Task 9.3 — Generate `profile-management.journey.spec.ts`

Key scenarios:
1. `@smoke` — User can view their profile page
2. User can edit bio and department
3. User can add a skill with status "Learning"
4. User can mark a skill as teachable
5. Saved changes persist on page reload

**Done when:** All five profile scenarios pass.

---

### Task 9.4 — Generate `booking-flow.journey.spec.ts`

Key scenarios:
1. `@smoke` — User can navigate to a skill detail page
2. User can click "Book Session" and see the form pre-filled
3. User can submit a booking and see it in their booking list
4. Provider can confirm a booking

**Done when:** Full booking flow passes as a browser-level E2E test.

---

### Task 9.5 — Create `global.setup.ts` for auth state

**File:** `e2e-tests/tests/global.setup.ts`
```typescript
import { chromium, FullConfig } from '@playwright/test';

async function globalSetup(config: FullConfig) {
  const browser = await chromium.launch();
  const page = await browser.newPage();

  // Sign in via Supabase UI
  await page.goto(`${config.projects[0].use.baseURL}/login`);
  await page.fill('[name="email"]', process.env.TEST_USER_EMAIL!);
  await page.fill('[name="password"]', process.env.TEST_USER_PASSWORD!);
  await page.click('button[type="submit"]');
  await page.waitForURL('/');

  // Save auth state
  await page.context().storageState({ path: 'e2e-tests/.auth/user.json' });
  await browser.close();
}

export default globalSetup;
```

In `playwright.config.ts`:
```typescript
globalSetup: require.resolve('./tests/global.setup'),
use: { storageState: '.auth/user.json' }
```

**Done when:** Tests run with pre-authenticated state; no login step in individual tests.

---

### Task 9.6 — Run full E2E suite

```bash
cd e2e-tests
npx playwright test tests/e2e/
```

**Done when:** All E2E journey tests pass; HTML report shows green.

---

### Task 9.7 — Apply Healer agent to flaky tests

**Goal:** Fix any timing-related or selector-broken E2E tests.

```bash
npx playwright-mcp heal \
  --spec e2e-tests/tests/e2e/booking-flow.journey.spec.ts \
  --error "TimeoutError: locator.click: Timeout 30000ms exceeded"
```

The healer may suggest adding explicit waits or updating selectors. Review carefully before applying.

**Done when:** No flaky tests; suite runs green 3 times in a row.

---

### Task 9.8 — Verify smoke test isolation

```bash
cd e2e-tests
npx playwright test --grep @smoke
```

**Done when:** Only `@smoke`-tagged tests run; result: 100% pass; total time < 2 minutes.

---

## Phase 10 — CI/CD (GitHub Actions)
**Estimated time:** 1–2 hours | **Week 3, Day 1**

---

### Task 10.1 — Create CI workflow

**File:** `.github/workflows/ci.yml`
```yaml
name: BenchPulse CI

on:
  push:
    branches: ['**']
  pull_request:
    branches: [main]

env:
  DOTNET_VERSION: '8.0.x'
  NODE_VERSION: '20.x'

jobs:
  # ── Backend build & test ─────────────────────────────────────────
  backend:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: '${{ env.DOTNET_VERSION }}' }
      - run: dotnet restore backend/BenchPulse/BenchPulse.sln
      - run: dotnet build backend/BenchPulse/BenchPulse.sln --no-restore

  # ── Frontend build ───────────────────────────────────────────────
  frontend:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '${{ env.NODE_VERSION }}' }
      - run: cd frontend && npm ci
      - run: cd frontend && npx ng build --configuration production

  # ── Playwright smoke tests (every branch push) ───────────────────
  smoke-tests:
    runs-on: ubuntu-latest
    needs: [backend, frontend]
    if: github.ref != 'refs/heads/main'
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '${{ env.NODE_VERSION }}' }
      - run: cd e2e-tests && npm ci
      - run: npx playwright install --with-deps chromium
      - run: cd e2e-tests && npx playwright test --grep @smoke
        env:
          API_URL: ${{ secrets.API_URL }}
          TEST_USER_EMAIL: ${{ secrets.TEST_USER_EMAIL }}
          TEST_USER_PASSWORD: ${{ secrets.TEST_USER_PASSWORD }}
      - uses: actions/upload-artifact@v4
        if: always()
        with:
          name: smoke-report
          path: e2e-tests/playwright-report/

  # ── Full test suite (PRs and pushes to main only) ────────────────
  full-tests:
    runs-on: ubuntu-latest
    needs: [backend, frontend]
    if: github.ref == 'refs/heads/main' || github.event_name == 'pull_request'
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: '${{ env.NODE_VERSION }}' }
      - run: cd e2e-tests && npm ci
      - run: npx playwright install --with-deps chromium
      - run: cd e2e-tests && npx playwright test
        env:
          API_URL: ${{ secrets.API_URL }}
          TEST_USER_EMAIL: ${{ secrets.TEST_USER_EMAIL }}
          TEST_USER_PASSWORD: ${{ secrets.TEST_USER_PASSWORD }}
      - uses: actions/upload-artifact@v4
        if: always()
        with:
          name: full-test-report
          path: e2e-tests/playwright-report/
```

**Done when:** Workflow file is committed; GitHub Actions tab shows the workflow listed.

---

### Task 10.2 — Add GitHub Secrets

In the GitHub repo: **Settings → Secrets and variables → Actions**, add:
- `API_URL` — your deployed or CI API URL
- `TEST_USER_EMAIL` — test account email
- `TEST_USER_PASSWORD` — test account password
- `SUPABASE_URL` — Supabase project URL
- `SUPABASE_ANON_KEY` — Supabase anon key

**Done when:** All secrets are visible in the Actions secrets list (values hidden).

---

### Task 10.3 — Trigger a feature branch push

```bash
git checkout -b feature/ci-test
git commit --allow-empty -m "ci: test feature branch smoke run"
git push origin feature/ci-test
```

**Done when:** GitHub Actions runs only the `smoke-tests` job; full-tests job is skipped.

---

### Task 10.4 — Trigger a PR to `main`

Create a PR from `feature/ci-test` to `main`.

**Done when:** GitHub Actions runs BOTH `smoke-tests` and `full-tests` jobs; both pass.

---

### Task 10.5 — Add status badge to README

```markdown
![CI](https://github.com/YOUR_ORG/benchpulse/actions/workflows/ci.yml/badge.svg)
```

**Done when:** Badge appears on the GitHub README showing workflow status.

---

## Phase 11 — Dockerize the Full Stack
**Estimated time:** 3–4 hours | **Week 3, Day 2**

---

### Task 11.1 — Create Backend Dockerfile

**File:** `backend/BenchPulse/Dockerfile`
```dockerfile
# ── Build stage ──────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BenchPulse.sln .
COPY BenchPulse.Domain/BenchPulse.Domain.csproj BenchPulse.Domain/
COPY BenchPulse.Application/BenchPulse.Application.csproj BenchPulse.Application/
COPY BenchPulse.Infrastructure/BenchPulse.Infrastructure.csproj BenchPulse.Infrastructure/
COPY BenchPulse.Api/BenchPulse.Api.csproj BenchPulse.Api/

RUN dotnet restore BenchPulse.sln

COPY . .
RUN dotnet publish BenchPulse.Api/BenchPulse.Api.csproj \
    -c Release -o /app/publish --no-restore

# ── Runtime stage ────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BenchPulse.Api.dll"]
```

**Done when:** `docker build -t benchpulse-api .` completes successfully from `backend/BenchPulse/`.

---

### Task 11.2 — Create Frontend Dockerfile

**File:** `frontend/Dockerfile`
```dockerfile
# ── Build stage ──────────────────────────────────────────────────
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npx ng build --configuration production

# ── Runtime stage (nginx) ────────────────────────────────────────
FROM nginx:alpine AS runtime
COPY --from=build /app/dist/benchpulse-ui/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
```

**File:** `frontend/nginx.conf`
```nginx
server {
    listen 80;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api/ {
        proxy_pass http://api:8080/api/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}
```

**Done when:** `docker build -t benchpulse-ui .` completes from `frontend/`.

---

### Task 11.3 — Create `docker-compose.yml`

**File:** `docker-compose.yml` (repo root)
```yaml
version: '3.9'

services:
  api:
    build:
      context: ./backend/BenchPulse
      dockerfile: Dockerfile
    ports:
      - "5001:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=${SUPABASE_CONNECTION_STRING}
      - Supabase__Url=${SUPABASE_URL}
      - Supabase__AnonKey=${SUPABASE_ANON_KEY}
      - AllowedOrigins__0=http://localhost:80
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
      interval: 30s
      timeout: 10s
      retries: 3

  ui:
    build:
      context: ./frontend
      dockerfile: Dockerfile
    ports:
      - "4200:80"
    depends_on:
      api:
        condition: service_healthy

networks:
  default:
    name: benchpulse-network
```

**Done when:** File is valid YAML; no syntax errors.

---

### Task 11.4 — Create `.env` for Docker Compose

**File:** `.env` (gitignored, at repo root)
```
SUPABASE_CONNECTION_STRING=Host=db.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR_PASSWORD
SUPABASE_URL=https://YOUR_PROJECT.supabase.co
SUPABASE_ANON_KEY=YOUR_ANON_KEY
```

**Done when:** File exists; is listed in `.gitignore`.

---

### Task 11.5 — Add health check endpoint to API

**File:** `BenchPulse.Api/Program.cs` — add:
```csharp
app.MapHealthChecks("/health");
builder.Services.AddHealthChecks();
```

**Done when:** `GET /health` returns `200 Healthy` from the running container.

---

### Task 11.6 — Build and run full stack via Compose

```bash
docker-compose up --build
```

**Done when:**
- `http://localhost:5001/swagger` shows the API
- `http://localhost:4200` shows the Angular app
- Both containers are `healthy` in `docker-compose ps`

---

### Task 11.7 — Verify cross-container communication

**Goal:** Confirm Angular app (in UI container) can call the API (in API container).

**Steps:**
1. Open `http://localhost:4200`
2. Navigate to Skill Search
3. Confirm skills load from the API (check Network tab: calls to `localhost:5001/api`)

**Done when:** API responses appear in the browser Network tab with status 200.

---

### Task 11.8 — Add Compose override for development

**File:** `docker-compose.override.yml`
```yaml
version: '3.9'
services:
  api:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    volumes:
      - ./backend/BenchPulse:/src
```

**Done when:** `docker-compose up` in development mode hot-reloads the API on code changes.

---

### Task 11.9 — Add Compose to CI pipeline

In `.github/workflows/ci.yml`, add a Docker build verification job:
```yaml
  docker-build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: docker-compose build
```

**Done when:** CI builds Docker images successfully on every push.

---

## Phase 12 — .NET Aspire (Local Orchestration)
**Estimated time:** 2–3 hours | **Week 3, Day 3**

---

### Task 12.1 — Install .NET Aspire workload

**Steps:**
```bash
dotnet workload install aspire
dotnet workload update
```

**Done when:** `dotnet workload list` shows `aspire` as installed.

---

### Task 12.2 — Add Aspire projects to the solution

**Steps:**
```bash
cd backend/BenchPulse

dotnet new aspire-apphost -n BenchPulse.AppHost -o BenchPulse.AppHost
dotnet new aspire-servicedefaults -n BenchPulse.ServiceDefaults -o BenchPulse.ServiceDefaults

dotnet sln add BenchPulse.AppHost/BenchPulse.AppHost.csproj
dotnet sln add BenchPulse.ServiceDefaults/BenchPulse.ServiceDefaults.csproj
```

**Done when:** `dotnet build BenchPulse.sln` succeeds with the two new projects included.

---

### Task 12.3 — Configure `BenchPulse.ServiceDefaults`

**Goal:** Define shared observability defaults (health checks, OpenTelemetry, service discovery).

The generated `Extensions.cs` in `BenchPulse.ServiceDefaults` should expose:
```csharp
public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
{
    builder.ConfigureOpenTelemetry();
    builder.AddDefaultHealthChecks();
    builder.Services.AddServiceDiscovery();
    builder.Services.ConfigureHttpClientDefaults(http =>
    {
        http.AddStandardResilienceHandler();
        http.AddServiceDiscovery();
    });
    return builder;
}
```

**Done when:** `ServiceDefaults` compiles; extension method is accessible.

---

### Task 12.4 — Wire API to ServiceDefaults

Add reference and call in `BenchPulse.Api`:
```bash
dotnet add BenchPulse.Api/BenchPulse.Api.csproj reference BenchPulse.ServiceDefaults/BenchPulse.ServiceDefaults.csproj
```

In `BenchPulse.Api/Program.cs`:
```csharp
builder.AddServiceDefaults();
// ... existing registrations ...
app.MapDefaultEndpoints(); // health + alive endpoints
```

**Done when:** API starts and `/health` returns healthy with Aspire telemetry attached.

---

### Task 12.5 — Configure `BenchPulse.AppHost`

**File:** `BenchPulse.AppHost/Program.cs`
```csharp
var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.BenchPulse_Api>("benchpulse-api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.AddNpmApp("benchpulse-ui", "../../frontend", "start")
    .WithReference(api)
    .WithHttpEndpoint(port: 4200, env: "PORT")
    .PublishAsDockerFile();

builder.Build().Run();
```

**Done when:** File compiles; `dotnet run --project BenchPulse.AppHost` starts without errors.

---

### Task 12.6 — Launch Aspire dashboard

```bash
cd backend/BenchPulse
dotnet run --project BenchPulse.AppHost
```

**Done when:**
- Terminal shows the Aspire dashboard URL (typically `https://localhost:15888`)
- Dashboard shows `benchpulse-api` and `benchpulse-ui` as running resources
- Both services show green health status
- Structured logs and traces are visible in the dashboard

---

### Task 12.7 — Verify one-command startup

**Goal:** Confirm the entire stack (API + Angular) starts with a single `dotnet run` from the AppHost.

**Steps:**
1. Stop all running processes
2. Run: `dotnet run --project backend/BenchPulse/BenchPulse.AppHost`
3. Wait for both services to be healthy
4. Open `http://localhost:4200` — app loads
5. Open `https://localhost:5001/swagger` — API is live
6. Open Aspire dashboard — all services green

**Done when:** All three URLs are accessible; dashboard shows 0 unhealthy services. **The full BenchPulse stack is running from one command.**

---

## Appendix — Key Commands Reference

```bash
# ── Backend ──────────────────────────────────────────────────────
dotnet run --project backend/BenchPulse/BenchPulse.Api
dotnet ef migrations add <Name> --project BenchPulse.Infrastructure --startup-project BenchPulse.Api
dotnet ef database update --project BenchPulse.Infrastructure --startup-project BenchPulse.Api

# ── Frontend ─────────────────────────────────────────────────────
cd frontend && ng serve
cd frontend && ng build --configuration production

# ── Tests ────────────────────────────────────────────────────────
cd e2e-tests && npx playwright test                  # all tests
cd e2e-tests && npx playwright test --grep @smoke    # smoke only
cd e2e-tests && npx playwright show-report           # open HTML report

# ── Docker ───────────────────────────────────────────────────────
docker-compose up --build                            # full stack
docker-compose down -v                               # stop + clean volumes

# ── Aspire ───────────────────────────────────────────────────────
dotnet run --project backend/BenchPulse/BenchPulse.AppHost

# ── Playwright CLI snapshots ─────────────────────────────────────
npx playwright-cli open http://localhost:4200
npx playwright-cli snapshot http://localhost:4200
```
