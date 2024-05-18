using Microsoft.EntityFrameworkCore;
using TemplateApp.Shared;

namespace TemplateApp.DAL
{
    public class WeatherDbContext(DbContextOptions<WeatherDbContext> options, IScopedContext scopedContext)
        : DbContext(options)
    {
        public DbSet<WeatherForecastEntity> WeatherForecast { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecastEntity>()
                .Property(b => b.TemperatureCelsius).HasPrecision(5, 2);

            modelBuilder
                .Entity<WeatherForecastEntity>()
                .Property(d => d.Probability)
                .HasConversion<string>();


            modelBuilder.Entity<WeatherForecastEntity>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e is { Entity: IUpdateableEntity, State: EntityState.Added or EntityState.Modified });

            foreach (var entry in entries)
            {
                var updateableEntry = (IUpdateableEntity)entry;
                updateableEntry.UpdatedAt = DateTime.UtcNow;
                updateableEntry.UpdatedBy = scopedContext.UserId;
            }
        }
    }
}