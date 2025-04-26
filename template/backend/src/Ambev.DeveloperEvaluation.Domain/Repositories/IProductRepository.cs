using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Item> GetByIdAsync(Guid id);
        Task<List<Item>> GetAllAsync();
        Task AddAsync(Item product);
        Task UpdateAsync(Item product);
        Task DeleteAsync(Guid id);
    }
}
