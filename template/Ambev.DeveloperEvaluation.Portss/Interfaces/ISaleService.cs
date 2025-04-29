using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Ports.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<Sale> GetSaleByIdAsync(Guid saleId);
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task CancelSaleAsync(Guid saleId);
        Task<Sale> UpdateAsync(Guid saleId,Sale sale);
        Task<bool> DeleteAsync(Guid saleId);
    }
}
