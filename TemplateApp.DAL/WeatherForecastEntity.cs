using System.ComponentModel.DataAnnotations;

namespace TemplateApp.DAL;

public class WeatherForecastEntity
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }

    public decimal TemperatureCelsius { get; set; }

    [MaxLength(256)] public string? Summary { get; set; }
    [MaxLength(20)] public ProbabilityEnum Probability { get; set; }

    public DateTime CreatedAt { get; set; }
}

public enum ProbabilityEnum
{
    High,
    Medium,
    Low
}