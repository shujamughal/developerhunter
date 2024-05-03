﻿// <auto-generated />
using System;
using CompanyProfile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompanyProfile.Migrations
{
    [DbContext(typeof(CompanyContext))]
    [Migration("20240324210146_InitialCreate9")]
    partial class InitialCreate9
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CompanyProfile.Models.Company", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyDepartments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CompanyEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Department1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role3")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role4")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role5")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role6")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role7")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role8")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role9")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Salary1")
                        .HasColumnType("float");

                    b.Property<double>("Salary2")
                        .HasColumnType("float");

                    b.Property<double>("Salary3")
                        .HasColumnType("float");

                    b.Property<double>("Salary4")
                        .HasColumnType("float");

                    b.Property<double>("Salary5")
                        .HasColumnType("float");

                    b.Property<double>("Salary6")
                        .HasColumnType("float");

                    b.Property<double>("Salary7")
                        .HasColumnType("float");

                    b.Property<double>("Salary8")
                        .HasColumnType("float");

                    b.Property<double>("Salary9")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CompanyEmail")
                        .IsUnique();

                    b.ToTable("CompanyDepartments");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyInsights", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CompanyEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double?>("CustomerGrowthPercentage")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EstablishedSince")
                        .HasColumnType("datetime2");

                    b.Property<double?>("EstimatedRevenue")
                        .HasColumnType("float");

                    b.Property<int?>("OrdersCompleted")
                        .HasColumnType("int");

                    b.Property<int?>("ProductsSold")
                        .HasColumnType("int");

                    b.Property<int?>("SatisfiedCustomers")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyEmail")
                        .IsUnique()
                        .HasFilter("[CompanyEmail] IS NOT NULL");

                    b.ToTable("CompanyInsights");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bio")
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Facebook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkedIn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Logo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumberOfEmployees")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twitter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyEmail")
                        .IsUnique()
                        .HasFilter("[CompanyEmail] IS NOT NULL");

                    b.ToTable("CompanyProfiles");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CompanyEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyEmail");

                    b.ToTable("CompanyReviews");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyDepartments", b =>
                {
                    b.HasOne("CompanyProfile.Models.Company", "Company")
                        .WithOne("CompanyDepartments")
                        .HasForeignKey("CompanyProfile.Models.CompanyDepartments", "CompanyEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyInsights", b =>
                {
                    b.HasOne("CompanyProfile.Models.Company", "Company")
                        .WithOne("CompanyInsights")
                        .HasForeignKey("CompanyProfile.Models.CompanyInsights", "CompanyEmail")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyProfile", b =>
                {
                    b.HasOne("CompanyProfile.Models.Company", "Company")
                        .WithOne("CompanyProfile")
                        .HasForeignKey("CompanyProfile.Models.CompanyProfile", "CompanyEmail")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyProfile.Models.CompanyReview", b =>
                {
                    b.HasOne("CompanyProfile.Models.Company", "Company")
                        .WithMany("CompanyReviews")
                        .HasForeignKey("CompanyEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("CompanyProfile.Models.Company", b =>
                {
                    b.Navigation("CompanyDepartments");

                    b.Navigation("CompanyInsights")
                        .IsRequired();

                    b.Navigation("CompanyProfile")
                        .IsRequired();

                    b.Navigation("CompanyReviews");
                });
#pragma warning restore 612, 618
        }
    }
}
