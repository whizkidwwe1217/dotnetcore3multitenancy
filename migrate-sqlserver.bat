set DatabaseProvider=SqlServer
dotnet ef migrations add SqlServerMigration -o Migrations/SqlServer -c TenantDbContext

pause