set DatabaseProvider=MySql
dotnet ef migrations add MySqlMigration -o Migrations/MySql -c TenantMySqlDbContext

pause