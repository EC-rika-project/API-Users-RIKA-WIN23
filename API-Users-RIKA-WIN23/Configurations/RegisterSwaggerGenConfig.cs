using Microsoft.OpenApi.Models;

namespace API_Users_RIKA_WIN23.Configurations
{
    public static class RegisterSwaggerGenConfig
    {
        public static void RegisterSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web-API-Silicon", Version = "v1" });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Description = "API Key Authentication",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Query, // Can change to header here and add headerinfo for every httprequest in controller instead of putting apikey in a query.
                    Name = "key"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                },
                new string[] { }
            }
        });
            });
        }
    }
}
