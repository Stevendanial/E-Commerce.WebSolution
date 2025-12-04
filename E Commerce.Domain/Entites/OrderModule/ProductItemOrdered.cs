using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entites.OrderModule
{
    public class ProductItemOrdered
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; } = default!;

        public string PictureUrl { get; set; }= default!;


    }
}
