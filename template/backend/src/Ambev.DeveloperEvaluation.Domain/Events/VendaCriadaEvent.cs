using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class VendaCriadaEvent
    {
        public Guid SaleId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
