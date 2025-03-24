using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;

namespace Validata.OrderManagementSystem.Application.CQRS.Commands.Customers;

public class CreateCustomer
{
    public partial class CreateCustomerCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public long PostalCode { get; set; }
    }
    public partial class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ILogger<CreateCustomerCommandHandler> _logger;
        private readonly UnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request is null)
                    throw new ArgumentNullException("Request cannot be null");
                
                var customerEntity = new Customer()
                {
                    Name = request.Name,
                    Address = request.Address,
                    PostalCode = request.PostalCode
                };
                var customer = await _unitOfWork.Customers.AddAsync(customerEntity);
                await _unitOfWork.SaveChangesAsync();
                return customer.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ApplicationException(e.Message);
            }
        }
    }
}