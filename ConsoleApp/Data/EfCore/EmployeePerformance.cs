using System;
using System.Collections.Generic;

namespace ConsoleApp.Data.EfCore
{
    public partial class EmployeePerformance
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public int SalesMade { get; set; }
    }
}
