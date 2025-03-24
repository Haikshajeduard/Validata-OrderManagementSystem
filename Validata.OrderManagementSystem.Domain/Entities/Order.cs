namespace Validata.OrderManagementSystem.Domain.Entities;

public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public long TotalPrice { get; set; }
    
    public List<OrderItem> OrderItems { get; set; }
    public Customer Customer { get; set; }
}