using AutoMapper;
using E_commerce_Website.DTOs;
using E_commerce_Website.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.RequestHelpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
        }
    }
}
