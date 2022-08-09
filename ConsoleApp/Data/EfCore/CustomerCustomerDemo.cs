using System;
using System.Collections.Generic;

namespace ConsoleApp.Data.EfCore
{
    public partial class CustomerCustomerDemo
    {
        public string CustomerId { get; set; }
        public string CustomerTypeId { get; set; }

        public virtual Customers Customer { get; set; }
    }
}
