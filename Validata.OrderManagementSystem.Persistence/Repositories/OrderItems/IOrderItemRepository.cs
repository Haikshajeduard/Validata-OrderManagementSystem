using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Repositories.OrderItems
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetOrderItems(int orderId);
    }
}
