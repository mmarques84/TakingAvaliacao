using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Ports.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Interfaces
{
    public interface ISaleItemService
    {
        Task<SaleItem> AddItemToSaleAsync(SaleItemDto saleItemDto);
        Task<SaleItem> RemoveItemFromSaleAsync(Guid saleId, Guid itemId);
        Task<IEnumerable<SaleItem>> GetSaleItemsAsync(Guid saleId);
    }
}
