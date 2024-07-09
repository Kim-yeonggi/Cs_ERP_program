using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erp_franchise
{
    internal class Customer
    {
        private static List<List<string>> _customer;
        public string csvFilePath = "../../customInfo.csv"; // 파일 경로

        private static List<List<string>> _csvOdData;
        public string csvOdFilePath = "../../orderList.csv"; // 파일 경로

        public static List<List<string>> Customer_property
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public static List<List<string>> OdList_property
        {
            get { return _csvOdData; }
            set { _csvOdData = value; }
        }
    }
}
