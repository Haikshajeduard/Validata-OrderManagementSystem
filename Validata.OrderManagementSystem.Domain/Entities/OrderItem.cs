namespace Validata.OrderManagementSystem.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }

    public Order Order { get; set; }
    public Item Item { get; set; }
}