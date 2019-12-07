@SETLOCAL ENABLEDELAYEDEXPANSION

@REM Use WMIC to retrieve date and time
@echo off
FOR /F "skip=1 tokens=1-6" %%A IN ('WMIC Path Win32_LocalTime Get Day^,Hour^,Minute^,Month^,Second^,Year /Format:table') DO (
    IF NOT "%%~F"=="" (
        SET /A SortDate = 10000 * %%F + 100 * %%D + %%A
        set YEAR=!SortDate:~0,4!
        set MON=!SortDate:~4,2!
        set DAY=!SortDate:~6,2!
        @REM Add 1000000 so as to force a prepended 0 if hours less than 10
        SET /A SortTime = 1000000 + 10000 * %%B + 100 * %%C + %%E
        set HOUR=!SortTime:~1,2!
        set MIN=!SortTime:~3,2!
        set SEC=!SortTime:~5,2!
    )
)

dotnet ef migrations add Database_Migration_SqlServer_Catalog_!YEAR!!MON!!DAY!!HOUR!!MIN! -o Migrations/Catalog/SqlServer -c SqlServerCatalogDbContext
dotnet ef migrations add Database_Migration_MySql_Catalog_!YEAR!!MON!!DAY!!HOUR!!MIN! -o Migrations/Catalog/MySql -c MySqlCatalogDbContext
dotnet ef migrations add Database_Migration_Sqlite_Catalog_!YEAR!!MON!!DAY!!HOUR!!MIN! -o Migrations/Catalog/Sqlite -c SqliteCatalogDbContext

dotnet ef migrations add Database_Migration_SqlServer_!YEAR!!MON!!DAY!!HOUR!!MIN! -o Migrations/SqlServer -c TenantSqlServerDbContext
dotnet ef migrations add Database_Migration_MySql_!YEAR!!MON!!DAY!!HOUR!!MIN! -o Migrations/MySql -c TenantMySqlDbContext
dotnet ef migrations add Database_Migration_Sqlite_!YEAR!!MON!!DAY!!HOUR!!MIN! -o Migrations/Sqlite -c TenantSqliteDbContext

pause