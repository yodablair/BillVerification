using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BillVerification_WPF
{
    public class Combine
    {
        public DataTable GroupByColumn_IBasis(DataTable dt1)
        {
            DataTable dt = new DataTable();

            // Add columns
            DataColumn dc = new DataColumn("Destination", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Billing", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Period", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Calls", typeof(Decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Min", typeof(Decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Currency", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Charge", typeof(Decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Rate", typeof(Decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Product", typeof(String));
            dt.Columns.Add(dc);

            // Group the common rows
            var groupRows = from carrier in dt1.AsEnumerable()
                            group carrier
                            by new
                            {
                                Destination = carrier.Field<String>("Destination"),
                                Rate = carrier.Field<String>("Inv Rate")
                            }
                            into groupBy
                            select new
                            {
                                Destination = groupBy.Key.Destination,
                                Rate = groupBy.Key.Rate,
                                Billing = groupBy.Max(r => r.Field<String>("Billing")),
                                Period = groupBy.Max(r => r.Field<String>("Period")),
                                Currency = groupBy.Max(r => r.Field<String>("Currency")),
                                Product = groupBy.Max(r => r.Field<String>("Product")),
                                Calls = groupBy.Sum(r => decimal.Parse(r.Field<string>("Calls"))),
                                Minutes = groupBy.Sum(r => decimal.Parse(r.Field<string>("Inv Min"))),
                                Charges = groupBy.Sum(r => decimal.Parse(r.Field<string>("Inv Charge")))
                            };

            foreach (var row in groupRows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = row.Destination;
                dr[1] = row.Billing;
                dr[2] = row.Period;
                dr[3] = row.Calls;
                dr[4] = row.Minutes;
                dr[5] = row.Currency;
                dr[6] = row.Charges;
                dr[7] = row.Rate;
                dr[8] = row.Product;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public DataTable GroupByColumn_BICS(DataTable dt1)
        {
            DataTable dt = new DataTable();

            // Add columns
            DataColumn dc = new DataColumn("Product Name", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Dest Operator Commercial Name", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Destination", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Destination Country Code", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Destination Operator", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("IBIS Code", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Calls", typeof(int));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Min", typeof(Decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Rate", typeof(Decimal));
            dt.Columns.Add(dc);
            dc = new DataColumn("Currency", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Charge", typeof(Decimal));
            dt.Columns.Add(dc);

            // Group the common rows
            var groupRows = from carrier in dt1.AsEnumerable()
                            group carrier
                            by new
                            {
                                Destination = carrier.Field<String>("Destination"),
                                Rate = decimal.Parse(carrier.Field<String>("Inv Rate")),
                                IbisCode = carrier.Field<String>("IBIS Code")
                            }
                            into groupBy
                            select new
                            {
                                ProductName = groupBy.Max(r => r.Field<String>("Product Name")),
                                DestOperatorCommercialName = groupBy.Max(r => r.Field<String>("Dest Operator Commercial Name")),
                                Destination = groupBy.Key.Destination,
                                DestCountryCode = groupBy.Max(r => r.Field<String>("Destination Country Code")),
                                DestOperator = groupBy.Max(r => r.Field<String>("Destination operator")),
                                IbisCode = groupBy.Key.IbisCode,
                                Calls = groupBy.Sum(r => decimal.Parse(r.Field<string>("Calls"))),
                                Minutes = groupBy.Sum(r => decimal.Parse(r.Field<string>("Inv Min"))),
                                Rate = groupBy.Key.Rate,
                                Currency = groupBy.Max(r => r.Field<String>("Currency")),
                                Charges = groupBy.Sum(r => decimal.Parse(r.Field<string>("Inv Charge")))
                            };

            foreach (var row in groupRows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = row.ProductName;
                dr[1] = row.DestOperatorCommercialName;
                dr[2] = row.Destination;
                dr[3] = row.DestCountryCode;
                dr[4] = row.DestOperator;
                dr[5] = row.IbisCode;
                dr[6] = row.Calls;
                dr[7] = row.Minutes;
                dr[8] = row.Rate;
                dr[9] = row.Currency;
                dr[10] = row.Charges;

                dt.Rows.Add(dr);
            }

            return dt;
        }

    }
}
