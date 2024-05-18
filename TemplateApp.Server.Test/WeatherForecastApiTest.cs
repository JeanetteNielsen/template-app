using FluentAssertions;
using TemplateApp.BLL.WeatherForcasts.Model;
using TemplateApp.DAL;
using Tools.Test.API;
using Tools.Test.AutoData;
using Xunit;

namespace TemplateApp.Server.Test
{
    public class WeatherForecastApiTest() : ApiTestBase<WeatherDbContext>("weatherforecast")
    {
        protected WeatherTestDb TestDb;

        protected override void SetupTestDb(WeatherDbContext dbContext)
        {
            TestDb = new WeatherTestDb(dbContext);
        }


        [Theory, AutoFakeData]
        public async Task GivenWeathers_WhenCallingGet_ThenWeatherForecastsAreReturned(
            List<WeatherForecastEntity> weather)
        {
            // Arrange
            TestDb.WithWeatherForcasts(weather);

            // Act
            var result = await GetAsyncAndDeserialize<List<WeatherForecast>>("");

            // Assert
            result.Count.Should().Be(3);
            foreach (var entity in weather)
            {
                var fromResult = result.FirstOrDefault(x => x.Id == entity.Id);
                fromResult.Should().NotBeNull("The weatherforcast should have been present in the result");
                AssertIsEqual(fromResult, entity);
            }
        }

        private static void AssertIsEqual(WeatherForecast weatherForecast, WeatherForecastEntity entity)
        {
            entity.Date.Should().Be(weatherForecast.Date);
            entity.Summary.Should().Be(weatherForecast.Summary);
            entity.TemperatureCelsius.Should().Be(weatherForecast.TemperatureC);
        }
    }
}