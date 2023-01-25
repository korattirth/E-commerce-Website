using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.DTOs
{
    public class BasketDTO
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public List<BasketItemDTO> Items { get; set; }
    }
}
