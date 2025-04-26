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
    public class BranchRepository : IBranchRepository
    {
        private readonly DbContext _context;

        public BranchRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Branch> GetByIdAsync(Guid id)
        {
            return await _context.Set<Branch>().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            return await _context.Set<Branch>().ToListAsync();
        }

        public async Task AddAsync(Branch branch)
        {
            await _context.Set<Branch>().AddAsync(branch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Branch branch)
        {
            _context.Set<Branch>().Update(branch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var branch = await GetByIdAsync(id);
            if (branch != null)
            {
                _context.Set<Branch>().Remove(branch);
                await _context.SaveChangesAsync();
            }
        }
    }

}
