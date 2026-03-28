using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YchPr.Model
{
    public class Sales
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

      
        
    }
}
