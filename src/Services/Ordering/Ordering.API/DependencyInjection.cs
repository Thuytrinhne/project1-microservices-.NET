using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddCarter(); // mininal api ( thay thế cho cách sử dụng MVC để điịnh nghĩa và xử lý các endpoint )
            service.AddExceptionHandler<CustomExceptionHandler>();
            service.AddHealthChecks()
                   .AddSqlServer(configuration.GetConnectionString("DefaultConnectionString")!);
             
            return service;
        }
        public static WebApplication UseApiService (this WebApplication app)
        {
            // where we are configuring the HTTP lifecycle 
            app.MapCarter();
            app.UseExceptionHandler(options =>{ });
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            return app;
        }
    
    }
}
