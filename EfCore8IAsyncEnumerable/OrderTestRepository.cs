namespace EfCore8IAsyncEnumerable;

public static class OrderTestRepository
{
    public static async Task SeedData(MyContext context)
    {
        const int totalItemsToAdd = 5_000_000;
        const int batchSize = 10_000;
        context.ChangeTracker.AutoDetectChangesEnabled = false;
    
        for (var i = 0; i < totalItemsToAdd; i += batchSize)
        {
            var itemsToAdd = Enumerable.Range(i, batchSize).Select(
                _ => new OrderTest { OrderTestId = Guid.NewGuid(), CreatedAt = DateTime.Now }
            ).ToList();

            // Consider using a bulk insert library here instead of AddRangeAsync
            await context.OrderTests.AddRangeAsync(itemsToAdd);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear(); // Clears context to save memory
        }

        // Re-enable AutoDetectChanges
        context.ChangeTracker.AutoDetectChangesEnabled = true;
    }
}
