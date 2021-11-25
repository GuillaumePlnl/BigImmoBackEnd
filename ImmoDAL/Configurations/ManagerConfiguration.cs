using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoDAL.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<ManagerDAO>
    {
        public void Configure(EntityTypeBuilder<ManagerDAO> builder)
        {
            builder.ToTable("TBL_Managers");

            builder.Property(m => m.CompanyName).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Siret).IsRequired().HasMaxLength(14);
        }
    }
}
