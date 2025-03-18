using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Models
{
    public class ProductToOrder
    {
        public int Product_id { get; set; }
        public Product? Product { get; set; }

        public int Order_id { get; set; }
        public Order? Order { get; set; }
    }
}
