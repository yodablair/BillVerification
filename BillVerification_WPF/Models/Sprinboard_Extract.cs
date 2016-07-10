using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Models
{
    class Sprinboard_Extract
    {
        // Properties
        public string Supplier { get; set; }
        public string Country { get; set; }
        public string SupplierDestination { get; set; }
        public string FX { get; set; }
        public decimal SupplierRate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Calls { get; set; }
        public decimal Minutes { get; set; }
        public decimal Costs { get; set; }
    }
}
