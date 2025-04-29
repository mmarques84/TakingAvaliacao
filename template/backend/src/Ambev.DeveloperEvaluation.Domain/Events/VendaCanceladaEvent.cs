using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class VendaCanceladaEvent
    {
        public Guid SaleId { get; set; }
        public string Reason { get; set; }
        public string CustomerEmail { get; set; }
    }
}
