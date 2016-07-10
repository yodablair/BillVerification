using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Models
{
    class Tata_Carrier
    {
        // Properties
        public string Destination { get; set; }
        public string DestId { get; set; }
        public string DestArea { get; set; }
        public string StartPeriod { get; set; }
        public string EndPeriod { get; set; }
        public string Origin { get; set; }
        public string ClassRating { get; set; }
        public string RateType { get; set; }
        public decimal RateByCall { get; set; }
        public decimal TerminationFee { get; set; }
        public decimal TransitFee { get; set; }
        public int NbCalls { get; set; }
        public decimal DurationMinutesRounded { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
