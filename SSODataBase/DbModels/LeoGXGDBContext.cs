using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SSODataBase.DbExtend;

namespace SSODataBase.DbModels
{
    public partial class LeoGXGDBContext : EFCoreAllContext
    {
        public LeoGXGDBContext(string conn) : base(conn)
        {
        }

        public LeoGXGDBContext(DbContextOptions<LeoGXGDBContext> options)
            : base(options.ToString())
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<StaffImport> StaffImport { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=47.116.75.154;Initial Catalog=LeoGXGDB;User=sa;Password=Pass1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserEncryption)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserPwd)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AttendDate5).HasMaxLength(256);

                entity.Property(e => e.CostCenterCode18).HasMaxLength(256);

                entity.Property(e => e.CostCenterName19).HasMaxLength(256);

                entity.Property(e => e.DduserId25)
                    .HasColumnName("DDUserID25")
                    .HasMaxLength(256);

                entity.Property(e => e.Email6).HasMaxLength(256);

                entity.Property(e => e.Erpid14)
                    .HasColumnName("ERPID14")
                    .HasMaxLength(256);

                entity.Property(e => e.HomeSequence16).HasMaxLength(256);

                entity.Property(e => e.IsActive17).HasMaxLength(256);

                entity.Property(e => e.IsNewUser20).HasMaxLength(256);

                entity.Property(e => e.IsSync23).HasMaxLength(256);

                entity.Property(e => e.IsTpmuser22)
                    .HasColumnName("IsTPMUser22")
                    .HasMaxLength(256);

                entity.Property(e => e.LastLoginTime12).HasMaxLength(256);

                entity.Property(e => e.LastSetPasswordDate13).HasMaxLength(256);

                entity.Property(e => e.Lcid15)
                    .HasColumnName("LCID15")
                    .HasMaxLength(256);

                entity.Property(e => e.LoginIp24)
                    .HasColumnName("LoginIP24")
                    .HasMaxLength(256);

                entity.Property(e => e.LoginName2).HasMaxLength(256);

                entity.Property(e => e.ModifiedDate21).HasMaxLength(256);

                entity.Property(e => e.Pwd9)
                    .HasColumnName("PWD9")
                    .HasMaxLength(256);

                entity.Property(e => e.Pwdtemp10)
                    .HasColumnName("PWDTemp10")
                    .HasMaxLength(256);

                entity.Property(e => e.RemindFormat11).HasMaxLength(256);

                entity.Property(e => e.StaffAlias4).HasMaxLength(256);

                entity.Property(e => e.StaffId0)
                    .HasColumnName("StaffID0")
                    .HasMaxLength(256);

                entity.Property(e => e.StaffName3).HasMaxLength(256);

                entity.Property(e => e.StaffNo1).HasMaxLength(256);

                entity.Property(e => e.Tel7).HasMaxLength(256);

                entity.Property(e => e.Title8).HasMaxLength(256);
            });

            modelBuilder.Entity<StaffImport>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LoginName0)
                    .HasColumnName("loginName0")
                    .HasMaxLength(256);

                entity.Property(e => e.Name1).HasMaxLength(256);

                entity.Property(e => e.StaffNo11).HasMaxLength(256);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
