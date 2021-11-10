using Discount.API.Extensions;
using Discount.API.Repositories;
using Npgsql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var postgresConnectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString");

    builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services
        .AddHealthChecks()
        .AddNpgSql(postgresConnectionString);
}

WebApplication app = builder.Build();

app.MigrateDatabase<Program>(); // create DB, Table & populate tbl

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();