using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Messaging.RabbitMQ;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly VendaEventsPublisher _vendaEventsPublisher;

        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository,
                           IBranchRepository branchRepository, ICustomerRepository customerRepository, VendaEventsPublisher vendaEventsPublisher)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _branchRepository = branchRepository;
            _customerRepository = customerRepository;
            _vendaEventsPublisher = vendaEventsPublisher;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {

            var customer = await _customerRepository.GetByIdAsync(sale.IdCustomer);
            if (customer == null)
                throw new Exception("Cliente não encontrado.");

            var branch = await _branchRepository.GetByIdAsync(sale.IdBranch);
            if (branch == null)
                throw new Exception("Filial não encontrada.");

            if (sale.SaleItems == null || !sale.SaleItems.Any())
                throw new Exception("A venda precisa ter pelo menos um item.");

            decimal totalAmount = 0;
            var processedItems = new List<SaleItem>();

            foreach (var item in sale.SaleItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Produto com ID {item.ProductId} não encontrado.");

                var saleItem = new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                 
                    
                };

                // Regras de negócio
                if (saleItem.Quantity < 4)//("A quantidade mínima para obter desconto é 4 itens");
                {
                    saleItem.Discount = 0;
                    
                }
                else if (saleItem.Quantity > 20)
                {
                    throw new Exception("Não é permitido vender mais de 20 unidades de um produto.");
                }
                else if (saleItem.Quantity <= 9)
                {
                    saleItem.Discount = 0.10m;
                }
                else if (saleItem.Quantity >= 10 || saleItem.Quantity <= 20)// de 10 a 20
                {
                    saleItem.Discount = 0.20m;
                }

                saleItem.TotalAmount = saleItem.Quantity * saleItem.UnitPrice * (1 - saleItem.Discount);
                totalAmount += saleItem.TotalAmount;

                processedItems.Add(saleItem);
            }

            sale.SaleItems = processedItems;
            sale.TotalAmount = totalAmount;
            sale.SaleDate = DateTime.UtcNow;


            try
            {
                await _saleRepository.AddAsync(sale);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner: " + ex.InnerException.Message);
            }
            //enviar para mensageria 
            var vendaCriadaEvent = new VendaCriadaEvent
            {
                SaleId = sale.Id,
                CustomerId = sale.IdCustomer,
                CustomerEmail = customer.Email,
                SaleDate = sale.SaleDate,
                TotalAmount = sale.TotalAmount
            };

            _vendaEventsPublisher.PublishVendaCriadaEvent(vendaCriadaEvent);

            return sale;
        }

      
        public async Task<Sale> GetSaleByIdAsync(Guid saleId)
        {
            return await _saleRepository.GetByIdAsync(saleId);
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task CancelSaleAsync(Guid saleId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale != null)
            {
                sale.IsCancelled = true;
                await _saleRepository.UpdateAsync(sale);
            }
            else
            {
                throw new Exception("Venda não encontrada.");
            }
        }


        public async Task<Sale> UpdateAsync(Guid saleId, Sale sale)
        {
            var existingSale = await _saleRepository.GetByIdAsync(saleId);
            if (existingSale == null)
            {
                throw new Exception("Venda não encontrada.");
            }
            existingSale.IdCustomer = sale.IdCustomer;
            existingSale.IdBranch = sale.IdBranch;
            existingSale.SaleDate = sale.SaleDate;
            existingSale.TotalAmount = sale.TotalAmount;
            existingSale.SaleItems = sale.SaleItems;

            await _saleRepository.UpdateAsync(existingSale);
            return existingSale;

        }

        public async Task<bool> DeleteAsync(Guid saleId)
        {
            try
            {
                var sale = await _saleRepository.GetByIdAsync(saleId);
                if (sale == null)
                {
                    return false;
                }

                await _saleRepository.DeleteAsync(sale.Id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}



