using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImmoDAL.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<SuperAdminDAO>
    {
        public void Configure(EntityTypeBuilder<SuperAdminDAO> builder)
        {
            builder.ToTable("TBL_Admins");
        }
    }
}
