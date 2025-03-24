using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;

namespace Validata.OrderManagementSystem.Configuration.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default")));
        services.AddScoped<IApplicationDBContext, ApplicationDBContext>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<UnitOfWork>();
        
        return services;
    }
}