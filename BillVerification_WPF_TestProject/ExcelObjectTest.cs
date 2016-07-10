using BillVerification_WPF.ExcelReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Data;
using OfficeOpenXml;

namespace BillVerification_WPF_TestProject
{
    
    
    /// <summary>
    ///This is a test class for ExcelObjectTest and is intended
    ///to contain all ExcelObjectTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExcelObjectTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

    }
}
