using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;

namespace Validata.OrderManagementSystem.Application.CQRS.Commands.Customers;

public class UpdateCustomer
{
    public partial class UpdateCustomerCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long PostalCode { get; set; }
    }
    public partial class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
    {
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;
        private readonly UnitOfWork _unitOfWork;

        public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommandHandler> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request is null)
                    throw new ArgumentNullException("Request cannot be null");
                
                var oldEntity = await _unitOfWork.Customers.GetByIdAsync(request.Id);
                if(oldEntity == null)
                    throw new Exception("Customer not found");
                
                oldEntity.Name = request.Name;
                oldEntity.Address = request.Address;
                oldEntity.PostalCode = request.PostalCode;
               
                var customer = _unitOfWork.Customers.Update(oldEntity);
                await _unitOfWork.SaveChangesAsync();
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