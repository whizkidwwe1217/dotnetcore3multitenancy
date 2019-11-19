set DatabaseProvider=SqlServer
dotnet ef migrations add SqlServerMigration -o Migrations/SqlServer -c TenantSqlServerDbContext

pause