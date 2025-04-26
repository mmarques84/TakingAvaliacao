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
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _context;

        public ProductRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Item> GetByIdAsync(Guid id)
        {
            return await _context.Set<Item>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Set<Item>().ToListAsync();
        }

        public async Task AddAsync(Item product)
        {
            await _context.Set<Item>().AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Item product)
        {
            _context.Set<Item>().Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Set<Item>().Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
