namespace SolidEcommerceApi.Data;
using Microsoft.EntityFrameworkCore;
using SolidEcommerceApi.Models;



public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } // ✅ Bu şekilde tanımlanmalı
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<RevokedToken> RevokedTokens { get; set; }
}
