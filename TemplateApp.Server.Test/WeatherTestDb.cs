using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateApp.DAL;
using TemplateApp.Server.Infrastructure;

namespace TemplateApp.Server.Test;

public class WeatherTestDb
{
    public WeatherDbContext Context { get; }

    public WeatherTestDb(WeatherDbContext context)
    {
        Context = context;
    }

    public WeatherTestDb WithWeatherForcast(WeatherForecastEntity entity)
    {
        Context.WeatherForecast.Add(entity);
        Context.SaveChanges();
        return this;
    }

    public WeatherTestDb WithWeatherForcasts(List<WeatherForecastEntity> entities)
    {
        Context.WeatherForecast.AddRange(entities);
        Context.SaveChanges();
        return this;
    }
}