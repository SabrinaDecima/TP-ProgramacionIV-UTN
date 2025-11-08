using Application.Abstraction;
using Application.Abstraction.ExternalServices;
using Domain.Entities;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services
            .AddDatabaseConfiguration(builder)
            .AddAuthenticationConfiguration(builder)
            .AddAuthorizationConfiguration()
            .AddRepositories()
            .AddExternalServices();
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddDbContext<GymDbContext>(options =>
         options.UseSqlServer(
             builder.Configuration.GetConnectionString("SqlServerConnection"),
             sqlOptions =>
             {
                 sqlOptions.EnableRetryOnFailure(
                     maxRetryCount: 5,
                     maxRetryDelay: TimeSpan.FromSeconds(10),
                     errorNumbersToAdd: null
                 );
             }));

    }

    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {
         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });

        return services;
    }

    public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(TypeRole.SuperAdministrador), policy => policy.RequireRole(nameof(TypeRole.SuperAdministrador)));
            options.AddPolicy(nameof(TypeRole.Administrador), policy => policy.RequireRole(nameof(TypeRole.Administrador)));
            options.AddPolicy(nameof(TypeRole.Socio), policy => policy.RequireRole(nameof(TypeRole.Socio)));
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGymClassRepository, GymClassRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        return services;
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}
