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