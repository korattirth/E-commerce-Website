using E_commerce_Website.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace E_commerce_Website.Utils
{
    public class GetUserDetails : IExamplesProvider<UserDTO>
    {
        public UserDTO GetExamples()
        {
            return new UserDTO()
            {
                Email = "testuser@test.com",
                Token = "abcdefg",
                Basket = new BasketDTO
                {
                    PaymentIntentId = "123",
                    ClientSecret = "abcdefgh",
                    BuyerId = "123",
                    Items = new List<BasketItemDTO>
                    {
                        new BasketItemDTO
                        {
                            Name = "Test",
                            Price = 5000
                        }
                    }
                }
            };
        }
    }
}
