﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using i21Apis.Data;

namespace i21Apis.Migrations.Sqlite
{
    [DbContext(typeof(TenantSqliteDbContext))]
    partial class TenantSqliteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("i21Apis.Models.tblARCustomer", b =>
                {
                    b.Property<int>("intEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal?>("dblARBalance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal?>("dblCreditLimit")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("strAccountNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strCurrency")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strCustomerNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strTaxNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("intEntityId");

                    b.ToTable("tblARCustomer");
                });

            modelBuilder.Entity("i21Apis.Models.tblSMCompanyLocation", b =>
                {
                    b.Property<int>("intCompanyLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("strAddress")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strCity")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strCountry")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strEmail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strFax")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strLocationName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strLocationNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strLocationType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strPhone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strStateProvince")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strWebsite")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("strZipPostalCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("intCompanyLocationId");

                    b.ToTable("tblSMCompanyLocation");
                });
#pragma warning restore 612, 618
        }
    }
}
