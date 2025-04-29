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
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<Branch> CreateBranchAsync(Branch branch)
        {
            var _branch = new Branch
            {
                Name = branch.Name, 
                Description = branch.Description,
                Document =branch.Document,
                CreatedAt = DateTime.UtcNow,
                Active = true,
            };

            await _branchRepository.AddAsync(_branch);
            return _branch;
        }

        public async Task<Branch> GetBranchByIdAsync(Guid branchId)
        {
            return await _branchRepository.GetByIdAsync(branchId);
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _branchRepository.GetAllAsync();
        }

        public async Task UpdateBranchAsync(Guid branchId, string name)
        {
            var branch = await _branchRepository.GetByIdAsync(branchId);
            if (branch != null)
            {
                branch.Name = name;
                //branch.Description = Description;
                await _branchRepository.UpdateAsync(branch);
            }
        }

        public async Task DeleteBranchAsync(Guid branchId)
        {
            var branch = await _branchRepository.GetByIdAsync(branchId);
            if (branch != null)
            {
                await _branchRepository.DeleteAsync(branchId);
            }
        }
    }
}
