using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace BillVerification_WPF_TestProject
{
    [TestClass]
    public class MainUnitTests
    {
        [TestMethod]
        public void TestGetMatches()
        {
            BillVerification_WPF.Matching match = new BillVerification_WPF.Matching();

            // Create 2 datatables to test
            DataTable dt1 = CreateSampleSpringboardDatatable();
            DataTable dt2 = CreateSampleCarrierDatatable();

            DataTable dt_matched = new DataTable();
            DataTable dt_testMatched = new DataTable();

            // Match the 2 datatables
            dt_matched = match.GetMatched(dt1, dt2);
            dt_testMatched = CreateSampleMatchedDatatable();

            // Compare the datarow in each datatable for a match
            Assert.AreEqual(true, CompareDatatable(dt_matched, dt_testMatched));
        }

        /// <summary>
        /// Compare on Destination and Rate only instead of the whole row
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        static bool CompareDatatable(DataTable dt1, DataTable dt2)
        {
            bool flag = false;

            foreach (DataRow row1 in dt1.Rows)
            {
                foreach (DataRow row2 in dt2.Rows)
                {
                    var array1 = row1.ItemArray;
                    var array2 = row2.ItemArray;

                    if (array1.SequenceEqual(array2))
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        private DataTable CreateSampleSpringboardDatatable()
        {
            DataTable dt = new DataTable();

            // Create columns
            DataColumn dc1 = new DataColumn("Supplier", typeof(String));
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("Country", typeof(String));
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("Supplier Destination", typeof(String));
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("FX", typeof(String));
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn("Supplier Rate", typeof(String));
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn("Start", typeof(String));
            dt.Columns.Add(dc6);
            DataColumn dc7 = new DataColumn("End", typeof(String));
            dt.Columns.Add(dc7);
            DataColumn dc8 = new DataColumn("# Calls", typeof(String));
            dt.Columns.Add(dc8);
            DataColumn dc9 = new DataColumn("Minutes", typeof(String));
            dt.Columns.Add(dc9);
            DataColumn dc10 = new DataColumn("Costs", typeof(String));
            dt.Columns.Add(dc10);

            // Create 1 sample row of data
            DataRow dr = dt.NewRow();
            dr[0] = "MGI";
            dr[1] = "Algeria";
            dr[2] = "Algeria";
            dr[3] = "USD";
            dr[4] = "0.003";
            dr[5] = "01/01/2016";
            dr[6] = "02/01/2016";
            dr[7] = "5";
            dr[8] = "10";
            dr[9] = "0.50";
            dt.Rows.Add(dr);

            return dt;
        }

        private DataTable CreateSampleCarrierDatatable()
        {
            DataTable dt = new DataTable();

            // Create columns
            DataColumn dc1 = new DataColumn("Destination", typeof(String));
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("Valid from", typeof(String));
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("Valid to", typeof(String));
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("Inv Rate", typeof(String));
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn("Calls", typeof(String));
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn("Inv Min", typeof(String));
            dt.Columns.Add(dc6);
            DataColumn dc7 = new DataColumn("Inv Charge", typeof(String));
            dt.Columns.Add(dc7);

            // Create 1 sample row of data
            DataRow dr = dt.NewRow();
            dr[0] = "Algeria";
            dr[1] = "01/01/2016";
            dr[2] = "02/01/2016";
            dr[3] = "0.003";
            dr[4] = "5";
            dr[5] = "10";
            dr[6] = "0.50";
            dt.Rows.Add(dr);

            return dt;
        }

        private DataTable CreateSampleMatchedDatatable()
        {
            DataTable dt = new DataTable();

            // Create columns
            // Add columns
            DataColumn dc = new DataColumn("Destination", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Our Rate", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("Our Min", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("Our Cost", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("Destination (Carrier)", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Rate", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Min", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("Inv Charge", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("Diff - Minutes", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("% Diff - Minutes", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("Diff - Cost", typeof(Double));
            dt.Columns.Add(dc);
            dc = new DataColumn("% Diff Cost", typeof(String));
            dt.Columns.Add(dc);
            dc = new DataColumn("% of Total Cost", typeof(String));
            dt.Columns.Add(dc);

            // Create 1 row of data
            DataRow dr = dt.NewRow();
            dr[0] = "Algeria";
            dr[1] = "0.003";
            dr[2] = "10";
            dr[3] = "0.50";
            dr[4] = "Algeria";
            dr[5] = "0.003";
            dr[6] = "10";
            dr[7] = "0.50";
            dr[8] = "0";
            dr[9] = "0%";
            dr[10] = "0";
            dr[11] = "0%";

            dt.Rows.Add(dr);

            return dt;
        }
    }
}
