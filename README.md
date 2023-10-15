# Minimal reproduction for memory regression in Ef Core RC2
Reference issue: https://github.com/dotnet/efcore/issues/32052

## Steps to reproduce
- Set up a SQL Server database connection string in the Program.cs file
- I could not reproduce the issue with a local database, it appears the issue surfaces with the latency of a remote database
- You can uncomment this line to seed the database:
```arm
    // await OrderTestRepository.SeedData(context);
```