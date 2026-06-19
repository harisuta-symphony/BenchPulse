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