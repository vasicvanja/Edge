using CustomValidation.Impl;
using CustomValidation.Interface;
using Edge.Repositories;
using Edge.Repositories.Interfaces;
using Edge.Services;
using Edge.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Stripe.Checkout;

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

            // Password Encryption
            serviceCollection.AddScoped<IPasswordEncryptionService, PasswordEncryptionService>();

            // Stripe Integration
            serviceCollection.AddScoped<IStripeService, StripeService>();
            serviceCollection.AddScoped<SessionLineItemService>();

            // Validator
            serviceCollection.AddSingleton<IEmailValidator, EmailValidator>();

            // Users
            serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
            serviceCollection.AddScoped<IUsersService, UsersService>();

            // Roles
            serviceCollection.AddScoped<IRolesRepository, RolesRepository>();
            serviceCollection.AddScoped<IRolesService, RolesService>();
        }
    }
}
