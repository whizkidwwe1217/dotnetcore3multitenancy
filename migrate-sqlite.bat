set DatabaseProvider=Sqlite
dotnet ef migrations add SqliteMigration -o Migrations/Sqlite -c TenantSqliteDbContext

pause