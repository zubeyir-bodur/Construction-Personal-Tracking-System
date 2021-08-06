using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Construction_Personal_Tracking_System.Deneme
{
    public partial class PersonelTakipDBContext : DbContext
    {
        public PersonelTakipDBContext()
        {
        }

        public PersonelTakipDBContext(DbContextOptions<PersonelTakipDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }
        public virtual DbSet<Personnel> Personnel { get; set; }
        public virtual DbSet<PersonnelType> PersonnelTypes { get; set; }
        public virtual DbSet<Tracking> Trackings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: Remove here the connection string and put it appsettings.json
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-A2IO9DQ;Initial Catalog=PersonelTakipDB;Integrated Security=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_100_CS_AI_KS_WS_SC_UTF8");

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.AreaId)
                    .ValueGeneratedNever()
                    .HasColumnName("area_id");

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("area_name")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("company_name")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(6, 3)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(6, 3)")
                    .HasColumnName("longitude");

                entity.Property(e => e.QrCode)
                    .HasColumnType("image")
                    .HasColumnName("qr_code");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Area_Company");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.CompanyId)
                    .ValueGeneratedNever()
                    .HasColumnName("company_id");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("company_name")
                    .UseCollation("Turkish_CI_AS");
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.ToTable("Leave");

                entity.Property(e => e.LeaveId)
                    .ValueGeneratedNever()
                    .HasColumnName("leave_id");

                entity.Property(e => e.LeaveEnd)
                    .HasColumnType("date")
                    .HasColumnName("leave_end");

                entity.Property(e => e.LeaveStart)
                    .HasColumnType("date")
                    .HasColumnName("leave_start");

                entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");

                entity.HasOne(d => d.Personnel)
                    .WithMany(p => p.Leaves)
                    .HasForeignKey(d => d.PersonnelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Leave_Personnel");
            });

            modelBuilder.Entity<Personnel>(entity =>
            {
                entity.HasIndex(e => e.UserName, "IX_Personel")
                    .IsUnique();

                entity.Property(e => e.PersonnelId)
                    .ValueGeneratedNever()
                    .HasColumnName("personnel_id");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("password")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.PersonnelName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("personnel_name")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.PersonnelSurname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("personnel_surname")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.PersonnelTypeId).HasColumnName("personnel_type_id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_name")
                    .UseCollation("Turkish_CI_AS");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Personnel)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Personnel_Company");

                entity.HasOne(d => d.PersonnelType)
                    .WithMany(p => p.Personnel)
                    .HasForeignKey(d => d.PersonnelTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Personnel_PersonnelType");
            });

            modelBuilder.Entity<PersonnelType>(entity =>
            {
                entity.ToTable("PersonnelType");

                entity.Property(e => e.PersonnelTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("personnel_type_id");

                entity.Property(e => e.PersonnelTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("personnel_type_name")
                    .UseCollation("Turkish_CI_AS");
            });

            modelBuilder.Entity<Tracking>(entity =>
            {
                entity.ToTable("Tracking");

                entity.Property(e => e.TrackingId)
                    .ValueGeneratedNever()
                    .HasColumnName("tracking_id");

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("area_name")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.AutoExit).HasColumnName("auto_exit");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("company_name")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.EntranceDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("entrance_date");

                entity.Property(e => e.ExitDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("exit_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");

                entity.Property(e => e.PersonnelType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("personnel_type")
                    .UseCollation("Turkish_CI_AS");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("surname")
                    .UseCollation("Turkish_CI_AS");

                entity.HasOne(d => d.Personnel)
                    .WithMany(p => p.Trackings)
                    .HasForeignKey(d => d.PersonnelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tracking_Personnel");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
