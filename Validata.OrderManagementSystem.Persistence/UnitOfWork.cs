using System.Threading.Tasks;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence.Repositories;
using Validata.OrderManagementSystem.Persistence.Repositories.OrderItems;

namespace Validata.OrderManagementSystem.Persistence;

public class UnitOfWork
{
    private readonly IApplicationDBContext _context;
    public IRepository<Customer> Customers { get; }
    public IRepository<Order> Orders { get; }
    public IOrderItemRepository OrderItems { get; }
    public IRepository<Item> Items { get; }
    public IRepository<Product> Products { get; }
    
    public UnitOfWork(IApplicationDBContext context,
        IRepository<Product> products,
        IRepository<Order> orders,
        IRepository<Item> items,
        IOrderItemRepository orderItems,
        IRepository<Customer> customers)
    {
        _context = context;
        Products = products;
        Orders = orders;
        Items = items;
        OrderItems = orderItems;
        Customers = customers;
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}