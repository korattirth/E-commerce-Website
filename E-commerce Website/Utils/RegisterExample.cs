using E_commerce_Website.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Utils
{
    public class RegisterExample : IExamplesProvider<RagisterDTO>
    {
        public RagisterDTO GetExamples()
        {
            return new RagisterDTO
            {
                Email = "test@test.com",
                Password = "1230",
                UserName = "test"
            };
        }
    }
    public class CreateOrderExample : IExamplesProvider<CreateOrderDTO>
    {
        public CreateOrderDTO GetExamples()
        {
            return new CreateOrderDTO
            {
                SaveAddress = false
            };
        }
    }
    //public class Ex : IExamplesProvider<LoginDTO>
    //{
    //    public LoginDTO GetExamples()
    //    {
    //        return new LoginDTO
    //        {
    //           UserName = "test",
    //           Password = "123"
    //        };
    //    }
    //}
}
