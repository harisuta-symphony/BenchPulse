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