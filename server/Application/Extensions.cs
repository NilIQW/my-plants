using Application.Interfaces;
using Application.Interfaces.Infrastructure.Postgres;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Extensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IPlantService, PlantService>();
        services.AddScoped<IWebsocketSubscriptionService, WebsocketSubscriptionService>();
        return services;
    }
}