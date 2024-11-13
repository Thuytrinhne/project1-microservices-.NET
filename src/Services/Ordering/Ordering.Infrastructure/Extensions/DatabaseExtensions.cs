using Microsoft.AspNetCore.Builder;
namespace Ordering.Infrastructure.Extensions
{
    public static  class DatabaseExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync(); // auto migration 
            //  await SeedAsync(context);
        }
        private static async Task SeedAsync(AppDbContext context)
        {
            await SeedCustomerAsync(context);
            await SeedProductAsync(context);
            await SeedOrderWithItemAsync(context);

        }
        private static async Task SeedCustomerAsync (AppDbContext context)
        {
            if(!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedProductAsync(AppDbContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedOrderWithItemAsync(AppDbContext context)
        {
            if (!await context.Orders.AnyAsync())
            {
                await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
                await context.SaveChangesAsync();
            }
        }
    }
}
