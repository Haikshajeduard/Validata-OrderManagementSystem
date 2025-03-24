namespace Validata.OrderManagementSystem.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public long Price { get; set; }

    public List<Item> Items { get; set; }
}