using ImmoDAL.Configurations;
using ImmoDAL.DAOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoDAL
{
    // Benoit
    public class ImmoContext : DbContext
    {
        public DbSet<UserDAO> Users { get; set; }
        public DbSet<ClientDAO> Clients { get; set; }
        public DbSet<ManagerDAO> Managers { get; set; }
        public DbSet<PropertyManagerDAO> PropertyManagers { get; set; }
        public DbSet<SalesManagerDAO> SalesManagers { get; set; }
        public DbSet<SuperAdminDAO> Admins { get; set; }
        public DbSet<PropertyDAO> Properties { get; set; }
        public DbSet<PhotoDAO> Photos { get; set; }
        public DbSet<VisitDAO> Visits { get; set; }

        public ImmoContext(DbContextOptions<ImmoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new ManagerConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyConfiguration());
            modelBuilder.ApplyConfiguration(new PhotoConfiguration());
            modelBuilder.ApplyConfiguration(new VisitConfiguration());
        }
    }
}
