using MediatR;
using TemplateApp.BLL.WeatherForcasts.Mappers;
using TemplateApp.BLL.WeatherForcasts.Model;
using TemplateApp.DAL.Query;

namespace TemplateApp.BLL.WeatherForcasts.Handlers
{
    public class GetWeatherForcastsRequest : IRequest<List<WeatherForecast>>;

    public class GetWeatherForcastsRequestHandler(IGetWeatherForcasts getWeatherForcasts)
        : IRequestHandler<GetWeatherForcastsRequest, List<WeatherForecast>>
    {
        public async Task<List<WeatherForecast>> Handle(GetWeatherForcastsRequest request,
            CancellationToken cancellationToken)
        {
            return (await getWeatherForcasts.Execute(cancellationToken))
                .Select(x => x.MapFromEntity()).ToList();
        }
    }
}