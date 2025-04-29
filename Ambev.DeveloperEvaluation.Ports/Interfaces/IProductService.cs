using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Interfaces
{
    public interface IProductService
    {
        Task<Item> CreateProductAsync(Item product);
        Task<Item> GetProductByIdAsync(Guid productId);
        Task<IEnumerable<Item>> GetAllProductsAsync();
        Task UpdateProductAsync(Guid productId, string name, decimal price);
        Task DeleteProductAsync(Guid productId);
    }
}
