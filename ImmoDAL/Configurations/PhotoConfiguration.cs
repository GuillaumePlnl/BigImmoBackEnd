using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoDAL.Configurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<PhotoDAO>
    {
        public void Configure(EntityTypeBuilder<PhotoDAO> builder)
        {
            builder.ToTable("TBL_Photos").HasKey(p => p.PhotoId);
            builder.Property(p => p.PhotoId).HasColumnName("PK_Photo");

            builder.Property(p => p.Image).IsRequired();
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);

            builder.Property(p => p.PropertyId).HasColumnName("FK_Property");

            builder.HasOne(p => p.Property).WithMany(prop => prop.Photos).HasForeignKey(p => p.PropertyId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
