using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.RequestHelpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> items , int count , int pageNumber , int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public MetaData MetaData { get; set; }

        public static async Task<PagedList<T>> ToPageList(IQueryable<T> query , int pageNumber , int pageSize)
        {
            var count = await query.CountAsync();
            var item = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(item, count, pageNumber, pageSize);
        }
    }
}
