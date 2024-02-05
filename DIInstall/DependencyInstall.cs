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
            serviceCollection.AddTransient<ICyclesRepository, CyclesRepository>();
            serviceCollection.AddTransient<ICyclesService, CyclesService>();
            serviceCollection.AddTransient<IArtworksRepository, ArtworksRepository>();
            serviceCollection.AddTransient<IArtworksService, ArtworksService>();
        }
    }
}
