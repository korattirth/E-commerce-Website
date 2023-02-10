using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace E_commerce_Website.Utils
{
    public class MyHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// Operation filter to add the requirement of the custom header
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                throw new Exception("Invalid operation");
            operation.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = "accept-language",
                Description = "pass the locale here: examples like => ar,ar-jo,en,en-gb",
                Required = false // set to false if this is optional,
            });; ;
        }

    }
}
