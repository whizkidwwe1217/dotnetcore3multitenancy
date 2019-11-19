using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace i21Apis.Migrations.SqlServer
{
    public partial class SqlServerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblARCustomer",
                columns: table => new
                {
                    intEntityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblARCustomer");

            migrationBuilder.DropTable(
                name: "tblSMCompanyLocation");
        }
    }
}
