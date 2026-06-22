using BenchPulse.Application.interfaces;
using BenchPulse.Domain.Entities;
using BenchPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BenchPulse.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _ctx;
    public BookingRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<BookingEntity>> GetByRequesterIdAsync(Guid requesterId) =>
        await _ctx.Bookings.AsNoTracking()
                           .Include(b => b.Provider)
                           .Include(b => b.Skill)
                           .Where(b => b.RequesterId == requesterId)
                           .ToListAsync();

    public async Task<IEnumerable<BookingEntity>> GetByProviderIdAsync(Guid providerId) =>
        await _ctx.Bookings.AsNoTracking()
                           .Include(b => b.Requester)
                           .Include(b => b.Skill)
                           .Where(b => b.ProviderId == providerId)
                           .ToListAsync();

    public async Task<BookingEntity?> GetByIdAsync(Guid id) =>
        await _ctx.Bookings.Include(b => b.Requester)
                           .Include(b => b.Provider)
                           .Include(b => b.Skill)
                           .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<BookingEntity> CreateAsync(BookingEntity booking)
    {
        booking.Id = Guid.NewGuid();
        booking.CreatedAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;
        _ctx.Bookings.Add(booking);
        await _ctx.SaveChangesAsync();
        return booking;
    }

    public async Task<BookingEntity> UpdateAsync(BookingEntity booking)
    {
        booking.UpdatedAt = DateTime.UtcNow;
        _ctx.Bookings.Update(booking);
        await _ctx.SaveChangesAsync();
        return booking;
    }

    public async Task DeleteAsync(Guid id)
    {
        var booking = await _ctx.Bookings.FindAsync(id);
        if (booking is not null) { _ctx.Bookings.Remove(booking); await _ctx.SaveChangesAsync(); }
    }
}
