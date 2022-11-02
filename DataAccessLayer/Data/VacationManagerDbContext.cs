using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationManager;

namespace Vacation_Manager.Data
{
    public partial class VacationManagerDbContext : DbContext
    {
        public VacationManagerDbContext()
        {
        }

        public VacationManagerDbContext(DbContextOptions<VacationManagerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectTeam> ProjectTeams { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersTeam> UsersTeams { get; set; }
        public virtual DbSet<Vacation> Vacations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=VacationManager;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).UseIdentityColumn(1,1);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProjectTeam>(entity =>
            {
                entity.HasIndex(e => e.ProjectId, "IX_ProjectTeams_ProjectId");

                entity.HasIndex(e => e.TeamId, "IX_ProjectTeams_TeamId");

                entity.Property(e => e.ProjectTeamId).UseIdentityColumn(1, 1);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectTeams)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectTeams_Projects");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.ProjectTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectTeams_Teams");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).UseIdentityColumn(1, 1);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.TeamId).UseIdentityColumn(1, 1);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

                entity.Property(e => e.UserId).UseIdentityColumn(1, 1);

                entity.HasIndex(u => u.UserName).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UsersTeam>(entity =>
            {
                entity.HasKey(e => e.UserTeamId);

                entity.HasIndex(e => e.TeamId, "IX_UsersTeams_TeamId");

                entity.HasIndex(e => e.UserId, "IX_UsersTeams_UserId");

                entity.Property(e => e.UserTeamId).UseIdentityColumn(1, 1);

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
                entity.HasIndex(e => e.UserId, "IX_Vacations_UserId");

                entity.Property(e => e.VacationId).UseIdentityColumn(1, 1);

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
