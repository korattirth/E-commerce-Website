using E_commerce_Website.DTOs;
using E_commerce_Website.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Extensitions
{
    public static class BasketExtensition
    {
        public static BasketDTO MapBasketToDto(this Basket basket)
        {
            return new BasketDTO
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                ClientSecret = basket.ClientSecret,
                PaymentIntentId = basket.PaymentIntentId,
                Items = basket.Item.Select(item => new BasketItemDTO
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query , string buyerId)
        {
            return query.Include(i => i.Item)
                .ThenInclude(p => p.Product).Where(b => b.BuyerId == buyerId);
        }
    }
}
