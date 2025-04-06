using TechNest.Application.Interfaces;
using TechNest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TechNest.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using TechNest.Infrastructure.Repositores;
using Microsoft.Extensions.DependencyInjection;

namespace TechNest.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Repositories and UnitOfWork
            services.AddScoped(typeof(IRepositores<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register AutoMapper with specific extension method
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register the PhysicalFileProvider
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            // Register other services
            services.AddSingleton<IImageManagmentService, ImageManagmentService>();

            return services.RegisterDbContext(configuration).PendingMigrations();
        }

        private static IServiceCollection PendingMigrations(this IServiceCollection services)
        {
            //Pending Migrations
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
                    throw;
                }
            }
            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            var connectionString = configuration.GetConnectionString("TechNest") ??
              throw new InvalidOperationException("Connection string 'TechNest' not found.");
            services.AddDbContext<ApplicationDbcontext>(options =>
                 options.UseSqlServer(connectionString));
            return services;
        }
    }


}
