using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
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

        public Sprinboard_Extract(string supplier, string country, string supplierDestination, string fx, decimal supplierRate,
            string startDate, string endDate, int calls, decimal minutes, decimal costs)
        {
            Supplier = supplier;
            Country = country;
            SupplierDestination = supplierDestination;
            FX = fx;
            SupplierRate = SupplierRate;
            StartDate = startDate;
            EndDate = endDate;
            Calls = calls;
            Minutes = minutes;
            Costs = costs;
        }
    }
}
