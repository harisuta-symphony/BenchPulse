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