using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoDAL.Configurations
{
    public class VisitConfiguration : IEntityTypeConfiguration<VisitDAO>
    {
        public void Configure(EntityTypeBuilder<VisitDAO> builder)
        {
            builder.ToTable("TBL_Visits").HasKey(v => v.VisitId);
            
            builder.Property(v => v.VisitId).HasColumnName("PK_Visit");
            builder.Property(v => v.VisitDate).IsRequired();
            builder.Property(v => v.ClientId).HasColumnName("FK_Client");
            builder.Property(v => v.SalesManagerId).HasColumnName("FK_SalesManager");
            builder.Property(v => v.PropertyId).HasColumnName("FK_Property");

            builder.HasOne(v => v.Client).WithMany(c => c.Visits).HasForeignKey(v => v.ClientId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(v => v.SalesManager).WithMany(s => s.Visits).HasForeignKey(v => v.SalesManagerId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(v => v.Property).WithMany(p => p.Visits).HasForeignKey(v => v.PropertyId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
