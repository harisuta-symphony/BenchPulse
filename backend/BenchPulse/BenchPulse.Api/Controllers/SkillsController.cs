using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BenchPulse.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _service;
    public SkillsController(ISkillService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q) =>
        Ok(await _service.SearchAsync(q));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var skill = await _service.GetByIdAsync(id);
        return skill is null ? NotFound() : Ok(skill);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
