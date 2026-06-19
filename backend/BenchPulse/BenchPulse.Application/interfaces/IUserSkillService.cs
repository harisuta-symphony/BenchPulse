using BenchPulse.Application.dtos;

namespace BenchPulse.Application.interfaces;

public interface IUserSkillService
{
    Task<IEnumerable<UserSkillDto>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserSkillDto>> GetBySkillIdAsync(Guid skillId);
    Task<UserSkillDto?> GetByIdAsync(Guid id);
    Task<UserSkillDto> CreateAsync(CreateUserSkillDto dto);
    Task<UserSkillDto> UpdateAsync(Guid id, CreateUserSkillDto dto);
    Task DeleteAsync(Guid id);
}
