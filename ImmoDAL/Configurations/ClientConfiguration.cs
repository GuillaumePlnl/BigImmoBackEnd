using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoDAL.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<ClientDAO>
    {
        public void Configure(EntityTypeBuilder<ClientDAO> builder)
        {
            builder.ToTable("TBL_Clients");

            builder.Property(c => c.SalesManagerId).HasColumnName("FK_SalesManager");

            builder.HasOne(c => c.SalesManager).WithMany(s => s.Clients)
                    .HasForeignKey(c => c.SalesManagerId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
