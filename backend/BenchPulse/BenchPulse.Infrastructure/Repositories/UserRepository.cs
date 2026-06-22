using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;
using BenchPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BenchPulse.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;
    public UserRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<UserEntity>> GetAllAsync() =>
        await _ctx.Users.AsNoTracking().ToListAsync();

    public async Task<UserEntity?> GetByIdAsync(Guid id) =>
        await _ctx.Users.Include(u => u.UserSkills)
                        .ThenInclude(us => us.Skill)
                        .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<UserEntity?> GetByEmailAsync(string email) =>
        await _ctx.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        _ctx.Users.Add(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task<UserEntity> UpdateAsync(UserEntity user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _ctx.Users.Update(user);
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _ctx.Users.FindAsync(id);
        if (user is not null) { _ctx.Users.Remove(user); await _ctx.SaveChangesAsync(); }
    }
}