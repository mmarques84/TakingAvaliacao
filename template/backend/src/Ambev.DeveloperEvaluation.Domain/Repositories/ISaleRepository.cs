﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(Guid id);
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task<List<Sale>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}
