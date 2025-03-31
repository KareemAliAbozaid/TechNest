using TechNest.Domain.Interface;
using TechNest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechNest.Infrastructure.Repositores;
using Microsoft.Extensions.DependencyInjection;
using Mapster;
using MapsterMapper;
using System.Reflection;
using TechNest.Application.Services;
using Microsoft.Extensions.FileProviders;

namespace TechNest.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Repositories and UnitOfWork
            services.AddScoped(typeof(IRepositores<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageManagmentService, ImageManagmentService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            // Register the DbContext with a SQL Server provider
            var connectionString = configuration.GetConnectionString("TechNest") ??
                throw new InvalidOperationException("Connection string 'TechNest' not found.");
            services.AddDbContext<ApplicationDbcontext>(options =>
                 options.UseSqlServer(connectionString));

            // Apply pending migrations at startup
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbcontext>();
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }

                }
                catch (Exception ex)
                {
                    // Optional: log or handle migration errors
                    // var logger = scope.ServiceProvider.GetRequiredService<ILogger<ServiceCollectionExtensions>>();
                    // logger.LogError(ex, "An error occurred while migrating the database.");
                    //throw;
                }
            }

            //Add mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }
    }
}
