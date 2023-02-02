using E_commerce_Website.Data;
using E_commerce_Website.DTOs;
using E_commerce_Website.Entites;
using E_commerce_Website.Extensitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;

        public BasketController(StoreContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This endpoint retrives basket for login User
        /// </summary>
        /// <remarks> ****GET**** /api/Basket</remarks>
        /// <response code="200">If Basket retrives successfully</response>
        /// <response code="400">If Basket does not found</response>
        [HttpGet(Name ="Get Basket")]
        public async Task<ActionResult<BasketDTO>> GetBasket()
        {
            Basket basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) return BadRequest(new ProblemDetails { Title = "Product not found" });
            return basket.MapBasketToDto();
        }
        /// <summary>
        /// This endpoint post product into basket when we provide productId and quantity 
        /// </summary>
        /// <param name="productId">Required for finding Product</param>
        /// <param name="quantity">How many Product want's to add</param>
        /// <remarks> ****POST**** /api/Basket</remarks>
        /// <returns>IEnumerable of slugs</returns>
        /// <response code="200">If product added successfully</response>
        /// <response code="400">If ProductId or quantity parameter is missing</response>
        /// <response code="404">If product does't found in database</response>
        [HttpPost(Name = "Add Item To Basket")]
        public async Task<ActionResult<BasketDTO>> AddItemToBasket(int productId , int quantity)
        {
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) basket = CreateBasket();
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();
            basket.AddItem(product, quantity);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
        }
        /// <summary>
        /// This endpoint remove product into basket when we provide productId and quantity 
        /// </summary>
        /// <remarks> ****DELETE**** /api/Basket</remarks>
        /// <param name="productId">Required for finding Product</param>
        /// <param name="quantity">How many Product want's to remove</param>
        /// <response code="200">If product removed successfully</response>
        /// <response code="400">If ProductId or quantity parameter is missing</response>
        /// <response code="404">If product does't found in database</response>
        [HttpDelete(Name = "Remove Basket")]
        public async Task<ActionResult> RemoveBasket(int productId , int quantity)
        {
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) return NotFound();
            basket.RemoveItem(productId, quantity);
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem removeing item from the basket" });
        }

        private Basket CreateBasket()
        {
            var buyerId = User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId  = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }
            var Basket = new Basket{ BuyerId = buyerId};
            _context.Baskets.Add(Basket);
            return Basket;
        }

        private async Task<Basket> RetrieveBasket(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }
            return await _context.Baskets.Include(i => i.Item).ThenInclude(p => p.Product).FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }
        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }
    }
}
