# Minimal reproduction for memory regression in Ef Core RC2
Reference issue: https://github.com/dotnet/efcore/issues/32052

## Steps to reproduce
- Set up a SQL Server database connection string in the Program.cs file
- I could not reproduce the issue with a local database, used an Azure SQL Managed Instance to reproduce.
- You can uncomment this line to seed the database:
```arm
    // await OrderTestRepository.SeedData(context);
```

## Update
- Managed to reproduce the issue with a local database by adding USerAzureSql() to the options builder.
```c#
options.UseSqlServer(connectionString, b => b.UseAzureSql());
```