using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TemplateApp.DAL;

/// <summary>
/// This is used for migrations design time and not when running the code
/// </summary>
public class WeatherDbContextContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
{
    public WeatherDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=(LocalDb)\\MSSQLLocalDB; Initial Catalog=WeatherDb; Integrated Security=True");

        return new WeatherDbContext(optionsBuilder.Options, null);
    }
}