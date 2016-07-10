using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BillVerification_WPF
{
    public class Matching
    {
        private decimal _totalOurMin;
        private decimal _totalOurCost;
        private decimal _totalInvMin;
        private decimal _totalInvCharge;
        private decimal _totalDiffMinutes;
        private decimal _totalDiffCost;
        private int _matchingRowcount;

        public decimal TotalOurMin { get { return _totalOurMin; } }
        public decimal TotalOurCost { get { return _totalOurCost; } }
        public decimal TotalInvMin { get { return _totalInvMin; } }
        public decimal TotalInvCharge { get { return _totalInvCharge; } }
        public decimal TotalDiffMinutes { get { return _totalDiffMinutes; } }
        public decimal TotalDiffCost { get { return _totalDiffCost; } }
        public int MatchingRowCount { get { return _matchingRowcount; } }
        
        public DataTable GetMatched(DataTable dtMatch1, DataTable dtMatch2)
        {
            DataTable dt = new DataTable();

            // Add columns
            DataColumn dc = new DataColumn("Destination", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("Our Rate", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Our Min", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Our Cost", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Destination (Carrier)", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Rate", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Min", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Charge", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Diff - Minutes", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("% Diff - Minutes", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("Diff - Cost", typeof(decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("% Diff Cost", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("% of Total Cost", typeof(string));
            dt.Columns.Add(dc);

            var matchingRows = from sbData in dtMatch1.AsEnumerable()
                               join carrierData in dtMatch2.AsEnumerable()
                               on new
                               {
                                   Rate = sbData.Field<decimal?>("Supplier Rate"),
                                   //Destination = sbData.Field<string>("Supplier Destination")
                               }
                               equals new
                               {
                                   Rate = carrierData.Field<decimal?>("Inv Rate"),
                                   //Destination = carrierData.Field<string>("Carrier Destination")
                               }
                               where sbData.Field<string>("Supplier Destination").Contains(carrierData.Field<string>("Destination"))
                               //where sbData.Field<string>("Supplier Destination").Contains(carrierData.Field<string>("Carrier Destination"))
                               select new
                               {
                                   col_SBDestination = sbData.Field<string>("Supplier Destination"),
                                   col_SBOurRate = sbData.Field<decimal?>("Supplier Rate"),
                                   col_SBMinutes = sbData.Field<decimal?>("Minutes"),
                                   col_SBOurCost = sbData.Field<decimal?>("Costs"),
                                   col_CarrierDestination = carrierData.Field<string>("Destination"),// != null ? carrierData.Field<string>("Destination") : null,
                                   col_CarrierInvRate = carrierData.Field<decimal?>("Inv Rate"),// != null ? carrierData.Field<string>("Inv Rate") : null,
                                   col_CarrierInvMin = carrierData.Field<decimal?>("Inv Min"),// != null,// ? carrierData.Field<string>("Inv Min") : null,
                                   col_CarrierInvCharge = carrierData.Field<decimal?>("Inv Charge"),// != null ? carrierData.Field<string>("Inv Charge") : null
                               };

            // Vars for the monetary values
            decimal ourCost, ourRate, invRate, invCharge, diffMinutes, percentageDiffMinutes, diffCost, percentageDiffCost, ourMin, invMin;

            foreach (var row in matchingRows)
            {
                if (row != null)
                {
                    DataRow dr = dt.NewRow();
                    ourRate = Convert.ToDecimal(row.col_SBOurRate);
                    ourMin = Convert.ToDecimal(row.col_SBMinutes);
                    ourCost = Convert.ToDecimal(row.col_SBOurCost);
                    invRate = Convert.ToDecimal(row.col_CarrierInvRate);
                    invMin = Convert.ToInt16(row.col_CarrierInvMin);
                    invCharge = Convert.ToDecimal(row.col_CarrierInvCharge);
                    diffMinutes = ourMin - invMin;
                    percentageDiffMinutes = Math.Round((diffMinutes / ourMin) * 100, 0, MidpointRounding.AwayFromZero);
                    diffCost = ourCost - invCharge;
                    if (ourCost == 0) percentageDiffCost = 0; else percentageDiffCost = Math.Round((diffCost / ourCost) * 100, 0, MidpointRounding.AwayFromZero);

                    dr[0] = row.col_SBDestination;
                    dr[1] = Math.Round(ourRate, 4, MidpointRounding.AwayFromZero);
                    dr[2] = row.col_SBMinutes;
                    dr[3] = ourCost;
                    dr[4] = row.col_CarrierDestination;
                    dr[5] = Math.Round(invRate, 4, MidpointRounding.AwayFromZero);
                    dr[6] = invMin; //Math.Round(invMin, 2, MidpointRounding.AwayFromZero);
                    dr[7] = Math.Round(invCharge, 2, MidpointRounding.AwayFromZero);
                    dr[8] = Math.Round(diffMinutes, 2, MidpointRounding.AwayFromZero);// Diff - Minutes
                    dr[9] = percentageDiffMinutes.ToString() + "%"; // % Diff - Minutes
                    dr[10] = Math.Round(diffCost, 2, MidpointRounding.AwayFromZero);// Diff - Cost
                    dr[11] = percentageDiffCost.ToString() + "%"; // % Diff - Cost
                    dr[12] = // % of Total Cost 

                    // Sum for the totals
                    _totalOurMin += ourMin;
                    _totalOurCost += ourCost;
                    _totalInvMin += invMin;
                    _totalInvCharge += invCharge;
                    _totalDiffMinutes += diffMinutes;
                    _totalDiffCost += diffCost;
                    _matchingRowcount++;

                    dt.Rows.Add(dr);
                }
            } //for
            return dt;
        }

        public DataTable GetUnmatched(DataTable dtMatch1, DataTable dtMatch2)
        {
            DataTable dt = new DataTable();

            // Add columns
            DataColumn dcNoMatch = new DataColumn("Destination", typeof(String));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Our Rate", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Our Min", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Our Cost", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Destination (Carrier)", typeof(String));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Inv Rate", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Inv Min", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Inv Charge", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Diff - Minutes", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("% Diff - Minutes", typeof(String));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("Diff - Cost", typeof(Decimal));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("% Diff Cost", typeof(String));
            dt.Columns.Add(dcNoMatch);
            dcNoMatch = new DataColumn("% of Total Cost", typeof(String));
            dt.Columns.Add(dcNoMatch);

            var noMatchingRows = from sbData in dtMatch1.AsEnumerable()
                                 join carrierData in dtMatch2.AsEnumerable()
                                 on new
                                 {
                                     Destination = sbData.Field<string>("Supplier Destination"),
                                     Rate = sbData.Field<decimal?>("Supplier Rate"),
                                 }
                                 equals new
                                 {
                                     Destination = carrierData.Field<string>("Destination"),
                                     Rate = carrierData.Field<decimal?>("Inv Rate"),
                                 }
                                 into grpJoinSBCarrier
                                 from carrierData in grpJoinSBCarrier.DefaultIfEmpty()
                                 select new
                                 {
                                     col_SBDestination = (sbData != null ? sbData.Field<string>("Supplier Destination") : null),
                                     col_SBOurRate = (sbData != null ? sbData.Field<decimal>("Supplier Rate").ToString() : null),
                                     col_SBMinutes = (sbData != null ? sbData.Field<string>("Minutes") : null),
                                     col_SBOurCost = (sbData != null ? sbData.Field<string>("Costs") : null),
                                     col_CarrierDestination = (carrierData != null ? carrierData.Field<string>("Destination") : null),
                                     col_CarrierInvRate = (carrierData != null ? carrierData.Field<decimal>("Inv Rate").ToString() : null),
                                     col_CarrierInvMin = (carrierData != null ? carrierData.Field<string>("Inv Min") : null),
                                     col_CarrierInvCharge = (carrierData != null ? carrierData.Field<string>("Inv Charge") : null)
                                 };

            // Vars for the monetary values
            decimal ourMin, ourCost, ourRate, invRate, invMin, invCharge, diffMinutes, percentageDiffMinutes, diffCost, percentageDiffCost;

            foreach (var row in noMatchingRows)
            {
                if ((row != null) && (row.col_CarrierDestination == null))
                {
                    DataRow dr = dt.NewRow();
                    ourRate = Convert.ToDecimal(row.col_SBOurRate);
                    ourMin = Convert.ToDecimal(row.col_SBMinutes);
                    ourCost = Convert.ToDecimal(row.col_SBOurCost);
                    invRate = Convert.ToDecimal(row.col_CarrierInvRate);
                    invMin = Convert.ToDecimal(row.col_CarrierInvMin);
                    invCharge = Convert.ToDecimal(row.col_CarrierInvCharge);
                    diffMinutes = ourMin - invMin;
                    percentageDiffMinutes = Math.Round((diffMinutes / ourMin) * 100, 0, MidpointRounding.AwayFromZero);
                    diffCost = ourCost - invCharge;
                    if (ourCost == 0) percentageDiffCost = 0; else percentageDiffCost = Math.Round((diffCost / ourCost) * 100, 0, MidpointRounding.AwayFromZero);

                    dr[0] = row.col_SBDestination;
                    dr[1] = Math.Round(ourRate, 4, MidpointRounding.AwayFromZero); ;
                    dr[2] = row.col_SBMinutes;
                    dr[3] = ourCost;
                    dr[4] = row.col_CarrierDestination;
                    dr[5] = Math.Round(invRate, 4, MidpointRounding.AwayFromZero); ;
                    dr[6] = Math.Round(invMin, 2, MidpointRounding.AwayFromZero); ;
                    dr[7] = Math.Round(invCharge, 2, MidpointRounding.AwayFromZero); ;
                    dr[8] = Math.Round(diffMinutes, 2, MidpointRounding.AwayFromZero);//decimal.Round(diffMinutes, 2, MidpointRounding.AwayFromZero); // Diff - Minutes
                    dr[9] = percentageDiffMinutes.ToString(); // % Diff - Minutes
                    dr[10] = Math.Round(diffCost, 2, MidpointRounding.AwayFromZero);//decimal.Round(diffCost, 2, MidpointRounding.AwayFromZero); // Diff - Cost
                    dr[11] = percentageDiffCost.ToString(); // % Diff - Cost
                                                            //dr[12] = // % of Total Cost 

                    // Sum for the totals
                    _totalOurMin += ourMin;
                    _totalOurCost += ourCost;
                    _totalInvMin += invMin;
                    _totalInvCharge += invCharge;
                    _totalDiffMinutes += diffMinutes;
                    _totalDiffCost += diffCost;
                    _matchingRowcount++;

                    dt.Rows.Add(dr);
                }
            } //for
            return dt;
        }

        /// <summary>
        /// Remove duplicates by looking for the greatest "Diff - Cost" between grouped records
        /// and deleting the max
        /// </summary>
        /// <param name="dt1"></param>
        /// <returns></returns>
        public DataTable RemoveDuplicates(DataTable dt1)
        {
            DataTable dtNew = new DataTable();
            dtNew = dt1.Clone();

            // Remove the dups
            var uniqueRows = from matchedData in dt1.AsEnumerable()
                             group matchedData
                             by new
                             {
                                 Destination = matchedData.Field<string>("Destination"),
                                 OurRate = matchedData.Field<decimal>("Our Rate"),
                                 OurMin = matchedData.Field<decimal>("Our Min"),
                                 OurCost = matchedData.Field<decimal>("Our Cost"),
                                 DestinationCarrier = matchedData.Field<string>("Destination (Carrier)"),
                                 InvRate = matchedData.Field<decimal>("Inv Rate"),
                                 DiffCost = matchedData.Field<decimal>("Diff - Cost")
                             }
                             into groupBy
                             select new
                             {
                                Destination = groupBy.Key.Destination,
                                OurRate = groupBy.Key.OurRate,
                                OurMin = groupBy.Key.OurMin,
                                OurCost = groupBy.Key.OurCost,
                                DestinationCarrier = groupBy.Key.DestinationCarrier,
                                InvRate = groupBy.Key.InvRate,
                                //InvMin = groupBy.Min(r => r.Field<decimal>("Inv Min")),
                                //InvCharge = groupBy.Min(r => r.Field<decimal>("Inv Charge")),
                                //DiffMinutes = groupBy.Min(r => r.Field<decimal>("Diff - Minutes")),
                                //PercentDiffMinutes = groupBy.Min(r => r.Field<string>("% Diff - Minutes")),
                                DiffCost = groupBy.Key.DiffCost
                                 ////DiffCost = groupBy.Where(r => r.Field<decimal>("Diff - Cost") < 0),
                                 //PercentDiffCost = groupBy.Min(r => r.Field<String>("% Diff Cost"))
                             };

            foreach (var row in uniqueRows)
            {
                DataRow dr = dtNew.NewRow();
                dr[0] = row.Destination;
                dr[1] = row.OurRate;
                dr[2] = row.OurMin;
                dr[3] = row.OurCost;
                dr[4] = row.DestinationCarrier;
                dr[5] = row.InvRate;
                //dr[6] = row.InvMin;
                //dr[7] = row.InvCharge;
                //dr[8] = row.DiffMinutes;
                //dr[9] = row.PercentDiffMinutes;
                dr[10] = row.DiffCost;
                //dr[11] = row.PercentDiffCost;

                dtNew.Rows.Add(dr);
            }

            return dtNew;
        }

    }
}
