﻿// <auto-generated />
using System;
using Backend.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221013073426_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Backend.Models.Code", b =>
                {
                    b.Property<int>("CodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodeId"), 1L, 1);

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodeId");

                    b.ToTable("Codes");
                });

            modelBuilder.Entity("Backend.Models.EnergyProduction", b =>
                {
                    b.Property<int>("EnergyProductionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnergyProductionId"), 1L, 1);

                    b.Property<string>("CurrentProduction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CurrentTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DailyProduction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PowerplantId")
                        .HasColumnType("int");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EnergyProductionId");

                    b.HasIndex("PowerplantId");

                    b.ToTable("EnergyProductions");
                });

            modelBuilder.Entity("Backend.Models.Powerplant", b =>
                {
                    b.Property<int>("PowerplantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PowerplantId"), 1L, 1);

                    b.Property<int>("ConnectionStatus")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PowerplantType")
                        .HasColumnType("int");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("PowerplantId");

                    b.HasIndex("UserId");

                    b.ToTable("Powerplants");
                });

            modelBuilder.Entity("Backend.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HashedPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.Models.EnergyProduction", b =>
                {
                    b.HasOne("Backend.Models.Powerplant", "Powerplant")
                        .WithMany("EnergyProductions")
                        .HasForeignKey("PowerplantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Powerplant");
                });

            modelBuilder.Entity("Backend.Models.Powerplant", b =>
                {
                    b.HasOne("Backend.Models.User", "User")
                        .WithMany("Powerplants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Models.Powerplant", b =>
                {
                    b.Navigation("EnergyProductions");
                });

            modelBuilder.Entity("Backend.Models.User", b =>
                {
                    b.Navigation("Powerplants");
                });
#pragma warning restore 612, 618
        }
    }
}
