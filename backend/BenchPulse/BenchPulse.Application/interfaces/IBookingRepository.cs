using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.interfaces;

public interface IBookingRepository
{
    Task<IEnumerable<BookingEntity>> GetByRequesterIdAsync(Guid requesterId);
    Task<IEnumerable<BookingEntity>> GetByProviderIdAsync(Guid providerId);
    Task<BookingEntity?> GetByIdAsync(Guid id);
    Task<BookingEntity> CreateAsync(BookingEntity booking);
    Task<BookingEntity> UpdateAsync(BookingEntity booking);
    Task DeleteAsync(Guid id);
}