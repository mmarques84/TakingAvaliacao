using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale: BaseEntity
    {
        public long SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }  
        public Guid IdCustomer { get; set; }
        public Customer Customer { get; set; }
        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public decimal TotalAmount { get; set; }
        public Guid IdBranch { get; set; }
        public Branch Branch { get; set; }
        public bool IsCancelled { get; set; }

    }
}
