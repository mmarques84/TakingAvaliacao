using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var _customer = new Customer
            {
                Name = customer.Name,
                Email = customer.Email,
                CreatedAt = DateTime.UtcNow,
                Active = true,
                BirthDate = customer.BirthDate,
                City = customer.City,
            };

            await _customerRepository.AddAsync(_customer);
            return _customer;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            return await _customerRepository.GetByIdAsync(customerId);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task UpdateCustomerAsync(Guid customerId, string name, string email)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer != null)
            {
                customer.Name = name;
                customer.Email = email;
                await _customerRepository.UpdateAsync(customer);
            }
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer != null)
            {
                await _customerRepository.DeleteAsync(customerId);
            }
        }
    }
}
