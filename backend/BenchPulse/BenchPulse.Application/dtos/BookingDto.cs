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