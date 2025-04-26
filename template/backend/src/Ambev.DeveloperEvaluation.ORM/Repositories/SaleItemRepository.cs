using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly DbContext _context;

        public SaleItemRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<SaleItem> GetByIdAsync(Guid id)
        {
            return await _context.Set<SaleItem>()
                                 .Include(si => si.Product)
                                 .FirstOrDefaultAsync(si => si.Id == id);
        }

        public async Task AddAsync(SaleItem saleItem)
        {
            await _context.Set<SaleItem>().AddAsync(saleItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SaleItem saleItem)
        {
            _context.Set<SaleItem>().Update(saleItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SaleItem>> GetAllAsync()
        {
            return await _context.Set<SaleItem>().ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var saleItem = await GetByIdAsync(id);
            if (saleItem != null)
            {
                _context.Set<SaleItem>().Remove(saleItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
