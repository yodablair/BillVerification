using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Models
{
    class IBasis_Carrier
    {
        // Properties
        public string Customer { get; set; }
        public string TrunkGroup { get; set; }
        public string Destination { get; set; }
        public string BillingDate { get; set; }
        public string PeriodDate { get; set; }
        public int Calls { get; set; }
        public decimal Minutes { get; set; }
        public string Currency { get; set; }
        public decimal Charges { get; set; }
        public decimal Rate { get; set; }
        public string Product { get; set; }
    }
}
