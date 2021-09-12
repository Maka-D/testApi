﻿// <auto-generated />
using System;
using CarSales.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarSales.Repository.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarSales.Domain.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Int")
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(20)")
                        .HasColumnName("Brand");

                    b.Property<string>("CarNumber")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(15)")
                        .HasColumnName("CarNumber");

                    b.Property<int>("ClientId")
                        .HasColumnType("int")
                        .HasColumnName("ClientId");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("CreationDate");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("DateTime")
                        .HasColumnName("DeletedAt");

                    b.Property<DateTime>("FinishedSale")
                        .HasColumnType("DateTime")
                        .HasColumnName("FinishedSaleDate");

                    b.Property<bool>("IsSold")
                        .HasColumnType("bit")
                        .HasColumnName("IsSold");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(20)")
                        .HasColumnName("Model");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("ModifiedDate");

                    b.Property<int>("Price")
                        .HasColumnType("Int")
                        .HasColumnName("Price");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("ReleaseDate");

                    b.Property<DateTime>("StartedSale")
                        .HasColumnType("DateTime")
                        .HasColumnName("StartedSaleDate");

                    b.Property<string>("VinCode")
                        .IsRequired()
                        .HasColumnType("VARCHAR(17)")
                        .HasColumnName("VinCode");

                    b.HasKey("Id")
                        .HasName("pk_CarId");

                    b.HasIndex("ClientId");

                    b.HasIndex("VinCode")
                        .IsUnique();

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarSales.Domain.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Int")
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("Address");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("BirthDate");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("CreationDate");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("DateTime")
                        .HasColumnName("DeletedAt");

                    b.Property<string>("Email")
                        .HasColumnType("NVARCHAR(50)")
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .HasColumnType("NVARCHAR(20)")
                        .HasColumnName("FirstName");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(11)")
                        .HasColumnName("IdentityNumber");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("ModifiedDate");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(15)")
                        .HasColumnName("PhoneNumber");

                    b.Property<string>("SecondName")
                        .HasColumnType("NVARCHAR(30)")
                        .HasColumnName("SecondName");

                    b.HasKey("Id")
                        .HasName("pk_ClientId");

                    b.HasIndex("IdentityNumber")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CarSales.Domain.Models.Car", b =>
                {
                    b.HasOne("CarSales.Domain.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });
#pragma warning restore 612, 618
        }
    }
}
