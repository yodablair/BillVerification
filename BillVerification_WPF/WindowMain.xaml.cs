using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Data;

namespace BillVerification_WPF
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        #region Private vars

        private static string _carrierFile;
        private static string _carrierFile2;
        private static string _sbFile;
        private static DataTable _dtLiquidSB;
        private static DataTable _dtCarrier;
        private static DataTable _dtCarrier2;
        private static DataTable _dtCarrierCombined;
        private static DataTable _dtMatches;
        private static DataTable _dtNoMatches;
        private static int _matchedRowCount;
        private static int _unmatchedRowCount;
        private static decimal _totalPercentageDiffMin;
        private static decimal _totalPercentageDiffCost;
        private static decimal _totalInvCharge;
        private static decimal _totalInvMin;
        private static decimal _totalPercentageDiffMin2;
        private static decimal _totalPercentageDiffCost2;
        private static decimal _totalInvCharge2;
        private static decimal _totalInvMin2;

        #endregion

        private const string CARRIER_BICS = "BICS";
        private const string CARRIER_BTGLOBAL = "BTGlobal";
        private const string CARRIER_BTSIP = "BTSip";
        private const string CARRIER_IBASIS = "iBasis";
        private const string CARRIER_IDT = "IDT";
        private const string CARRIER_MGI = "MGI";
        private const string CARRIER_TATA = "Tata";

        public WindowMain()
        {
            InitializeComponent();
            HideControls(true);

            // Add items to the Carrier combobox
            this.cmbCarrierName.Items.Add(CARRIER_BICS);
            this.cmbCarrierName.Items.Add(CARRIER_BTGLOBAL);
            this.cmbCarrierName.Items.Add(CARRIER_BTSIP);
            this.cmbCarrierName.Items.Add(CARRIER_IBASIS);
            this.cmbCarrierName.Items.Add(CARRIER_IDT);
            this.cmbCarrierName.Items.Add(CARRIER_MGI);
            this.cmbCarrierName.Items.Add(CARRIER_TATA);
            this.cmbCarrierName.SelectedIndex = -1;
        }

        private void btnBrowseForSBFile_Click(object sender, RoutedEventArgs e)
        {
            this.lblMsg.Content = "";

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".xlsx";
            fileDialog.Filter = "Excel workbook|*.xlsx";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null)
            {
                HideControls(true);
                this.txtSBFile.Text = fileDialog.FileName;
                _sbFile = this.txtSBFile.Text;
            }
        }

        private void btnImportSB_Click(object sender, RoutedEventArgs e)
        {
            this.tabCtrlViewExcel.SelectedValue = this.tabItmLiquidData;

            try
            {
                if (this.txtSBFile.Text.Length == 0)
                {
                    MessageBox.Show("No file has been selected to import", "Import Springboard data");
                }
                else
                {
                    if (File.Exists(_sbFile))
                    {
                        // Load into datatable
                        ExcelReader.ExcelObject excel = new ExcelReader.ExcelObject();

                        using (Stream stream = File.OpenRead(_sbFile))
                        {
                            _dtLiquidSB = excel.ReadExcelSpringboardData(stream);
                        }

                        if (_dtLiquidSB != null)
                        {
                            // Change column datatypes for "Inv Rate" to 4 decimal places
                            bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtLiquidSB, "Supplier Rate", typeof(decimal));

                            // Change column datatype for "Calls" to decimal
                            x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtLiquidSB, "# Calls", typeof(decimal));

                            // Change column datatype for "Minutes" to decimal
                            x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtLiquidSB, "Minutes", typeof(decimal));

                            // Change column datatype for "Costs" to decimal
                            x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtLiquidSB, "Costs", typeof(decimal));

                            this.dgrdLiquidData.ItemsSource = _dtLiquidSB.DefaultView;
                            this.lblMsg.Content = "Springboard extract imported. " + _dtLiquidSB.Rows.Count.ToString() + " records imported";
                            MessageBox.Show("Springboard extract imported. " + _dtLiquidSB.Rows.Count.ToString() + " records imported", "Import Springboard data");
                        }
                        else
                        {
                            this.lblMsg.Content = "Unable to read excel file";
                            MessageBox.Show("Unable to read excel file", "Import Springboard data");
                        }
                    }
                    else
                    {
                        this.lblMsg.Content = "File does not exist";
                        MessageBox.Show("File: " + _sbFile + " does not exist", "Import Springboard data");
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblMsg.Content = ex.Message;
                MessageBox.Show(ex.Message, "Import Springboard data");
            }

        }

        private void btnImportCarrierData_Click(object sender, RoutedEventArgs e)
        {
            this.tabCtrlViewExcel.SelectedValue = this.tabItmCarrierData;

            try
            {
                if (this.cmbCarrierName.SelectedIndex == -1)
                {
                    // Nothing selected
                    MessageBox.Show("A Carrier must be selected", "Import Carrier data");
                }
                else
                {
                    if (this.txtCarrierDataFile.Text.Length == 0)
                    {
                        MessageBox.Show("No file has been selected to import", "Import Carrier data");
                    }
                    else
                    {
                        // Carrier determines which path to take
                        string selectedValue = cmbCarrierName.SelectedItem.ToString();

                        switch (selectedValue)
                        {
                            case CARRIER_BICS:
                                // Load carrier file into datatable
                                ExcelReader.ExcelObject excelBics = new ExcelReader.ExcelObject();
                                using (Stream stream = File.OpenRead(_carrierFile)) { _dtCarrier = excelBics.ReadExcelCarrierData(stream); }

                                // Do any grouping of similar rows
                                Combine cb = new Combine();
                                _dtCarrier = cb.GroupByColumn_BICS(_dtCarrier);

                                if (_dtCarrier != null)
                                {
                                    // Change column datatypes for Inv Rate to 4 decimal places
                                    bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Rate", typeof(decimal));

                                    // Change column datatype for "Calls" to decimal
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Calls", typeof(decimal));

                                    this.dgrdCarrierData.ItemsSource = _dtCarrier.DefaultView;
                                    this.lblMsg.Content = "Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported";
                                    MessageBox.Show("Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported", "Import Carrier data");
                                }
                                break;
                            case CARRIER_BTGLOBAL:
                                // Load carrier file into datatable
                                ExcelReader.ExcelObject excelBTglobal = new ExcelReader.ExcelObject();
                                using (Stream stream = File.OpenRead(_carrierFile)) { _dtCarrier = excelBTglobal.ReadExcelCarrierData(stream); }

                                if (_dtCarrier != null)
                                {
                                    // Change column datatypes for Inv Rate to 4 decimal places
                                    bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Rate", typeof(decimal));

                                    // Change column datatype for "Calls" to decimal
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Calls", typeof(decimal));

                                    this.dgrdCarrierData.ItemsSource = _dtCarrier.DefaultView;
                                    this.lblMsg.Content = "Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported";
                                    MessageBox.Show("Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported", "Import Carrier data");
                                }
                                break;
                            case CARRIER_BTSIP:
                                // Load carrier file into datatable
                                ExcelReader.ExcelObject excelBTsip = new ExcelReader.ExcelObject();
                                using (Stream stream = File.OpenRead(_carrierFile)) { _dtCarrier = excelBTsip.ReadExcelCarrierData(stream); }

                                if (_dtCarrier != null)
                                {
                                    // Change column datatypes for Inv Rate to 4 decimal places
                                    bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Rate", typeof(decimal));

                                    // Change column datatype for "Calls" to decimal
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Calls", typeof(decimal));

                                    this.dgrdCarrierData.ItemsSource = _dtCarrier.DefaultView;
                                    this.lblMsg.Content = "Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported";
                                    MessageBox.Show("Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported", "Import Carrier data");
                                }
                                break;
                            case CARRIER_IBASIS:
                                // 2 carrier files so have to be combined
                                // Load first carrier file into 1st datatable
                                ExcelReader.ExcelObject excelIbasis = new ExcelReader.ExcelObject();
                                using (Stream stream = File.OpenRead(_carrierFile)) { _dtCarrier = excelIbasis.ReadExcelCarrierData(stream); }
                                // Load second carrier file
                                using (Stream stream = File.OpenRead(_carrierFile2)) { _dtCarrier2 = excelIbasis.ReadExcelCarrierData(stream); }
                                // Add the 2 datatables
                                Combine combine = new Combine();
                                _dtCarrierCombined = BillVerification_WPF.Controllers.Common.AddDatables(_dtCarrier, _dtCarrier2);
                                // Now doing the grouping of similar rows
                                _dtCarrierCombined = combine.GroupByColumn_IBasis(_dtCarrierCombined);

                                if (_dtCarrierCombined != null)
                                {
                                    // Change column datatypes for Inv Rate to 4 decimal places
                                    bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrierCombined, "Inv Rate", typeof(decimal));

                                    // Change column datatype for "Calls" to decimal
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Calls", typeof(decimal));

                                    this.dgrdCarrierData.ItemsSource = _dtCarrierCombined.DefaultView;
                                    this.lblMsg.Content = "Carrier data imported. " + _dtCarrierCombined.Rows.Count.ToString() + " records imported";
                                    MessageBox.Show("Carrier data imported. " + _dtCarrierCombined.Rows.Count.ToString() + " records imported", "Import Carrier data");
                                }
                                break;
                            case CARRIER_IDT:
                                // Load carrier file into datatable
                                ExcelReader.ExcelObject excelIdt = new ExcelReader.ExcelObject();
                                using (Stream stream = File.OpenRead(_carrierFile)) { _dtCarrier = excelIdt.ReadExcelCarrierData(stream); }

                                if (_dtCarrier != null)
                                {
                                    // Change column datatypes for Inv Rate to 4 decimal places
                                    bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Rate", typeof(decimal));

                                    // Change column datatype for "Calls" to decimal
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Calls", typeof(decimal));

                                    this.dgrdCarrierData.ItemsSource = _dtCarrier.DefaultView;
                                    this.lblMsg.Content = "Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported";
                                    MessageBox.Show("Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported", "Import Carrier data");
                                }
                                break;
                            case CARRIER_MGI:
                                // Load carrier file into datatable
                                ExcelReader.ExcelObject excelMgi = new ExcelReader.ExcelObject();
                                using (Stream stream = File.OpenRead(_carrierFile)) { _dtCarrier = excelMgi.ReadExcelCarrierData(stream); }

                                if (_dtCarrier != null)
                                {
                                    // Change column datatypes for Inv Rate to 4 decimal places
                                    bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Rate", typeof(decimal));
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Calls", typeof(decimal));
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Min", typeof(decimal));
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Charge", typeof(decimal));

                                    this.dgrdCarrierData.ItemsSource = _dtCarrier.DefaultView;
                                    this.lblMsg.Content = "Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported";
                                    MessageBox.Show("Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported", "Import Carrier data");
                                }
                                break;
                            case CARRIER_TATA:
                                // Load carrier file into datatable
                                ExcelReader.ExcelObject excelTata = new ExcelReader.ExcelObject();
                                using (Stream stream = File.OpenRead(_carrierFile)) { _dtCarrier = excelTata.ReadExcelCarrierData(stream); }

                                if (_dtCarrier != null)
                                {
                                    // Change column datatypes to 4 decimal places
                                    bool x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Rate", typeof(decimal));
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Calls", typeof(decimal));
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Min", typeof(decimal));
                                    x = BillVerification_WPF.Controllers.Common.ChangeColumnDataType(_dtCarrier, "Inv Charge", typeof(decimal));

                                    this.dgrdCarrierData.ItemsSource = _dtCarrier.DefaultView;
                                    this.lblMsg.Content = "Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported";
                                    MessageBox.Show("Carrier data imported. " + _dtCarrier.Rows.Count.ToString() + " records imported", "Import Carrier data");
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblMsg.Content = ex.Message;
                MessageBox.Show(ex.Message, "Import Carrier data");
            }
        }

        private void btnBrowseForCarrierDataFile_Click(object sender, RoutedEventArgs e)
        {
            this.lblMsg.Content = "";

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".xlsx";
            fileDialog.Filter = "Excel workbook|*.xlsx";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null)
            {
                HideControls(true);
                this.txtCarrierDataFile.Text = fileDialog.FileName;
                _carrierFile = this.txtCarrierDataFile.Text;
            }
        }

        private void btnMatchData_Click(object sender, RoutedEventArgs e)
        {
            this.tabCtrlViewExcel.SelectedValue = this.tabItmDataMatch;

            if (_dtLiquidSB != null && _dtCarrier != null)
            {
                #region Matched data

                // Do the matching
                Matching match = new Matching();
                _dtMatches = match.GetMatched(_dtLiquidSB,_dtCarrier);

                // Sort the datatable
                //_dtMatches = BillVerification_WPF.Controllers.Common.SortDatableMultipleCols(_dtMatches);

                // Now get rid of dups from the datatable
                //_dtMatches = match.RemoveDuplicates(_dtMatches);

                dgrdDataMatch.ItemsSource = _dtMatches.DefaultView;

                HideControls(false);

                this.lblMsg.Content = "Matching complete";

                _totalPercentageDiffMin = (match.TotalDiffMinutes / match.TotalOurMin) * 100;
                _totalPercentageDiffCost = (match.TotalDiffCost / match.TotalOurCost) * 100;
                _totalInvMin = match.TotalInvMin;
                _totalInvCharge = match.TotalInvCharge;

                // Populate totals
                this.lblTotalOurMin.Content = Math.Round(match.TotalOurMin, 2, MidpointRounding.AwayFromZero);
                this.lblTotalOurCost.Content = Math.Round(match.TotalOurCost, 2, MidpointRounding.AwayFromZero);
                this.lblTotalInvMin.Content = Math.Round(_totalInvMin, 2, MidpointRounding.AwayFromZero);
                this.lblTotalInvCharge.Content = Math.Round(_totalInvCharge, 2, MidpointRounding.AwayFromZero);
                this.lblTotalDiffMinutes.Content = Math.Round(match.TotalDiffMinutes, 2, MidpointRounding.AwayFromZero);
                this.lblTotalPDiffMinutes.Content = Math.Round(_totalPercentageDiffMin, 2, MidpointRounding.AwayFromZero) + "%";
                this.lblTotalDiffCost.Content = Math.Round(match.TotalDiffCost, 2, MidpointRounding.AwayFromZero);
                this.lblTotalPDiffCost.Content = Math.Round(_totalPercentageDiffCost, 2, MidpointRounding.AwayFromZero) + "%";
                this.lblMatchingRowcount.Content = match.MatchingRowCount.ToString();

                _matchedRowCount = match.MatchingRowCount;

                #endregion

                //#region Unmatched data

                //Matching unmatch = new Matching();
                //_dtNoMatches = unmatch.GetUnmatched(_dtLiquidSB, _dtCarrier);
                //dGrdUnmatchedData.ItemsSource = _dtNoMatches.DefaultView;

                //HideControls(false);

                //_totalPercentageDiffMin2 = (unmatch.TotalDiffMinutes / unmatch.TotalOurMin) * 100;
                //_totalPercentageDiffCost2 = (unmatch.TotalDiffCost / unmatch.TotalOurCost) * 100;
                //_totalInvMin2 = unmatch.TotalInvMin;
                //_totalInvCharge2 = unmatch.TotalInvCharge;

                //this.lblUnmatchedRowcount.Content = unmatch.MatchingRowCount.ToString();

                //_unmatchedRowCount = unmatch.MatchingRowCount;

                //#endregion
            }
            else
            {
                // Files have not been imported yet
                this.lblMsg.Content = "You must import both the Springboard extract and the Carrier data before matching";
                MessageBox.Show("You must import the Springboard and Carrier data before matching", "Data matching");
            }
        }

        private void HideControls(bool flag)
        {
            if (flag)
            {
                this.lblLabelTotalOurMin.Visibility = Visibility.Hidden;
                this.lblLabelTotalOurCost.Visibility = Visibility.Hidden;
                this.lblLabelTotalInvMin.Visibility = Visibility.Hidden;
                this.lblLabelTotalInvCharge.Visibility = Visibility.Hidden;
                this.lblLabelTotalDiffMinutes.Visibility = Visibility.Hidden;
                this.lblLabelTotalPDiffMinutes.Visibility = Visibility.Hidden;
                this.lblLabelTotalDiffCost.Visibility = Visibility.Hidden;
                this.lblLabelTotalPDiffCost.Visibility = Visibility.Hidden;
                this.lblLabelPTotalCost.Visibility = Visibility.Hidden;
                this.lblLabelMatchingRowCount.Visibility = Visibility.Hidden;
                this.lblLabelUnmatchedRowCount.Visibility = Visibility.Hidden;

                this.lblTotalOurMin.Visibility = Visibility.Hidden;
                this.lblTotalOurCost.Visibility = Visibility.Hidden;
                this.lblTotalInvMin.Visibility = Visibility.Hidden;
                this.lblTotalInvCharge.Visibility = Visibility.Hidden;
                this.lblTotalDiffMinutes.Visibility = Visibility.Hidden;
                this.lblTotalPDiffMinutes.Visibility = Visibility.Hidden;
                this.lblTotalDiffCost.Visibility = Visibility.Hidden;
                this.lblTotalPDiffCost.Visibility = Visibility.Hidden;
                this.lblPTotalCost.Visibility = Visibility.Hidden;
                this.lblMatchingRowcount.Visibility = Visibility.Hidden;
                this.lblUnmatchedRowcount.Visibility = Visibility.Hidden;
            }
            else
            {
                this.lblLabelTotalOurMin.Visibility = Visibility.Visible;
                this.lblLabelTotalOurCost.Visibility = Visibility.Visible;
                this.lblLabelTotalInvMin.Visibility = Visibility.Visible;
                this.lblLabelTotalInvCharge.Visibility = Visibility.Visible;
                this.lblLabelTotalDiffMinutes.Visibility = Visibility.Visible;
                this.lblLabelTotalPDiffMinutes.Visibility = Visibility.Visible;
                this.lblLabelTotalDiffCost.Visibility = Visibility.Visible;
                this.lblLabelTotalPDiffCost.Visibility = Visibility.Visible;
                this.lblLabelPTotalCost.Visibility = Visibility.Visible;
                this.lblLabelMatchingRowCount.Visibility = Visibility.Visible;
                this.lblLabelUnmatchedRowCount.Visibility = Visibility.Visible;

                this.lblTotalOurMin.Visibility = Visibility.Visible;
                this.lblTotalOurCost.Visibility = Visibility.Visible;
                this.lblTotalInvMin.Visibility = Visibility.Visible;
                this.lblTotalInvCharge.Visibility = Visibility.Visible;
                this.lblTotalDiffMinutes.Visibility = Visibility.Visible;
                this.lblTotalPDiffMinutes.Visibility = Visibility.Visible;
                this.lblTotalDiffCost.Visibility = Visibility.Visible;
                this.lblTotalPDiffCost.Visibility = Visibility.Visible;
                this.lblPTotalCost.Visibility = Visibility.Visible;
                this.lblMatchingRowcount.Visibility = Visibility.Visible;
                this.lblUnmatchedRowcount.Visibility = Visibility.Visible;
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCarrierName.Text.Length > 0 && txtInvoiceDate.Text.Length > 0 && txtInvoiceNumber.Text.Length > 0 && txtInvoicePeriod.Text.Length > 0 && txtCurrency.Text.Length > 0)
            {
                if (_dtMatches != null && _dtNoMatches != null)
                {
                    string filename = string.Empty;

                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.DefaultExt = ".xlsx";
                    sfd.Filter = "Excel workbook|*.xlsx";

                    if (sfd.ShowDialog() == true)
                    {
                        filename = sfd.FileName;

                        try
                        {
                            ExcelReader.ExcelObject excel = new ExcelReader.ExcelObject();
                            excel.CarrierName = this.cmbCarrierName.SelectedValue.ToString();
                            excel.InvoiceDate = this.txtInvoiceDate.Text;
                            excel.InvoiceNo = this.txtInvoiceNumber.Text;
                            excel.InvoicePeriod = this.txtInvoicePeriod.Text;
                            excel.Currency = this.txtCurrency.Text;
                            excel.MatchedRowCount = _matchedRowCount;
                            excel.UnmatchedRowCount = _unmatchedRowCount;
                            excel.TotalPercentageDiffMin = _totalPercentageDiffMin;
                            excel.TotalPercentageDiffCost = _totalPercentageDiffCost;
                            excel.TotalInvCharge = _totalInvCharge;
                            excel.TotalInvMin = _totalInvMin;

                            if (excel.ExportToFile(_dtMatches, _dtNoMatches, filename))
                            {
                                this.lblMsg.Content = "Data exported to: " + filename;
                                MessageBox.Show("Data exported to: " + filename, "Export");
                            }
                            else
                            {
                                this.lblMsg.Content = "An error has occured";
                                MessageBox.Show("An error has occured", "Export");
                            }
                        }
                        catch (Exception ex)
                        {
                            this.lblMsg.Content = ex.Message;
                            MessageBox.Show(ex.Message, "Export");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("You must match the data first before exporting", "Export");
                }
            }
            else
            {
                MessageBox.Show("You must fill in the header information fields before exporting", "Export");
            }
        }

        private void btnBrowseForCarrierDataFile2_Click(object sender, RoutedEventArgs e)
        {
            this.lblMsg.Content = "";

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".xlsx";
            fileDialog.Filter = "Excel workbook|*.xlsx";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null)
            {
                HideControls(true);
                this.txtCarrierDataFile2.Text = fileDialog.FileName;
                _carrierFile2 = this.txtCarrierDataFile2.Text;
            }
        }
    }
}
