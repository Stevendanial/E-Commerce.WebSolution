using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entites.OrderModule
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        PaymentFailed,
    }
}
