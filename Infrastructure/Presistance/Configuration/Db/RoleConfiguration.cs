using Domain.Entities.UsersEnities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Presistance.Configuration.Db
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            var seed = new List<Role>
            {
                new Role
                {
                    Id = Guid.Parse("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0001"),
                    Name = UserRoles.User.ToString(),
                    NormalizedName = UserRoles.User.ToString().ToUpperInvariant(),
                    ConcurrencyStamp = "ROLE-USER-0001"
                },
                new Role
                {
                    Id = Guid.Parse("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0002"),
                    Name = UserRoles.Broker.ToString(),
                    NormalizedName = UserRoles.Broker.ToString().ToUpperInvariant(),
                    ConcurrencyStamp = "ROLE-BROKER-0001"
                },
                new Role
                {
                    Id = Guid.Parse("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0003"),
                    Name = UserRoles.Admin.ToString(),
                    NormalizedName = UserRoles.Admin.ToString().ToUpperInvariant(),
                    ConcurrencyStamp = "ROLE-ADMIN-0001"
                },
                new Role
                {
                    Id = Guid.Parse("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0004"),
                    Name = UserRoles.SuperAdmin.ToString(),
                    NormalizedName = UserRoles.SuperAdmin.ToString().ToUpperInvariant(),
                    ConcurrencyStamp = "ROLE-SUPERADMIN-0001"
                },
            };

            builder.HasData(seed);
        }
    }
}