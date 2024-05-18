using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TemplateApp.BLL.WeatherForcasts.Handlers;
using TemplateApp.DAL;
using TemplateApp.DAL.Query;
using TemplateApp.Server.ScopedContext;
using TemplateApp.Shared;

namespace TemplateApp.Server.Infrastructure
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog();
            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IScopedContext, ScopedContextMock>();

            services.AddTransient<IGetWeatherForcasts, GetWeatherForcasts>();

            // Since the app is so small, there is no ref to BLL yet.
            // The following lines assures the assembly is loaded before the mediator is registered to allow for the handler to me registered.
            var t = typeof(GetWeatherForcastsRequestHandler);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            SetupWeatherDb(services);
        }

        protected virtual void SetupWeatherDb(IServiceCollection services)
        {
            services.AddDbContext<WeatherDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:WeatherDb"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();

            // TODO: setup Authentication

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public virtual void SeedData(IServiceProvider services)
        {
            var context = services.GetService<WeatherDbContext>();
            WeatherDataSeed.Migrate(context);
            WeatherDataSeed.SeedRandomData(context);
        }
    }

    // TODO: Teststartup.cs will nok work if moved to test project. Investigate how this can be corrected, since it does not belong here.
    public class TestStartup(IConfiguration configuration) : Startup(configuration)
    {
        protected override void SetupWeatherDb(IServiceCollection services)
        {
            services.AddDbContext<WeatherDbContext>(options =>
                options.UseInMemoryDatabase("TestingDB"));
        }
    }
}