using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class BTSip_Carrier
    {
        // Properties
        public string Destination { get; set; }
        public decimal InvRate { get; set; }
        public int InvMin { get; set; }
        public decimal InvCharge { get; set; }

        public BTSip_Carrier(string destination, decimal invRate, int invMin, decimal invCharge)
        {
            Destination = destination;
            InvRate = InvRate;
            InvMin = InvMin;
            InvCharge = InvCharge;
        }
    }
}
