using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
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

        public IBasis_Carrier(string customer, string trunkGroup, string destination, string billingDate, string periodDate,
            int calls, decimal minutes, string currency, decimal charges, decimal rate, string product)
        {
            Customer = customer;
            TrunkGroup = trunkGroup;
            Destination = destination;
            BillingDate = billingDate;
            PeriodDate = periodDate;
            Calls = calls;
            Minutes = minutes;
            Currency = currency;
            Charges = charges;
            Rate = rate;
            Product = product;
        }
    }
}
