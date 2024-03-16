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
            // Authentication
            serviceCollection.AddTransient<IAuthService, AuthService>();

            // Cycles
            serviceCollection.AddScoped<ICyclesRepository, CyclesRepository>();
            serviceCollection.AddScoped<ICyclesService, CyclesService>();

            // Artworks
            serviceCollection.AddScoped<IArtworksRepository, ArtworksRepository>();
            serviceCollection.AddScoped<IArtworksService, ArtworksService>();
            // SmtpSettings
            serviceCollection.AddScoped<ISmtpSettingsService, SmtpSettingsService>();
            serviceCollection.AddScoped<ISmtpSettingsRepository, SmtpSettingsRepository>();

            // Email
            serviceCollection.AddScoped<IEmailService, EmailService>();
            serviceCollection.AddScoped<IContactMessagesRepository, ContactMessagesRepository>();
            serviceCollection.AddScoped<IContactMessagesService, ContactMessagesService>();
        }
    }
}
