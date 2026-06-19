using AutoMapper;
using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _repo;
    private readonly IMapper _mapper;

    public SkillService(ISkillRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SkillDto>> GetAllAsync()
    {
        var skills = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<SkillDto>>(skills);
    }

    public async Task<IEnumerable<SkillDto>> SearchAsync(string query)
    {
        var skills = await _repo.SearchAsync(query);
        return _mapper.Map<IEnumerable<SkillDto>>(skills);
    }

    public async Task<SkillDto?> GetByIdAsync(Guid id)
    {
        var skill = await _repo.GetByIdAsync(id);
        return skill is null ? null : _mapper.Map<SkillDto>(skill);
    }

    public async Task<SkillDto> CreateAsync(CreateSkillDto dto)
    {
        var entity = _mapper.Map<SkillEntity>(dto);
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<SkillDto>(created);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
}
