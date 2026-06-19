using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface ISkillRepository
{
    Task<IEnumerable<SkillEntity>> GetAllAsync();
    Task<IEnumerable<SkillEntity>> SearchAsync(string query);
    Task<SkillEntity?> GetByIdAsync(Guid id);
    Task<SkillEntity> CreateAsync(SkillEntity skill);
    Task DeleteAsync(Guid id);
}