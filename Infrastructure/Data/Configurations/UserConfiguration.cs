﻿using ApplicationCore.Entities;
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
            builder.Property(t => t.Username).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(256);
            builder.Property(t => t.Password).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Role).IsRequired().HasMaxLength(20);
        }
    }
}
