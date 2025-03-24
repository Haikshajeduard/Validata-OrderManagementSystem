using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Validata.OrderManagementSystem.Application.Infrastructure.AutoMapper;

namespace Validata.OrderManagementSystem.Configuration.Extensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        services = services.AddAutoMapper(new Assembly[] { typeof(MappingProfile).GetTypeInfo().Assembly });
        return services;
    }
}