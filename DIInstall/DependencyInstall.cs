using Edge.Repositories;
using Edge.Repositories.Interfaces;
using Edge.Services;
using Edge.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DIInstall
{
    /// <summary>
    /// DependencyInstall class.
    /// </summary>
    public static class DependencyInstall
    {
        /// <summary>
        /// Installs application specific services.
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void InstallAppDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAuthService, AuthService>();
            serviceCollection.AddScoped<ICyclesRepository, CyclesRepository>();
            serviceCollection.AddScoped<ICyclesService, CyclesService>();
            serviceCollection.AddScoped<IArtworksRepository, ArtworksRepository>();
            serviceCollection.AddScoped<IArtworksService, ArtworksService>();
            serviceCollection.AddScoped<ISmtpSettingsService, SmtpSettingsService>();
            serviceCollection.AddScoped<ISmtpSettingsRepository, SmtpSettingsRepository>();
        }
    }
}
