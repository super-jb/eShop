using AspnetRunBasics.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace AspnetRunBasics;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();
        SeedDatabase(host);
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    private static void SeedDatabase(IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        
        IServiceProvider services = scope.ServiceProvider;
        ILoggerFactory loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            AspnetRunContext aspnetRunContext = services.GetRequiredService<AspnetRunContext>();
            AspnetRunContextSeed.SeedAsync(aspnetRunContext, loggerFactory).Wait();
        }
        catch (Exception exception)
        {
            ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(exception, "An error occurred seeding the DB.");
        }
    }
}
