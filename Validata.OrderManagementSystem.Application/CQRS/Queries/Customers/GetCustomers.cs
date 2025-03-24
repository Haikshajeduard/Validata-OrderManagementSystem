using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.Models.Customers;
using Validata.OrderManagementSystem.Application.Models.Products;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Application.CQRS.Queries.Customers
{
    public class GetCustomers
    {
        public partial class GetCustomersQuery : IRequest<IEnumerable<CustomerModel>>
        {
        }

        public partial class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerModel>>
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ILogger<GetCustomersQueryHandler> _logger;

            public GetCustomersQueryHandler(UnitOfWork unitOfWork, IMapper mapper, ILogger<GetCustomersQueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<IEnumerable<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var customers = await _unitOfWork.Customers.GetAllAsync();
                    var customersModel = _mapper.Map<IEnumerable<CustomerModel>>(customers);
                    return customersModel;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw new ApplicationException(ex.Message);
                }
            }
        }
    }
}
