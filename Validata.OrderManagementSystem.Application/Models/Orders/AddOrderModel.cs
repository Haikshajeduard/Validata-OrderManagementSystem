using Validata.OrderManagementSystem.Application.Models.Items;

namespace Validata.OrderManagementSystem.Application.Models.Orders;

public class AddOrderModel
{
    public int CustomerId { get; set; }
    public List<AddItemModel> Items { get; set; }
}