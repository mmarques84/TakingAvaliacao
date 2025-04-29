using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> CreateSaleAsync(Guid customerId, List<SaleItem> items);
        Task<Sale> GetSaleByIdAsync(Guid saleId);
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task CancelSaleAsync(Guid saleId);
    }
}
