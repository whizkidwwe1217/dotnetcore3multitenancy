set DatabaseProvider=Db2
dotnet ef migrations add Db2Migration -o Migrations/Db2 -c TenantDb2DbContext

pause