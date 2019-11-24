using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HordeFlow.Migrations.Catalog
{
    public partial class SqlServerCatalogMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Tenant");
        }
    }
}
