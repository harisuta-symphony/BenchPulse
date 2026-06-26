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

public class UpdateUserSkillDto
{
    public SkillStatus? Status { get; set; }
    public bool? IsTeachable { get; set; }
    public string? Notes { get; set; }
}