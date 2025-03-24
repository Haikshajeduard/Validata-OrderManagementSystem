using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Application.CQRS.Commands.Orders
{
    public class DeleteOrder
    {
        public partial class DeleteOrderCommand : IRequest<Unit>
        {
            public int Id { get; set; }
        }
        public partial class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
        {
            private readonly ILogger<DeleteOrderCommandHandler> _logger;
            private readonly UnitOfWork _unitOfWork;

            public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, UnitOfWork unitOfWork)
            {
                _logger = logger;
                _unitOfWork = unitOfWork;
            }
            public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request is null)
                        throw new ArgumentNullException("Request cannot be null");

                    var oldEntity = await _unitOfWork.Orders.GetByIdAsync(request.Id);
                    if (oldEntity == null)
                        throw new Exception("Order not found");

                    _unitOfWork.Orders.Delete(oldEntity);
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"Order with id: {oldEntity.Id} deleted");
                    return Unit.Value;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw new ApplicationException(e.Message);
                }
            }
        }
    }
}
