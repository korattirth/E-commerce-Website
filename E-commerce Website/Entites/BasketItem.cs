﻿using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_Website.Entites
{
    [Table("BasketItems")]
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int BasketId { get; set; } 
        public Basket Basket { get; set; }
    }
}