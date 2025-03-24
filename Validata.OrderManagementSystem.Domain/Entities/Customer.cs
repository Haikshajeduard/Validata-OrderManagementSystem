namespace Validata.OrderManagementSystem.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public long PostalCode { get; set; }

    public IList<Order> Orders { get; set; }
}