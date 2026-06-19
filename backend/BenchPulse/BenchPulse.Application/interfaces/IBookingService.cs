using BenchPulse.Application.dtos;

namespace BenchPulse.Application.interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetByRequesterIdAsync(Guid requesterId);
    Task<IEnumerable<BookingDto>> GetByProviderIdAsync(Guid providerId);
    Task<BookingDto?> GetByIdAsync(Guid id);
    Task<BookingDto> CreateAsync(CreateBookingDto dto);
    Task<BookingDto> UpdateStatusAsync(Guid id, UpdateBookingStatusDto dto);
    Task DeleteAsync(Guid id);
}
