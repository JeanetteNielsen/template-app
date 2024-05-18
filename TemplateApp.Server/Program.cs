using Serilog;
using TemplateApp.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


try
{
    Log.Information("Starting Template app web api");

    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        startup.SeedData(services);
    }

    startup.Configure(app, app.Environment);
    app.MapControllers();
    app.MapFallbackToFile("/index.html");
    app.Run();
    Log.Information("Started Template app web api successfully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Template app web api was unable to start due to exceptions");
}
finally
{
    Log.CloseAndFlush();
}