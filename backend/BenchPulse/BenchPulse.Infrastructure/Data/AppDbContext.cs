using BenchPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BenchPulse.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<SkillEntity> Skills => Set<SkillEntity>();
    public DbSet<UserSkillEntity> UserSkills => Set<UserSkillEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.FullName).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(320);
        });

        modelBuilder.Entity<SkillEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Name).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<UserSkillEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.SkillId }).IsUnique();
            e.HasOne(x => x.User)
             .WithMany(u => u.UserSkills)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Skill)
             .WithMany(s => s.UserSkills)
             .HasForeignKey(x => x.SkillId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BookingEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Requester)
             .WithMany(u => u.BookingsAsRequester)
             .HasForeignKey(x => x.RequesterId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Provider)
             .WithMany(u => u.BookingsAsProvider)
             .HasForeignKey(x => x.ProviderId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}