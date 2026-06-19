using AutoMapper;
using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var entity = _mapper.Map<UserEntity>(dto);
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<UserDto>(created);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");
        _mapper.Map(dto, entity);
        var updated = await _repo.UpdateAsync(entity);
        return _mapper.Map<UserDto>(updated);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
}
