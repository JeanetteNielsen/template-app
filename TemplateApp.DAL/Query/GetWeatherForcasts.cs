using Microsoft.EntityFrameworkCore;

namespace TemplateApp.DAL.Query
{
    public interface IGetWeatherForcasts
    {
        public Task<List<WeatherForecastEntity>> Execute(CancellationToken cancellationToken);
    }

    public class GetWeatherForcasts(WeatherDbContext context) : IGetWeatherForcasts
    {
        public async Task<List<WeatherForecastEntity>> Execute(CancellationToken cancellationToken)
        {
            return await context.WeatherForecast.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}