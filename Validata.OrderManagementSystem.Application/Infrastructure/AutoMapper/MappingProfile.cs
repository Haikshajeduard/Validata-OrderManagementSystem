using System.Reflection;
using AutoMapper;

namespace Validata.OrderManagementSystem.Application.Infrastructure.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        LoadCustomMappings();
    }

    private void LoadCustomMappings()
    {
        var mapsFrom = MapperProfileHelper.LoadCustomMappings(Assembly.GetExecutingAssembly());

        foreach (var map in mapsFrom)
        {
            map.CreatMappings(this);
        }
    }
}