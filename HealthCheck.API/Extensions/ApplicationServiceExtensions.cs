using System.Reflection;

using HealthCheck.API.Authorization;
using HealthCheck.Core.Interfaces;
using HealthCheck.Core.Services;

using Microsoft.OpenApi.Models;

namespace HealthCheck.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ISessionUserService, SessionUserService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();


        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "HealthCheck API",
                Description = "Health Check API for Health Check application",
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
        });
        services.AddSwaggerDocument();

        return services;
    }
}
