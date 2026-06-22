using BenchPulse.Application.interfaces;
using BenchPulse.Application.services;
using Microsoft.Extensions.DependencyInjection;

namespace BenchPulse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IUserSkillService, UserSkillService>();
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }
}