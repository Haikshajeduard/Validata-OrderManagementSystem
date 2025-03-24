using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.CQRS.Commands.Customers;
using static Validata.OrderManagementSystem.Application.CQRS.Commands.Customers.CreateCustomer;

namespace Validata.OrderManagementSystem.Configuration.Extensions
{
    public static class MediatorExtensions
    {
        public static void RegisterMediator(this IServiceCollection services)
        {
            services.AddMediatR(x=> x.RegisterServicesFromAssembly(typeof(CreateCustomer.CreateCustomerCommand).GetTypeInfo().Assembly));

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}
