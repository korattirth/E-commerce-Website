using E_commerce_Website.DTOs;
using E_commerce_Website.Entites;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Utils
{
    public class LoginExample : IExamplesProvider<UserDTO>
    {
        public UserDTO GetExamples()
        {
            return new UserDTO()
            {
                Email = "testuser@test.com",
                Token = "abcdefg",
                //Basket = new BasketDTO
                //{
                //    PaymentIntentId = "123",
                //    ClientSecret = "abcdefgh",
                //    BuyerId = "123",
                //    Items = new List<BasketItemDTO>
                //    {
                //        new BasketItemDTO
                //        {
                //            Name = "Test",
                //            Price = 5000
                //        }
                //    }
                //}
            };
        }
    }
}
