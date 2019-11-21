using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HordeFlow.Migrations.Catalog
{
    public partial class CatalogMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    DatabaseProvider = table.Column<string>(nullable: true)
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
