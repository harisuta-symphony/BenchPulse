using BenchPulse.Application.dtos;
using BenchPulse.Application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BenchPulse.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/user-skills")]
public class UserSkillsController : ControllerBase
{
    private readonly IUserSkillService _service;
    public UserSkillsController(IUserSkillService service) => _service = service;

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId) =>
        Ok(await _service.GetByUserIdAsync(userId));

    [HttpGet("skill/{skillId:guid}")]
    public async Task<IActionResult> GetBySkill(Guid skillId) =>
        Ok(await _service.GetBySkillIdAsync(skillId));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userSkill = await _service.GetByIdAsync(id);
        return userSkill is null ? NotFound() : Ok(userSkill);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserSkillDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserSkillDto dto) =>
        Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
