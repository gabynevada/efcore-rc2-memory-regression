// See https://aka.ms/new-console-template for more information

using EfCore8IAsyncEnumerable;
using Microsoft.EntityFrameworkCore;
using ProtoBuf;

var context = new MyContext();
await context.Database.MigrateAsync();

// Comment out the following line to seed the database
// await OrderTestRepository.SeedData(context);

var orderTests = context.OrderTests
    .Select(e => new OrderTestImportDto(e.OrderTestId, e.CreatedAt))
    .AsAsyncEnumerable();

var stream = File.Create("TestFile.gz");
var totalRecordsImported = 0;
await foreach (var dataItem in orderTests)
{
    Serializer.SerializeWithLengthPrefix(stream, dataItem, PrefixStyle.Base128, 1);
    totalRecordsImported++;
}

Console.WriteLine($"Imported {totalRecordsImported} records");

public record OrderTestImportDto(Guid OrderTestId, DateTime CreatedAt);

public class OrderTest
{
    public Guid OrderTestId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MyContext : DbContext
{
    public DbSet<OrderTest> OrderTests => Set<OrderTest>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = "Server=localhost;Database=Test;user id=sa;password=MyP@ssword;TrustServerCertificate=True;";
        options.UseSqlServer(connectionString, b => b.UseAzureSql());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderTest>().ToTable("OrderTest", "order");
        modelBuilder.Entity<OrderTest>().HasKey(x => x.OrderTestId);
    }
}
