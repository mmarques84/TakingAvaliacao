using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Mapping;
using Ambev.DeveloperEvaluation.Ports.DTOs;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using AutoMapper;
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
        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository,
                           IBranchRepository branchRepository, ICustomerRepository customerRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _branchRepository = branchRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Sale> CreateSaleAsync(SaleDto sale)
        {

            var customer = await _customerRepository.GetByIdAsync(sale.CustomerId);
            if (customer == null)
                throw new Exception("Cliente não encontrado.");


            var branch = await _branchRepository.GetByIdAsync(sale.BranchId);
            if (branch == null)
                throw new Exception("Filial não encontrada.");


            if (sale.Items == null || !sale.Items.Any())
                throw new Exception("A venda precisa ter pelo menos um item.");
         
            decimal totalAmount = 0;
            foreach (var item in sale.Items)
            {
 
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Produto com ID {item.ProductId} não encontrado.");


                var saleItem = new SaleItemDto
                {
                    ProductId = product.Id,  
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount
                };

                if (saleItem.Quantity < 4)
                {
                    saleItem.Discount = 0; // Sem desconto para quantidades abaixo de 4
                }
                else if (saleItem.Quantity >= 4 && saleItem.Quantity <= 9)
                {
                    saleItem.Discount = 0.10m; // 10% de desconto para 4 a 9 itens
                }
                else if (saleItem.Quantity >= 10 && saleItem.Quantity <= 20)
                {
                    saleItem.Discount = 0.20m; // 20% de desconto para 10 a 20 itens
                }
                else
                {
                    throw new Exception("Não é permitido vender mais de 20 unidades de um produto.");
                }

                saleItem.TotalAmount = saleItem.Quantity * saleItem.UnitPrice * (1 - saleItem.Discount);
                totalAmount += saleItem.TotalAmount;

                // Adicionar o item da venda à venda
                sale.Items.Add(saleItem);
            }

            sale.TotalAmount = totalAmount;
            sale.SaleDate = DateTime.Now; // Definir a data da venda
            var salemap = _mapper.Map<Sale>(sale);
            // Salvar a venda no repositório
            await _saleRepository.AddAsync(salemap);

            // Retornar a venda criada
            return salemap;
        }

        private decimal CalculateItemTotal(SaleItem item)
        {
            return item.Quantity * item.UnitPrice * (1 - item.Discount);
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

       
    }
}

