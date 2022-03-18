using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoverCore.Abstractions.Settings;
using ClassPort.Domain.Entities.Settings;
using ClassPort.Infrastructure.Common.Seeder.Services;
using ClassPort.Infrastructure.Common.Settings.Services;
using ClassPort.Infrastructure.Persistence.DbContexts;

namespace ClassPort.Infrastructure.Common.Extensions;

public static class HostExtensions
{
    public static WebApplication RunSeeders(this WebApplication host, bool overrideSettings = false)
    {
        using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var seeder = serviceScope.ServiceProvider.GetService<ApplicationSeederService>();
            var settingsService = serviceScope.ServiceProvider.GetRequiredService<ISettingsService<ApplicationSettings>>();

            settingsService.LoadPersistedSettings().GetAwaiter().GetResult();

            var settings = settingsService.GetSettings();

            if (overrideSettings || settings is { SeedDataOnStartup: true }) seeder?.SeedAsync().GetAwaiter().GetResult();
        }

        return host;
    }

    public static WebApplication RunMigrations(this WebApplication host, bool overrideSettings = false)
    {
        using (var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            var settingsService = serviceScope.ServiceProvider.GetRequiredService<ISettingsService<ApplicationSettings>>();

            settingsService.LoadPersistedSettings().GetAwaiter().GetResult();

            var settings = settingsService.GetSettings();

            if (overrideSettings || settings is { ApplyMigrationsOnStartup: true }) context?.Database.Migrate();
        }

        return host;
    }
}