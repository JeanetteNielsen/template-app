using TemplateApp.BLL.WeatherForcasts.Model;
using TemplateApp.DAL;

namespace TemplateApp.BLL.WeatherForcasts.Mappers
{
    internal static class WeatherforcastExtension
    {
        public static WeatherForecast MapFromEntity(this WeatherForecastEntity entity)
        {
            return new WeatherForecast
            {
                Date = entity.Date,
                Summary = entity.Summary,
                TemperatureC = (int)entity.TemperatureCelsius,
                Id = entity.Id
            };
        }
    }
}