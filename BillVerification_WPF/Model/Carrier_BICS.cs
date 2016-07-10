using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillVerification_WPF.Model
{
    class Carrier_BICS
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
        public decimal NumberOfCalls { get; set; }
        public decimal NumberOfUnits { get; set; }
        public decimal UnitTariff { get; set; }
        public string TariffValidityDate { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string AgreementNo { get; set; }
        public string AgreementName { get; set; }
        public string AgreementPeriod { get; set; }

        // Constructor
        public Carrier_BICS(string trafficStart, string trafficEnd, string productName, string timeDifference, string destOperatorCommercialName,
            string destinationCountry, string destinationCountryCode, string destinationOperator, string city, string description, string ibisCode,
            decimal numberOfCalls, decimal numberOfUnits, decimal unitTariff, string tariffValidityDate, string currency, decimal amount,
            string agreementNo, string agreementName, string agreementPeriod)
        {
            TrafficStart = trafficStart;
            TrafficEnd = trafficEnd;
            ProductName = productName;
            TimeDifference = timeDifference;
            DestOperatorCommercialName = destOperatorCommercialName;
            DestinationCountry = destinationCountry;
            DestinationCountryCode = destinationCountryCode;
            DestinationOperator = destinationOperator;
            City = city;
            Description = description;
            IBISCode = ibisCode;
            NumberOfCalls = numberOfCalls;
            NumberOfUnits = numberOfUnits;
            UnitTariff = UnitTariff;
            TariffValidityDate = tariffValidityDate;
            Currency = currency;
            Amount = amount;
            AgreementNo = agreementNo;
            AgreementName = agreementName;
            AgreementPeriod = agreementPeriod;
        }

    }
}
