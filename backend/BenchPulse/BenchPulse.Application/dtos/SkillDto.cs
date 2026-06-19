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