using E_commerce_Website.Data;
using E_commerce_Website.DTOs;
using E_commerce_Website.Entites;
using E_commerce_Website.Extensitions;
using E_commerce_Website.Services;
using E_commerce_Website.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManger;
        private readonly TokenService _tokenService;
        private readonly StoreContext _context;

        public AccountController(UserManager<User> userManger , TokenService tokenService , StoreContext context)
        {
            _userManger = userManger;
            _tokenService = tokenService;
            _context = context;
        }
        /// <remarks> ****POST**** /api/Account/login</remarks>
        [HttpPost("login",Name ="Post Login")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManger.FindByNameAsync(loginDTO.UserName);
            if (user == null || !await _userManger.CheckPasswordAsync(user, loginDTO.Password))
                return Unauthorized();
            var userBasket = await RetrieveBasket(loginDTO.UserName);
            var anonBasket = await RetrieveBasket(Request.Cookies["buyerId"]);
            if(anonBasket != null)
            {
                if(userBasket != null)
                {
                    _context.Baskets.Remove(userBasket);
                }
                anonBasket.BuyerId = user.UserName;
                Response.Cookies.Delete("buyerId");
                await _context.SaveChangesAsync();
            }
            return new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = anonBasket != null ? anonBasket.MapBasketToDto() : userBasket?.MapBasketToDto()
        };
        }
        /// <remarks> ****POST**** /api/Account/register</remarks>
        [HttpPost("register", Name = "Post Register")]
        public async Task<ActionResult> Ragister(RagisterDTO ragisterDTO)
        {
            var user = new User { UserName = ragisterDTO.UserName, Email = ragisterDTO.Email };
            var result = await _userManger.CreateAsync(user, ragisterDTO.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManger.AddToRoleAsync(user, "Member");
            return StatusCode(201);
        }
        /// <remarks> ****GET**** /api/Account/currentUser</remarks>
        [Authorize]
        [HttpGet("currentUser", Name = "Get Current User")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManger.FindByNameAsync(User.Identity.Name);
            var userBasket = await RetrieveBasket(User.Identity.Name);
            return new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = userBasket?.MapBasketToDto()
            };
        }
        /// <remarks> ****GET**** /api/Account/savedAddress</remarks>
        [Authorize]
        [HttpGet("savedAddress", Name = "Get Save Address")]
        public async Task<ActionResult<UserAddress>> GetSavedAddress()
        {
            return await _userManger.Users.Where(x => x.UserName == User.Identity.Name).Select(user => user.Address).FirstOrDefaultAsync();
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
    }
}
