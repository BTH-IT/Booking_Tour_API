﻿// <auto-generated />
using System;
using Identity.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Identity.API.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    [Migration("20241106022015_AddAvatarToUser")]
    partial class AddAvatarToUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Identity.API.Entites.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Identity.API.Entites.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Identity.API.Entites.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RoleName")
                        .HasColumnType("longtext");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Identity.API.Entites.RoleDetail", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("PermissionId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<int>("ActionName")
                        .HasColumnType("int")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("RoleId", "PermissionId", "ActionName");

                    b.HasIndex("PermissionId");

                    b.ToTable("RoleDetails");
                });

            modelBuilder.Entity("Identity.API.Entites.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Identity.API.Entites.Account", b =>
                {
                    b.HasOne("Identity.API.Entites.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Identity.API.Entites.RoleDetail", b =>
                {
                    b.HasOne("Identity.API.Entites.Permission", "Permission")
                        .WithMany("RoleDetails")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Identity.API.Entites.Role", "Role")
                        .WithMany("RoleDetails")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Identity.API.Entites.User", b =>
                {
                    b.HasOne("Identity.API.Entites.Account", "Account")
                        .WithOne("User")
                        .HasForeignKey("Identity.API.Entites.User", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Identity.API.Entites.Account", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Identity.API.Entites.Permission", b =>
                {
                    b.Navigation("RoleDetails");
                });

            modelBuilder.Entity("Identity.API.Entites.Role", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("RoleDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
