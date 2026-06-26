using AutoMapper;
using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.services;

public class UserSkillService : IUserSkillService
{
    private readonly IUserSkillRepository _repo;
    private readonly IMapper _mapper;

    public UserSkillService(IUserSkillRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserSkillDto>> GetByUserIdAsync(Guid userId)
    {
        var userSkills = await _repo.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<UserSkillDto>>(userSkills);
    }

    public async Task<IEnumerable<UserSkillDto>> GetBySkillIdAsync(Guid skillId)
    {
        var userSkills = await _repo.GetBySkillIdAsync(skillId);
        return _mapper.Map<IEnumerable<UserSkillDto>>(userSkills);
    }

    public async Task<UserSkillDto?> GetByIdAsync(Guid id)
    {
        var userSkill = await _repo.GetByIdAsync(id);
        return userSkill is null ? null : _mapper.Map<UserSkillDto>(userSkill);
    }

    public async Task<UserSkillDto> CreateAsync(CreateUserSkillDto dto)
    {
        var entity = _mapper.Map<UserSkillEntity>(dto);
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<UserSkillDto>(created);
    }

    public async Task<UserSkillDto> UpdateAsync(Guid id, UpdateUserSkillDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"UserSkill {id} not found.");
        if (dto.Status.HasValue) entity.Status = dto.Status.Value;
        if (dto.IsTeachable.HasValue) entity.IsTeachable = dto.IsTeachable.Value;
        if (dto.Notes != null) entity.Notes = dto.Notes;
        var updated = await _repo.UpdateAsync(entity);
        return _mapper.Map<UserSkillDto>(updated);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
}
