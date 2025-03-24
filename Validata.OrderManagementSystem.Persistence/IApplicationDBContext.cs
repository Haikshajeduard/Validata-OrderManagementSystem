using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence;

public interface IApplicationDBContext
{
    Task<int> SaveChangesAsync();
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
}