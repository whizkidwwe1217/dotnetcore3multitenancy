set DatabaseProvider=SqlServer
dotnet ef migrations add CatalogMigration -o Migrations/Catalog -c CatalogDbContext

set DatabaseProvider=SqlServer
dotnet ef migrations add SqlServerMigration -o Migrations/SqlServer -c TenantSqlServerDbContext

set DatabaseProvider=MySql
dotnet ef migrations add MySqlMigration -o Migrations/MySql -c TenantMySqlDbContext

set DatabaseProvider=Sqlite
dotnet ef migrations add SqliteMigration -o Migrations/Sqlite -c TenantSqliteDbContext

pause