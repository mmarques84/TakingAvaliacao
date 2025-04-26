using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleItemRepository
    {
        Task<SaleItem> GetByIdAsync(Guid id);
        Task AddAsync(SaleItem saleItem);
        Task UpdateAsync(SaleItem saleItem);
        Task<List<SaleItem>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}
