﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Services;

namespace Ordering.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
            ? configuration.GetConnectionString("OrderingConnectionStringDocker")
            : configuration.GetConnectionString("OrderingConnectionString");

        services.AddDbContext<OrderContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}