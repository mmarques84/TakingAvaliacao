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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Item> CreateProductAsync(Item item)
        {
            var product = new Item
            {
                Name = item.Name,
                UnitPrice = item.UnitPrice,
                Active=true,
                CreatedAt = DateTime.UtcNow,

            };

            await _productRepository.AddAsync(product);
            return product;
        }

        public async Task<Item> GetProductByIdAsync(Guid productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        public async Task<IEnumerable<Item>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task UpdateProductAsync(Guid productId, string name, decimal price)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                product.Name = name;
                product.UnitPrice = price;
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                await _productRepository.DeleteAsync(productId);
            }
        }

       
    }
}
