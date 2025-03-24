using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence.Repositories.OrderItems
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IApplicationDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItems(int orderId)
        {
            var orderItems = await _dbSet
                .Include(x => x.Item)
                .Where(x => x.OrderId == orderId)
                .ToListAsync();

            return orderItems;
        }
    }
}
