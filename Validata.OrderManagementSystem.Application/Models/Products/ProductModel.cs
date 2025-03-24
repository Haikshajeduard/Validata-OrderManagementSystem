using AutoMapper;
using Validata.OrderManagementSystem.Application.Interfaces;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Application.Models.Products;

public class ProductModel : ICustomMappings
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long Price { get; set; }

    public void CreatMappings(Profile configuration)
    {
        configuration.CreateMap<Product, ProductModel>();
    }
}