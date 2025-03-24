using System.Reflection;
using Validata.OrderManagementSystem.Application.Interfaces;

namespace Validata.OrderManagementSystem.Application.Infrastructure.AutoMapper;

public class MapperProfileHelper
{
    public static IList<ICustomMappings> LoadCustomMappings(Assembly rootAssembly)
    {
        var types = rootAssembly.GetExportedTypes();

        var mapsFrom = (
            from type in types
            from instance in type.GetInterfaces()
            where
                typeof(ICustomMappings).IsAssignableFrom(type) &&
                !type.IsAbstract &&
                !type.IsInterface
            select (ICustomMappings)Activator.CreateInstance(type)
        ).ToList();

        return mapsFrom;
    }
}