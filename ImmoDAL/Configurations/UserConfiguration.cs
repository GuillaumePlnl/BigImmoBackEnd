using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoDAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDAO>
    {
        public void Configure(EntityTypeBuilder<UserDAO> builder)
        {
            builder.ToTable("TBL_Users").HasKey(u => u.UserId);
            builder.HasIndex(u => u.Mail).IsUnique();

            builder.Property(u => u.UserId).HasColumnName("PK_User");

            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(250);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(250);
            builder.Property(u => u.Mail).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(100);
        }
    }
}
