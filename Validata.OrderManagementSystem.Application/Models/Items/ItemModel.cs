using AutoMapper;
using Validata.OrderManagementSystem.Application.Interfaces;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Application.Models.Items;

public class ItemModel : ICustomMappings
{
    public int Id { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public long TotalPrice { get; set; }
    public void CreatMappings(Profile configuration)
    {
        configuration.CreateMap<Item, ItemModel>()
            .ForMember(x=>x.Id, opt => opt.MapFrom(x=>x.Id))
            .ForMember(x=>x.Product, opt => opt.MapFrom(x=>x.Product.Name))
            .ForMember(x=>x.Quantity, opt => opt.MapFrom(x=>x.Quantity))
            .ForMember(x=>x.TotalPrice, opt => opt.MapFrom(x=>x.Quantity * x.Product.Price));
    }
}