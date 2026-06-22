using BenchPulse.Domain.Entities;
using BenchPulse.Domain.Enums;
using BenchPulse.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BenchPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _ctx;
    private readonly IWebHostEnvironment _env;

    public SeedController(AppDbContext ctx, IWebHostEnvironment env)
    {
        _ctx = ctx;
        _env = env;
    }

    [HttpPost]
    public async Task<IActionResult> Seed()
    {
        if (!_env.IsDevelopment())
            return Forbid();

        if (_ctx.Users.Any())
            return BadRequest("Database already seeded.");

        var users = new List<UserEntity>
        {
            new() { Id = Guid.NewGuid(), FullName = "Alice Martin",   Email = "alice@symphony.is",   Department = "Frontend",  Bio = "Angular enthusiast",      CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), FullName = "Bob Kovač",      Email = "bob@symphony.is",     Department = "Backend",   Bio = ".NET developer",          CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), FullName = "Clara Svensson", Email = "clara@symphony.is",   Department = "QA",        Bio = "Playwright test wizard",  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), FullName = "David Horváth",  Email = "david@symphony.is",   Department = "DevOps",    Bio = "Docker & CI/CD lover",    CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), FullName = "Emma Nowak",     Email = "emma@symphony.is",    Department = "Frontend",  Bio = "Signals and RxJS fan",    CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        var skills = new List<SkillEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Angular",        Category = "Frontend",  Description = "Component-based web framework",        CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Playwright",     Category = "Testing",   Description = "End-to-end browser testing",           CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = ".NET Core",      Category = "Backend",   Description = "Cross-platform C# web API framework",  CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Docker",         Category = "DevOps",    Description = "Containerisation platform",            CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "PostgreSQL",     Category = "Database",  Description = "Open-source relational database",      CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "TypeScript",     Category = "Frontend",  Description = "Typed superset of JavaScript",         CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Entity Framework Core", Category = "Backend", Description = "ORM for .NET",                   CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "GitHub Actions", Category = "DevOps",    Description = "CI/CD automation platform",           CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "RxJS",           Category = "Frontend",  Description = "Reactive extensions for JavaScript",   CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Supabase",       Category = "Backend",   Description = "Open-source Firebase alternative",     CreatedAt = DateTime.UtcNow },
        };

        await _ctx.Users.AddRangeAsync(users);
        await _ctx.Skills.AddRangeAsync(skills);
        await _ctx.SaveChangesAsync();

        var userSkills = new List<UserSkillEntity>
        {
            new() { Id = Guid.NewGuid(), UserId = users[0].Id, SkillId = skills[0].Id, Status = SkillStatus.Expert,     IsTeachable = true,  Notes = "5 years experience",    CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[0].Id, SkillId = skills[5].Id, Status = SkillStatus.Proficient, IsTeachable = true,  Notes = null,                     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[1].Id, SkillId = skills[2].Id, Status = SkillStatus.Expert,     IsTeachable = true,  Notes = "Clean Architecture fan", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[1].Id, SkillId = skills[6].Id, Status = SkillStatus.Proficient, IsTeachable = false, Notes = null,                     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[2].Id, SkillId = skills[1].Id, Status = SkillStatus.Expert,     IsTeachable = true,  Notes = "Wrote the test suite",   CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[2].Id, SkillId = skills[5].Id, Status = SkillStatus.Learning,   IsTeachable = false, Notes = null,                     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[3].Id, SkillId = skills[3].Id, Status = SkillStatus.Expert,     IsTeachable = true,  Notes = "Runs our k8s cluster",   CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[3].Id, SkillId = skills[7].Id, Status = SkillStatus.Expert,     IsTeachable = true,  Notes = null,                     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[4].Id, SkillId = skills[8].Id, Status = SkillStatus.Proficient, IsTeachable = true,  Notes = "RxJS patterns",          CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), UserId = users[4].Id, SkillId = skills[0].Id, Status = SkillStatus.Proficient, IsTeachable = false, Notes = null,                     CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        await _ctx.UserSkills.AddRangeAsync(userSkills);
        await _ctx.SaveChangesAsync();

        return Ok(new { users = users.Count, skills = skills.Count, userSkills = userSkills.Count });
    }
}
