using AutoMapper;
using Validata.OrderManagementSystem.Application.Interfaces;
using Validata.OrderManagementSystem.Application.Models.Items;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Application.Models.Orders;

public class OrderModel : ICustomMappings
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public List<ItemModel> Items { get; set; }
    public long TotalPrice { get; set; }

    public void CreatMappings(Profile configuration)
    {
        configuration.CreateMap<Order, OrderModel>()
            .ForMember(x => x.Items, opt => opt.MapFrom(x => x.OrderItems.Select(x=>x.Item).ToList()));
    }
}