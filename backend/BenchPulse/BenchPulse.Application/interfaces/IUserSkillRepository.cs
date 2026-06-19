using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface IUserSkillRepository
{
    Task<IEnumerable<UserSkillEntity>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserSkillEntity>> GetBySkillIdAsync(Guid skillId);
    Task<UserSkillEntity?> GetByIdAsync(Guid id);
    Task<UserSkillEntity> CreateAsync(UserSkillEntity userSkill);
    Task<UserSkillEntity> UpdateAsync(UserSkillEntity userSkill);
    Task DeleteAsync(Guid id);
}