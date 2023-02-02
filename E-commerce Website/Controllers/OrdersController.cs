using E_commerce_Website.Data;
using E_commerce_Website.DTOs;
using E_commerce_Website.Entites.OrderAggregate;
using E_commerce_Website.Extensitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }
        /// <remarks> ****GET**** /api/Orders</remarks>
        [HttpGet(Name ="Get Orders")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            return await _context.Orders
                .ProjectOrderToOrderDTO()
                .Where(x => x.BuyerId == User.Identity.Name).ToListAsync();
        }
        /// <remarks> ****GET**** /api/Orders/id</remarks>
        [HttpGet("{id}",Name ="Get Order")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            return await _context.Orders.ProjectOrderToOrderDTO()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id).FirstOrDefaultAsync();
        }
        /// <remarks> ****POST**** /api/Orders</remarks>
        [HttpPost(Name ="Create Order")]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDTO orderDto)
        {
            var basket = await _context.Baskets.RetrieveBasketWithItems(User.Identity.Name).FirstOrDefaultAsync();
            if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });
            var items = new List<OrderItem>();
            foreach(var item in basket.Item)
            {
                var productItem = await _context.Products.FindAsync(item.ProductId);
                var itemOrdered = new ProductItemOrder
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };
                var orderItem = new OrderItem
                {
                    ItemOrder = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 10000 ? 0 : 500;
            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                PaymentIntentId = basket.PaymentIntentId
            };
            _context.Orders.Add(order);
            _context.Baskets.Remove(basket);
            if (orderDto.SaveAddress)
            {
                var user = await _context.Users.Include(a => a.Address).FirstOrDefaultAsync(c => c.UserName == User.Identity.Name);
                var address = new Entites.UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    State = orderDto.ShippingAddress.State,
                    Zip = orderDto.ShippingAddress.Zip,
                    Country = orderDto.ShippingAddress.Country
                };
                user.Address = address;
            }
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);

            return BadRequest("Problem creating order");

        }
    }
}
