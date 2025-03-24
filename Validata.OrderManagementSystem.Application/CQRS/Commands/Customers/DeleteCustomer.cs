using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;

namespace Validata.OrderManagementSystem.Application.CQRS.Commands.Customers;

public class DeleteCustomer
{
    public partial class DeleteCustomerCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long PostalCode { get; set; }
    }
    public partial class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;
        private readonly UnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(ILogger<DeleteCustomerCommandHandler> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request is null)
                    throw new ArgumentNullException("Request cannot be null");
                
                var oldEntity = await _unitOfWork.Customers.GetByIdAsync(request.Id);
                if(oldEntity == null)
                    throw new Exception("Customer not found");
               
                _unitOfWork.Customers.Remove(oldEntity);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Customer {oldEntity.Name} with id: {oldEntity.Id} deleted");
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