using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Ports.DTOs;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Services
{
    public class SaleItemService: ISaleItemService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public SaleItemService(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<SaleItem> AddItemToSaleAsync(SaleItemDto saleItemDto)
        {
            var sale = await _saleRepository.GetByIdAsync(saleItemDto.SaleId);

            var product = await _productRepository.GetByIdAsync(saleItemDto.ProductId);
            if (product == null)
                throw new Exception("Produto não encontrado.");

            var saleItem = new SaleItem
            {
                ProductId = saleItemDto.ProductId,
                Quantity = saleItemDto.Quantity,
                UnitPrice = saleItemDto.UnitPrice,
                Discount = saleItemDto.Discount,
            };

            sale.SaleItems.Add(saleItem);
            sale.TotalAmount += saleItem.TotalAmount;

            await _saleRepository.UpdateAsync(sale);
            return saleItem;
        }

        public async Task<SaleItem> RemoveItemFromSaleAsync(Guid saleId, Guid itemId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null)
                throw new Exception("Venda não encontrada.");

            var item = sale.SaleItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new Exception("Item não encontrado.");

            sale.SaleItems.Remove(item);
            sale.TotalAmount -= item.TotalAmount;

            await _saleRepository.UpdateAsync(sale);
            return item;
        }

        public async Task<IEnumerable<SaleItem>> GetSaleItemsAsync(Guid saleId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null)
                throw new Exception("Venda não encontrada.");

            return sale.SaleItems;
        }
    }
}
