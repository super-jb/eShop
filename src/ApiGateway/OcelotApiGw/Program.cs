using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json");
});

builder.Host.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

builder.Services
    .AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    app.MapGet("/", () => "Hello World!");
});

app.UseOcelot();

app.Run();
