using System;
using System.Collections.Generic;
using System.Text;

namespace BillVerification_WPF.Models
{
    class BICS_Carrier
    {
        // Properties
        public string TrafficStart { get; set; }
        public string TrafficEnd { get; set; }
        public string ProductName { get; set; }
        public string TimeDifference { get; set; }
        public string DestOperatorCommercialName { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationCountryCode { get; set; }
        public string DestinationOperator { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string IBISCode { get; set; }
        public int NumberOfCalls { get; set; }
        public decimal NumberOfUnits { get; set; }
        public decimal UnitTariff { get; set; }
        public string TariffValidityDate { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string AgreementNo { get; set; }
        public string AgreementName { get; set; }
        public string AgreementPeriod { get; set; }
    }
}
