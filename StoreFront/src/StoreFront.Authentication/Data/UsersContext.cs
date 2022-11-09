using System;
using Microsoft.EntityFrameworkCore;

namespace YourBrand.StoreFront.Authentication.Data;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
}
