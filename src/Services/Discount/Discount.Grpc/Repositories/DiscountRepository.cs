using Ardalis.GuardClauses;
using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = Guard.Against.Null(configuration, nameof(configuration));
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var npgsqlConnection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        npgsqlConnection.Open();

        var coupon = 
            await npgsqlConnection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE LOWER(ProductName) = @ProductName", new { ProductName = productName.ToLower().Trim() });

        if (coupon == null)
        {
            return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
        }

        return coupon;
    }
        
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var npgsqlConnection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        npgsqlConnection.Open();

        var affected =
            await npgsqlConnection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                    new { coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

        return affected != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var npgsqlConnection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        npgsqlConnection.Open();

        var affected = 
            await npgsqlConnection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                    new { coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

        return affected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var npgsqlConnection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        npgsqlConnection.Open();

        var affected = 
            await npgsqlConnection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

        return affected != 0;
    }
}
