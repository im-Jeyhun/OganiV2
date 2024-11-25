using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Database.Models;

namespace OganiWebApp.Database.Configurations
{
    public class RoleConfigurations : IEntityTypeConfiguration<Role>
    {
        private int _idCounter = 1;

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
               .ToTable("Roles");

            builder
                .HasData(
                    new Role
                    {
                        Id = _idCounter++,
                        Name = "admin",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Role
                    {
                        Id = _idCounter++,
                        Name = "user",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }

                );
        }
    }
}
