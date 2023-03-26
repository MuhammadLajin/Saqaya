﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryLayer.Context;

namespace saqaya.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230326221230_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DomainLayer.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("accessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("marketingConsent")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = "testKeyONE",
                            CreatedAt = new DateTime(2023, 3, 27, 0, 12, 29, 446, DateTimeKind.Local).AddTicks(9520),
                            IsDeleted = false,
                            email = "testUserOne",
                            firstName = "User",
                            lastName = "One",
                            marketingConsent = false
                        },
                        new
                        {
                            Id = "testKeyTWO",
                            CreatedAt = new DateTime(2023, 3, 27, 0, 12, 29, 450, DateTimeKind.Local).AddTicks(2416),
                            IsDeleted = false,
                            email = "testUserTWO",
                            firstName = "User",
                            lastName = "TWO",
                            marketingConsent = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
