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