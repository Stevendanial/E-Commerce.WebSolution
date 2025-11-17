using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entites.Basket_Module
{
    public class CustomerBasket
    {
        public string Id { get; set; }= default!;

        public ICollection<BasketItem> Item { get; set; } = [];

    }
}
