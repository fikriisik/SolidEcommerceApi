using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolidEcommerceApi.DTOs;
using SolidEcommerceApi.Models;
using SolidEcommerceApi.Services;

namespace SolidEcommerceApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _service.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    //[HttpPost("register")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDto dto)
    {
        var existing = await _service.GetByUsernameAsync(dto.Username);
        if (existing != null) return BadRequest("Kullanıcı zaten mevcut.");

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User { Username = dto.Username, PasswordHash = hash };
        
        await _service.CreateAsync(user);
       // return Ok("Kayıt başarılı.");

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult GetAdminStuff()
    {
        return Ok("Sadece Adminler görebilir.");
    }
}
