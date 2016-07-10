using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Models
{
    class BTSip_Carrier
    {
        // Properties
        public string Destination { get; set; }
        public decimal InvRate { get; set; }
        public int InvMin { get; set; }
        public decimal InvCharge { get; set; }
    }
}
