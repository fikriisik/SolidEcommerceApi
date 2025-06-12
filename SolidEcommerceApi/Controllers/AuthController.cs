using SolidEcommerceApi.DTOs;
using SolidEcommerceApi.Models;
using SolidEcommerceApi.Repositories;

namespace SolidEcommerceApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly IRefreshTokenRepository _refreshTokenRepo;
    private readonly IUserRepository _userRepository;
    private readonly IRevokedTokenRepository _revokedTokenRepo;

    public AuthController(IConfiguration config, IRefreshTokenRepository refreshTokenRepo, IUserRepository userRepository, IRevokedTokenRepository revokedTokenRepo)
    {
        _config = config;
        _refreshTokenRepo = refreshTokenRepo;
        _userRepository = userRepository;
        _revokedTokenRepo = revokedTokenRepo;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (dto.Username != "admin" || dto.Password != "123456")
            return Unauthorized();

        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

        
        var accessToken = JwtHelper.GenerateJwtToken(dto.Username);
        var refreshToken = await GenerateAndSaveRefreshToken(dto.Username);

        return Ok(new { accessToken, refreshToken });
    }

    private  async Task<string> GenerateAndSaveRefreshToken(string username)
    {
        var refreshToken = new RefreshToken
        {
            Username = username,
            Token = Guid.NewGuid().ToString(),
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepo.AddAsync(refreshToken);
        return refreshToken.Token;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
    {
        var storedToken = await _refreshTokenRepo.GetByTokenAsync(dto.RefreshToken);
        if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            return Unauthorized("Refresh token geçersiz veya süresi dolmuş.");
        

        var newAccessToken = JwtHelper.GenerateJwtToken(storedToken.Username);
        var newRefreshToken = await GenerateAndSaveRefreshToken(storedToken.Username);

        await _refreshTokenRepo.DeleteAsync(storedToken);

        return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var username = User.Identity?.Name;
        if (username == null) return Unauthorized();
        
        var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var exp = DateTimeOffset.FromUnixTimeSeconds(
            long.Parse(User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value ?? "0")
        ).UtcDateTime;

        if (!string.IsNullOrEmpty(jti))
        {
            await _revokedTokenRepo.AddAsync(jti, exp);
        }
        
        await _refreshTokenRepo.DeleteAllAsync(username);

        return Ok("Oturum sonlandırıldı.");
    }
    
    

}
