using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SimpleOnlineStore.Api.Domain.Entities;

namespace SimpleOnlineStore.Api.DataAccess.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                .IsRequired();

            builder.HasData
            (
                new User
                {
                    Id = 1,
                    Name = "Peyman"
                },
                new User
                {
                    Id = 2,
                    Name = "Pegah"
                },
                new User
                {
                    Id = 3,
                    Name = "Paria"
                },
                new User
                {
                    Id = 4,
                    Name = "Pardisan"
                }

            );
        }
    }


    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasIndex(p => p.Title).IsUnique();
        }
    }


}
