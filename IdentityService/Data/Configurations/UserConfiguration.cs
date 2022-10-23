
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourBrand.IdentityService.Models;

namespace YourBrand.IdentityService.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(name: "Users");

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                j => j
                    .HasOne(pt => pt.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(pt => pt.RoleId),
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserRoles)
                    .HasForeignKey(pt => pt.UserId));
    }
}
