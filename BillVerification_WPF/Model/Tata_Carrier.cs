using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
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

        public Tata_Carrier(string destination, string destId, string destArea, string startPeriod, string endPeriod, string origin, string classRating, string rateType,
            decimal rateByCall, decimal terminationFee, decimal transitFee, int nbCalls, decimal durationMinutesRounded, decimal amount, string currency)
        {
            Destination = destination;
            DestId = destId;
            DestArea = destArea;
            StartPeriod = startPeriod;
            EndPeriod = endPeriod;
            Origin = origin;
            ClassRating = classRating;
            RateType = rateType;
            RateByCall = rateByCall;
            TerminationFee = terminationFee;
            TransitFee = transitFee;
            NbCalls = nbCalls;
            DurationMinutesRounded = durationMinutesRounded;
            Amount = amount;
            Currency = currency;
        }
    }
}
