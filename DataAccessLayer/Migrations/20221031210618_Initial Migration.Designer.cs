﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vacation_Manager.Data;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(VacationManagerDbContext))]
    [Migration("20221031210618_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("VacationManager.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("VacationManager.ProjectTeam", b =>
                {
                    b.Property<int>("ProjectTeamId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("ProjectTeamId");

                    b.HasIndex(new[] { "ProjectId" }, "IX_ProjectTeams_ProjectId");

                    b.HasIndex(new[] { "TeamId" }, "IX_ProjectTeams_TeamId");

                    b.ToTable("ProjectTeams");
                });

            modelBuilder.Entity("VacationManager.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("RoleIdentifier")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("VacationManager.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("VacationManager.User", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.HasIndex(new[] { "RoleId" }, "IX_Users_RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VacationManager.UsersTeam", b =>
                {
                    b.Property<int>("UserTeamId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserTeamId");

                    b.HasIndex(new[] { "TeamId" }, "IX_UsersTeams_TeamId");

                    b.HasIndex(new[] { "UserId" }, "IX_UsersTeams_UserId");

                    b.ToTable("UsersTeams");
                });

            modelBuilder.Entity("VacationManager.Vacation", b =>
                {
                    b.Property<int>("VacationId")
                        .HasColumnType("int");

                    b.Property<bool>("Aprooved")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HalfDayVacation")
                        .HasColumnType("bit");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("VacationId");

                    b.HasIndex(new[] { "UserId" }, "IX_Vacations_UserId");

                    b.ToTable("Vacations");
                });

            modelBuilder.Entity("VacationManager.ProjectTeam", b =>
                {
                    b.HasOne("VacationManager.Project", "Project")
                        .WithMany("ProjectTeams")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK_ProjectTeams_Projects")
                        .IsRequired();

                    b.HasOne("VacationManager.Team", "Team")
                        .WithMany("ProjectTeams")
                        .HasForeignKey("TeamId")
                        .HasConstraintName("FK_ProjectTeams_Teams")
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("VacationManager.User", b =>
                {
                    b.HasOne("VacationManager.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_Users_Roles")
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("VacationManager.UsersTeam", b =>
                {
                    b.HasOne("VacationManager.Team", "Team")
                        .WithMany("UsersTeams")
                        .HasForeignKey("TeamId")
                        .HasConstraintName("FK_UsersTeams_Teams")
                        .IsRequired();

                    b.HasOne("VacationManager.User", "User")
                        .WithMany("UsersTeams")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UsersTeams_Users")
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VacationManager.Vacation", b =>
                {
                    b.HasOne("VacationManager.User", "User")
                        .WithMany("Vacations")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Vacations_Users")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VacationManager.Project", b =>
                {
                    b.Navigation("ProjectTeams");
                });

            modelBuilder.Entity("VacationManager.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("VacationManager.Team", b =>
                {
                    b.Navigation("ProjectTeams");

                    b.Navigation("UsersTeams");
                });

            modelBuilder.Entity("VacationManager.User", b =>
                {
                    b.Navigation("UsersTeams");

                    b.Navigation("Vacations");
                });
#pragma warning restore 612, 618
        }
    }
}