set DatabaseProvider=SqlServer
dotnet ef migrations add SqlServerMigration -o Migrations/SqlServer -c TenantDbContext

set DatabaseProvider=MySql
dotnet ef migrations add MySqlMigration -o Migrations/MySql -c TenantDbContext

set DatabaseProvider=Sqlite
dotnet ef migrations add SqliteMigration -o Migrations/Sqlite -c TenantDbContext

pause