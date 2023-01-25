using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Entites.OrderAggregate
{
    public enum OrderStatus
    {
        Pending,
        PaymentRecevied,
        PaymentFailed
    }
}
