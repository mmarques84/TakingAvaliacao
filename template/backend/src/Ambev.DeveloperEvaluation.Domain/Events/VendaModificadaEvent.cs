using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class VendaModificadaEvent
    {
        public Guid SaleId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Modifications { get; set; }
        public string CustomerEmail { get; set; }
    }
}
