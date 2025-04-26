using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
