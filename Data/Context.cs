using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Construction_Personal_Tracking_System.Models;

// Change the path later for organization
// This, along with the Migrations file should be the only Database file imo.
namespace Construction_Personal_Tracking_System.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}

        // TODO DBSets for all tables in the server
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<PersonnelType> PersonnelTypes { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // To Do
            // Declare one to one, one to many and many to many relations
            // To define foreign keys; the "ID"s that are not primary keys            
            // Then define ID's that are primary keys

            // PRIMARY KEYS
            modelBuilder.Entity<Area>().HasKey(a => a.area_id);
            modelBuilder.Entity<Personnel>().HasKey(p => p.personnel_id);
            modelBuilder.Entity<PersonnelType>().HasKey(pt => pt.personnel_type_id);
            modelBuilder.Entity<Company>().HasKey(c => c.company_id);
            modelBuilder.Entity<Tracking>().HasKey(t => t.tracking_id);
            modelBuilder.Entity<Leave>().HasKey(l => l.leave_id);

            // FOREIGN KEYS
            // TO DO


        }
    }
}