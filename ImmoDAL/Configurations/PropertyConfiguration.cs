using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoDAL.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<PropertyDAO>
    {
        public void Configure(EntityTypeBuilder<PropertyDAO> builder)
        {
            builder.ToTable("TBL_Properties").HasKey(a => a.PropertyId);
            builder.HasIndex(a => a.ReferenceCode).IsUnique();

            builder.Property(a => a.ClientSellerId).HasColumnName("FK_ClientSeller");
            builder.Property(a => a.PropertyManagerId).HasColumnName("FK_PropertyManager");

            builder.HasOne(a => a.ClientSeller).WithMany(c => c.Properties).HasForeignKey(a => a.ClientSellerId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.PropertyManager).WithMany(p => p.Properties).HasForeignKey(a => a.PropertyManagerId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(a => a.PropertyId).HasColumnName("PK_Property");
            builder.Property(a => a.ReferenceCode).IsRequired().HasMaxLength(9);
            builder.Property(a => a.Title).IsRequired().HasMaxLength(500);
            builder.Property(a => a.PostalCode).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Surface).IsRequired();
            builder.Property(a => a.NumberOfRooms).IsRequired();
            builder.Property(a => a.DesiredPrice).IsRequired();
            builder.Property(a => a.MinimumPrice).IsRequired();
            builder.Property(a => a.Status).IsRequired();
        }
    }
}
