using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Models
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
    }
}
