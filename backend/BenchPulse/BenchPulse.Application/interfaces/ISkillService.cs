using BenchPulse.Application.dtos;

namespace BenchPulse.Application.interfaces;

public interface ISkillService
{
    Task<IEnumerable<SkillDto>> GetAllAsync();
    Task<IEnumerable<SkillDto>> SearchAsync(string query);
    Task<SkillDto?> GetByIdAsync(Guid id);
    Task<SkillDto> CreateAsync(CreateSkillDto dto);
    Task DeleteAsync(Guid id);
}
