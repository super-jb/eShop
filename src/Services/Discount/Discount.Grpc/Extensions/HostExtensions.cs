using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryValue = retry.Value;

            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            IConfiguration configuration = services.GetRequiredService<IConfiguration>();
            ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating PostgreSql db");

                using NpgsqlConnection connection = new(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();

                using NpgsqlCommand command = new() { Connection = connection };

                // drop table if exists
                command.CommandText = "DROP TABLE IF EXISTS COUPON";
                command.ExecuteNonQuery();

                // create table
                command.CommandText =
                    "CREATE TABLE COUPON (" +
                        $" ID SERIAL PRIMARY KEY NOT NULL, " +
                        $" ProductName VARCHAR(24) NOT NULL, " +
                        $" Description TEXT, " +
                        $" Amount INT " +
                        $"); ";
                command.ExecuteNonQuery();

                // populate table
                command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();

                logger.LogInformation("Migrated PostgreSql db :)");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occurred Migrating Database");

                if (retryValue < 10)
                {
                    retryValue++;
                    Thread.Sleep(1000);
                    MigrateDatabase<TContext>(host, retryValue);
                }
            }

            return host;
        }
    }
}
