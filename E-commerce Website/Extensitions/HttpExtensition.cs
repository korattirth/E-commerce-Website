using E_commerce_Website.RequestHelpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_commerce_Website.Extensitions
{
    public static class HttpExtensition
    {
        public static void AddPaginationHeader(this HttpResponse responce, MetaData metaData)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            responce.Headers.Add("Pagination", JsonSerializer.Serialize(metaData, options));
            responce.Headers.Add("Access-Control-Expose-Headers", "Pagination");
         }
    }
}
