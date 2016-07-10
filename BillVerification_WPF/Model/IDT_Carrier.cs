using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class IDT_Carrier
    {
        // Properties
        public string Destination { get; set; }
        public int BilledCalls { get; set; }
        public decimal SellMinutes { get; set; }
        public decimal SellRate { get; set; }
        public decimal SellPrice { get; set; }

        public IDT_Carrier(string destination, int billedCalls, decimal sellMinutes, decimal sellRate, decimal sellPrice)
        {
            Destination = destination;
            BilledCalls = billedCalls;
            SellMinutes = sellMinutes;
            SellRate = sellRate;
            SellPrice = sellPrice;
        }
    }
}
