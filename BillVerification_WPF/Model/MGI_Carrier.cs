using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class MGI_Carrier
    {
        // Properties
        public string Destination { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public decimal Rate { get; set; }
        public int Calls { get; set; }
        public decimal Minutes { get; set; }
        public string Currency { get; set; }
        public decimal TotalAmount { get; set; }

        public MGI_Carrier(string destination, string validFrom, string validTo, decimal rate, int calls, decimal minutes, string currency, decimal totalAmount)
        {
            Destination = destination;
            ValidFrom = ValidFrom;
            ValidTo = ValidTo;
            Rate = rate;
            Calls = calls;
            Minutes = minutes;
            Currency = currency;
            TotalAmount = totalAmount;
        }
    }
}
