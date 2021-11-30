using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
        {
            new Order
            {
                UserName = "jb",
                FirstName = "John",
                LastName = "B",
                EmailAddress = "john.acb@gmail.com",
                AddressLine = "My Street 123, SF CA",
                State = "CA",
                ZipCode = "94117",
                Country = "USA",
                TotalPrice = 350,
                CardName = "JB",
                CardNumber = "1234567890123456",
                Expiration = "12/28",
                CVV = "123",
                PaymentMethod = 1
            }
        };
    }
}
