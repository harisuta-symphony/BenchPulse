using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<UserEntity> UpdateAsync(UserEntity user);
    Task DeleteAsync(Guid id);
}