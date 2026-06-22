using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BenchPulse.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _service;
    public BookingsController(IBookingService service) => _service = service;

    [HttpGet("requester/{requesterId:guid}")]
    public async Task<IActionResult> GetByRequester(Guid requesterId) =>
        Ok(await _service.GetByRequesterIdAsync(requesterId));

    [HttpGet("provider/{providerId:guid}")]
    public async Task<IActionResult> GetByProvider(Guid providerId) =>
        Ok(await _service.GetByProviderIdAsync(providerId));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var booking = await _service.GetByIdAsync(id);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateBookingStatusDto dto) =>
        Ok(await _service.UpdateStatusAsync(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
