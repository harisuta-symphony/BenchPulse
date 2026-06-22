using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;
using BenchPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BenchPulse.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext _ctx;
    public SkillRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<SkillEntity>> GetAllAsync() =>
        await _ctx.Skills.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<SkillEntity>> SearchAsync(string query) =>
        await _ctx.Skills.AsNoTracking()
                         .Where(s => s.Name.ToLower().Contains(query.ToLower())
                                  || (s.Category != null && s.Category.ToLower().Contains(query.ToLower())))
                         .ToListAsync();

    public async Task<SkillEntity?> GetByIdAsync(Guid id) =>
        await _ctx.Skills.Include(s => s.UserSkills)
                         .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<SkillEntity> CreateAsync(SkillEntity skill)
    {
        skill.Id = Guid.NewGuid();
        skill.CreatedAt = DateTime.UtcNow;
        _ctx.Skills.Add(skill);
        await _ctx.SaveChangesAsync();
        return skill;
    }

    public async Task DeleteAsync(Guid id)
    {
        var skill = await _ctx.Skills.FindAsync(id);
        if (skill is not null) { _ctx.Skills.Remove(skill); await _ctx.SaveChangesAsync(); }
    }
}
