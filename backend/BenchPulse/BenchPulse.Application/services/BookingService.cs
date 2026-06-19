using AutoMapper;
using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;

namespace BenchPulse.Application.services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repo;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookingDto>> GetByRequesterIdAsync(Guid requesterId)
    {
        var bookings = await _repo.GetByRequesterIdAsync(requesterId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetByProviderIdAsync(Guid providerId)
    {
        var bookings = await _repo.GetByProviderIdAsync(providerId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<BookingDto?> GetByIdAsync(Guid id)
    {
        var booking = await _repo.GetByIdAsync(id);
        return booking is null ? null : _mapper.Map<BookingDto>(booking);
    }

    public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
    {
        var entity = _mapper.Map<BookingEntity>(dto);
        var created = await _repo.CreateAsync(entity);
        return _mapper.Map<BookingDto>(created);
    }

    public async Task<BookingDto> UpdateStatusAsync(Guid id, UpdateBookingStatusDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Booking {id} not found.");
        _mapper.Map(dto, entity);
        var updated = await _repo.UpdateAsync(entity);
        return _mapper.Map<BookingDto>(updated);
    }

    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
}
