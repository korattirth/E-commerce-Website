using E_commerce_Website.DTOs;
using E_commerce_Website.Entites.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace E_commerce_Website.Extensitions
{
    public static class OrderExtensition
    {
        public static IQueryable<OrderDTO> ProjectOrderToOrderDTO(this IQueryable<Order> query)
        {
            return query
               .Select(order => new OrderDTO
               {
                   Id = order.Id,
                   BuyerId = order.BuyerId,
                   OrderDate = order.OrderDate,
                   ShippingAddress = order.ShippingAddress,
                   DeliveryFee = order.DeliveryFee,
                   Subtotal = order.Subtotal,
                   OrderStatus = order.OrderStatus.ToString(),
                   Total = order.GetTotal(),
                   OrderItems = order.OrderItems.Select(item => new OrderItemDTO
                   {
                       ProductId = item.ItemOrder.ProductId,
                       Name = item.ItemOrder.Name,
                       PictureUrl = item.ItemOrder.PictureUrl,
                       Price = item.Price,
                       Quantity = item.Quantity
                   }).ToList()
               }).AsNoTracking();
        }
    }
}
