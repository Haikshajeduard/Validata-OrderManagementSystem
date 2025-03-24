using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.Interfaces;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Application.Models.Customers
{
    public class CustomerModel : ICustomMappings
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public long PostalCode { get; set; }

        public void CreatMappings(Profile configuration)
        {
            configuration.CreateMap<Customer, CustomerModel>();
        }
    }
}
