using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();
            builder.Property(t => t.Username).IsRequired().HasMaxLength(500);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(500);
            builder.Property(t => t.Password).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Role).IsRequired().HasMaxLength(20);

            builder.HasAlternateKey(t => t.Username).HasName("UniqueKey_Username");
            builder.HasAlternateKey(t => t.Email).HasName("UniqueKey_Email");
        }
    }
}
