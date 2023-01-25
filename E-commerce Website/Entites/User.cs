using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Entites
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
    }
}
