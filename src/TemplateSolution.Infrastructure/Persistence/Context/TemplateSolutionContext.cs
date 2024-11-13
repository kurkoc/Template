using Microsoft.EntityFrameworkCore;

namespace TemplateSolution.Infrastructure.Persistence.Context;
public class TemplateSolutionContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateSolutionContext).Assembly);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>()
            .AreUnicode(false)
            .HaveMaxLength(100);

        base.ConfigureConventions(configurationBuilder);
    }
}
