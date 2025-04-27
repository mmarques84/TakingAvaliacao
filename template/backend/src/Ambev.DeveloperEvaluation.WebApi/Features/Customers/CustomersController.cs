using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomers;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            if (request == null)
                return BadRequest("Dados do cliente inválidos.");

            var CustomerMapper = _mapper.Map<Customer>(request);
            var customer = await _customerService.CreateCustomerAsync(CustomerMapper);
            var response = _mapper.Map<CreateCustomerResponse>(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            var response = _mapper.Map<CreateCustomerResponse>(customer);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var response = _mapper.Map<IEnumerable<CreateCustomerResponse>>(customers);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CreateCustomerRequest request)
        {
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                var errorResponse = new ErrorResponse(404, "Cliente não encontrado.");
                return NotFound(errorResponse);
            }

            // Atualiza os dados
            existingCustomer.Name = request.Name;
            existingCustomer.Email = request.Email;
            existingCustomer.City = request.City;
            existingCustomer.BirthDate = request.BirthDate;

            await _customerService.UpdateCustomerAsync(existingCustomer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
                return NotFound("Cliente não encontrado.");

            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}
