using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.RequestHelpers
{
    public class ProductParms : PaginationParms
    {
        public string OrderBy { get; set; }
        public string SearchTerm { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
    }
}
