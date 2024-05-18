using Microsoft.EntityFrameworkCore;

namespace TemplateApp.DAL;

public class WeatherDataSeed
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static void Migrate(WeatherDbContext context)
    {
        context.Database.Migrate();
    }

    public static void SeedRandomData(WeatherDbContext context)
    {
        if (context.WeatherForecast.Any())
        {
            return;
        }

        var toSeed = Enumerable.Range(1, 5).Select(index => new WeatherForecastEntity()
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureCelsius = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        context.WeatherForecast.AddRange(toSeed);
        context.SaveChanges();
    }
}