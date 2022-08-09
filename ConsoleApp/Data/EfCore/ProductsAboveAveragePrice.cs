using System;
using System.Collections.Generic;

namespace ConsoleApp.Data.EfCore
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
