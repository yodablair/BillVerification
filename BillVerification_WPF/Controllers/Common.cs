using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BillVerification_WPF.Controllers
{
    class Common
    {
        /// <summary>
        /// Simply adds 2 datatables together
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static DataTable AddDatables(DataTable dt1, DataTable dt2)
        {
            DataTable dt = new DataTable();

            dt1.Merge(dt2);
            dt = dt1;

            return dt;
        }

        /// <summary>
        /// Sort the datatable by the given column name
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static DataTable SortDatable(DataTable dt, string columnName)
        {
            DataTable dtSort = new DataTable();
            if (dt.Rows.Count > 0)
            {
                //  Convert DataTable to DataView
                DataView dv = dt.DefaultView;
                //   Sort data
                dv.Sort = columnName;
                //   Convert back your sorted DataView to DataTable
                dt = dv.ToTable();
                dtSort = dt;
            }
            return dtSort;
        }

        /// <summary>
        /// Change the datatype of the DataColumn
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colName"></param>
        /// <param name="newType"></param>
        /// <returns></returns>
        public static bool ChangeColumnDataType(DataTable dt, string colName, Type newType)
        {
            if (dt.Columns.Contains(colName) == false)
                return false;

            DataColumn column = dt.Columns[colName];
            if ((column.DataType == newType) && (newType != typeof(decimal)))
                return true;

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newType);
                dt.Columns.Add(newcolumn);

                decimal cell;
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        cell = decimal.Parse(Convert.ChangeType(row[colName], newType).ToString());
                        row["temporary"] = Math.Round(cell, 4, MidpointRounding.AwayFromZero);
                        //Math.Round(ourRate, 4, MidpointRounding.AwayFromZero);
                    }
                    catch { }
                }
                newcolumn.SetOrdinal(column.Ordinal);
                dt.Columns.Remove(colName);
                newcolumn.ColumnName = colName;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        ///// <summary>
        ///// Create a new column by concatenating the values of 2 columns
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="newColName"></param>
        ///// <param name="colHeader1"></param>
        ///// <param name="colHeader2"></param>
        ///// <returns></returns>
        //public static DataTable CreateNewColumn(DataTable dt, string newColName, string colHeader1, string colHeader2, string colHeader3)
        //{
        //    DataTable dtNew = new DataTable();
        //    DataColumn newCol = new DataColumn(newColName, typeof(string));
        //    dt.Columns.Add(newCol);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        // For now determine whether the string "Mobile" is present
        //        if (dr[colHeader2].ToString().Contains("Mobile"))
        //        {
        //            if (dr[colHeader3] != null)
        //            {
        //                dr[newCol] = dr[colHeader1].ToString() + " Mobile " + dr[colHeader3].ToString();
        //            }
        //            else
        //                dr[newCol] = dr[colHeader1].ToString() + ' ' + "Mobile";
        //        }
        //        else
        //            dr[newCol] = dr[colHeader1].ToString();
        //    }

        //    return dt;
        //}
    }
}
