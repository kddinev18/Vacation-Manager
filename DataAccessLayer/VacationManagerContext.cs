using System;
using DataAccessLayer.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccessLayer
{
    public partial class VacationManagerContext : DbContext
    {
        public VacationManagerContext()
        {
        }

        public VacationManagerContext(DbContextOptions<VacationManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersTeam> UsersTeams { get; set; }
        public virtual DbSet<Vacation> Vacations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Vacation-Manager;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Teams_Projects");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(254);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasIndex(i => i.UserName).IsUnique(true);
                entity.HasIndex(i => i.Email).IsUnique(true);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UsersTeam>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.UserId })
                    .HasName("PK_UsersTeams_1");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.UsersTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersTeams_Teams");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersTeams)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersTeams_Users");
            });

            modelBuilder.Entity<Vacation>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vacations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacations_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
