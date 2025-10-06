using Application.Abstraction;
using Application.Interfaces;
using Application.Services;
using Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGymClassService, GymClassService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
