namespace Validata.OrderManagementSystem.Domain.Entities;

public class Item : BaseEntity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public Product Product { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}