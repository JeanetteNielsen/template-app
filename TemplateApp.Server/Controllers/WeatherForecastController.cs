using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemplateApp.BLL.WeatherForcasts.Handlers;
using TemplateApp.BLL.WeatherForcasts.Model;

namespace TemplateApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(
        IMediator mediator,
        ILogger<WeatherForecastController> logger) : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<List<WeatherForecast>>> Get(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetWeatherForcastsRequest(), cancellationToken);
            return Ok(result);
        }
    }
}