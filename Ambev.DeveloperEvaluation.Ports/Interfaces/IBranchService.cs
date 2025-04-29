using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Ports.Interfaces
{
    public interface IBranchService
    {
        Task<Branch> CreateBranchAsync(Branch branch);
        Task<Branch> GetBranchByIdAsync(Guid branchId);
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task UpdateBranchAsync(Guid branchId, string name);
        Task DeleteBranchAsync(Guid branchId);
    }
}
