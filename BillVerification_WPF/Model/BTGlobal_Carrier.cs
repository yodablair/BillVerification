using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class BTGlobal_Carrier
    {
        // Properties
        public string Destination { get; set; }
        public decimal Rate { get; set; }
        public int Minutes { get; set; }
        public decimal Costs { get; set; }

        public BTGlobal_Carrier(string destination, decimal rate, int minutes, decimal costs)
        {
            Destination = destination;
            Rate = rate;
            Minutes = minutes;
            Costs = costs;
        }

    }
}
