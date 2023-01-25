using E_commerce_Website.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Extensitions
{
    public static class ProductExtensition
    {
        public static IQueryable<Product> sort(this IQueryable<Product> query, string orderBy)
        {
            if (String.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);
            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }
        public static IQueryable<Product> Search(this IQueryable<Product> query , string searchTerm)
        {
            if(String.IsNullOrWhiteSpace(searchTerm)) return query;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
        public static IQueryable<Product> Filter(this IQueryable<Product> query, string brand , string type)
        {
            var branList = new List<string>();
            var typeList = new List<string>();

            if (!string.IsNullOrEmpty(brand))
                branList.AddRange(brand.ToLower().Split(",").ToList());

            if (!string.IsNullOrEmpty(type))
                typeList.AddRange(type.ToLower().Split(",").ToList());

            query = query.Where(p => branList.Count == 0 || branList.Contains(p.Brand.ToLower()));
            query = query.Where(p => typeList.Count == 0 || typeList.Contains(p.Type.ToLower()));

            return query;
        }
    }
}
