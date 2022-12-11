using System;
using Microsoft.EntityFrameworkCore;

namespace Site.Server.Authentication.Data;

public class UsersContext : DbContext
{
    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
}
