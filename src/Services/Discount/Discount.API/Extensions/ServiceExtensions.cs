using Npgsql;

namespace Discount.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection MigrateDatabase<TContext>(this IServiceCollection services, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgresql database...");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand { Connection = connection };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(
		                                    ID          SERIAL PRIMARY KEY  NOT NULL,
		                                    ProductName VARCHAR(24)         NOT NULL,
		                                    Description TEXT,
		                                    Amount      INT
	                                    )";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postgresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postgresql database.");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(services, retryForAvailability);
                    }
                }
            }

            return services;
        }
    }
}
