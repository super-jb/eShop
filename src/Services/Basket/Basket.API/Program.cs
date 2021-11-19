using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var redisConnectionString = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
    });

    builder.Services.AddScoped<IBasketRepository, BasketRepository>();

    // Grpc Configuration
    builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
        (x => x.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
    
    builder.Services.AddScoped<DiscountGrpcService>();


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services
        .AddHealthChecks()
        .AddRedis(redisConnectionString, "Redis Health", HealthStatus.Degraded);
}
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
