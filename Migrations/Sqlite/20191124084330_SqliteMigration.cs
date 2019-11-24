﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HordeFlow.Migrations.Sqlite
{
    public partial class SqliteMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblARCustomer",
                columns: table => new
                {
                    intEntityId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    strCustomerNumber = table.Column<string>(nullable: true),
                    strType = table.Column<string>(nullable: true),
                    dblCreditLimit = table.Column<decimal>(nullable: true),
                    dblARBalance = table.Column<decimal>(nullable: true),
                    strAccountNumber = table.Column<string>(nullable: true),
                    strTaxNumber = table.Column<string>(nullable: true),
                    strCurrency = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ConcurrencyTimeStamp = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblARCustomer", x => x.intEntityId);
                });

            migrationBuilder.CreateTable(
                name: "tblSMCompanyLocation",
                columns: table => new
                {
                    intCompanyLocationId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    strLocationName = table.Column<string>(nullable: true),
                    strLocationNumber = table.Column<string>(nullable: true),
                    strLocationType = table.Column<string>(nullable: true),
                    strAddress = table.Column<string>(nullable: true),
                    strZipPostalCode = table.Column<string>(nullable: true),
                    strCity = table.Column<string>(nullable: true),
                    strStateProvince = table.Column<string>(nullable: true),
                    strCountry = table.Column<string>(nullable: true),
                    strPhone = table.Column<string>(nullable: true),
                    strFax = table.Column<string>(nullable: true),
                    strEmail = table.Column<string>(nullable: true),
                    strWebsite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSMCompanyLocation", x => x.intCompanyLocationId);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    DatabaseProvider = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(nullable: true),
                    ConcurrencyTimeStamp = table.Column<DateTime>(nullable: true),
                    IsDedicated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblARCustomer");

            migrationBuilder.DropTable(
                name: "tblSMCompanyLocation");

            migrationBuilder.DropTable(
                name: "Tenant");
        }
    }
}