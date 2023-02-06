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
        ///<summary>This endpoint will create a new user, including their contacts, address book and entities. 
        ///This function can only be used by employees who have the rights and is not accessible from the outside.
        /// </summary>
        /// <remarks>Ragister Method</remarks>
        ///<response code="200">If the user registers successfully </response>
        ///<response code="400">If any Validation error arrive </response>
        [HttpPost("register", Name = "Post Register")]
        public async Task<ActionResult> Ragister(RagisterDTO ragisterDTO)
        {
            var user = new User { UserName = ragisterDTO.UserName, Email = ragisterDTO.Email };
            var result = await _userManger.CreateAsync(user, ragisterDTO.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManger.AddToRoleAsync(user, "Member");
            return StatusCode(201);
        }

        ///<summary>Registered users can login using this API described below.
        ///The login operation requires two properties: one marked as user identity and the second is password. 
        ///After login, user will get token value for that user.</summary>
        /// <remarks>Login Method</remarks>
        ///<response code="200">If the user login successfully </response>
        ///<response code="401">If user credentials are invalid </response>
        [HttpPost("login",Name ="Post Login")]
        //[ProducesResponseType(typeof(UserDTO), 200)]
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
        ///<summary>This endpoint retrieves the current login users' information (name, address, contacts, token , basket items and so on) 
        ///that matches the query condition.
        ///In addition to input parameters, you may also have to provide the access token indicating when you login.
        ///</summary>
        /// <remarks> ****GET**** /api/Account/currentUser</remarks>
        /// <param name="token">In order to get current user details, the user will need to provide their
        /// token through the header</param>
        ///<response code="200">If the user is autorize than it will return user details and basket </response>
        ///<response code="400">If user's token is not valid </response>
        [Authorize]
        [HttpGet("currentUser", Name = "Get Current User")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser([FromHeader(Name = "Authorization")] string token)
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
        //[Authorize]
        //[HttpGet("savedAddress", Name = "Get Save Address")]
        //public async Task<ActionResult<UserAddress>> GetSavedAddress()
        //{
        //    return await _userManger.Users.Where(x => x.UserName == User.Identity.Name).Select(user => user.Address).FirstOrDefaultAsync();
        //}
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
