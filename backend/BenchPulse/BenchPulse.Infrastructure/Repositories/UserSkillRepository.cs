using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;
using BenchPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BenchPulse.Infrastructure.Repositories;

public class UserSkillRepository : IUserSkillRepository
{
    private readonly AppDbContext _ctx;
    public UserSkillRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<UserSkillEntity>> GetByUserIdAsync(Guid userId) =>
        await _ctx.UserSkills.AsNoTracking()
                             .Include(us => us.Skill)
                             .Where(us => us.UserId == userId)
                             .ToListAsync();

    public async Task<IEnumerable<UserSkillEntity>> GetBySkillIdAsync(Guid skillId) =>
        await _ctx.UserSkills.AsNoTracking()
                             .Include(us => us.User)
                             .Where(us => us.SkillId == skillId)
                             .ToListAsync();

    public async Task<UserSkillEntity?> GetByIdAsync(Guid id) =>
        await _ctx.UserSkills.Include(us => us.User)
                             .Include(us => us.Skill)
                             .FirstOrDefaultAsync(us => us.Id == id);

    public async Task<UserSkillEntity> CreateAsync(UserSkillEntity userSkill)
    {
        userSkill.Id = Guid.NewGuid();
        userSkill.CreatedAt = DateTime.UtcNow;
        userSkill.UpdatedAt = DateTime.UtcNow;
        _ctx.UserSkills.Add(userSkill);
        await _ctx.SaveChangesAsync();
        return userSkill;
    }

    public async Task<UserSkillEntity> UpdateAsync(UserSkillEntity userSkill)
    {
        userSkill.UpdatedAt = DateTime.UtcNow;
        _ctx.UserSkills.Update(userSkill);
        await _ctx.SaveChangesAsync();
        return userSkill;
    }

    public async Task DeleteAsync(Guid id)
    {
        var userSkill = await _ctx.UserSkills.FindAsync(id);
        if (userSkill is not null) { _ctx.UserSkills.Remove(userSkill); await _ctx.SaveChangesAsync(); }
    }
}
