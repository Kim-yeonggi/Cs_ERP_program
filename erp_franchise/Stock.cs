using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.Diagnostics;
using System.Data;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace erp_franchise
{
    public class Stock
    {
        public string Class { get; set; } // 품목 클래스
        public string Code { get; set; } // 품목 코드
        public string Name { get; set; } // 품목 이름
        public int Amount { get; set; } // 수량
        public string Unit { get; set; } // 단위
        public int Price { get; set; } // 가격
        public string Account { get; set; } // 계정
        public string Status { get; set; } // 등록 상태


        public Stock(string clss, string code, string name, int amount, string unit, int price, string account, string status)
        {
            Class = clss;
            Code = code;
            Name = name;
            Amount = amount;
            Unit = unit;
            Price = price;
            Account = account;
            Status = status;
        }

        public static List<List<string>> GetFranStock()
        {
            List<List<string>> data = new List<List<string>>();
            string filePath = "stockdata.csv";
            try
            {
                string[] lines = File.ReadAllLines(filePath, Encoding.Default);

                foreach (string line in lines)
                {
                    List<string> rowData = new List<string>(line.Split(','));
                    data.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파일을 읽는 도중 오류가 발생했습니다: {ex.Message}");
            }

            return data;
        }
        public static List<List<string>> GetBskStock()
        {
            List<List<string>> data = new List<List<string>>();
            string filePath = "stockdata.csv";
            try
            {
                string[] lines = File.ReadAllLines(filePath, Encoding.Default);

                foreach (string line in lines)
                {
                    List<string> rowData = new List<string>(line.Split(','));
                    data.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파일을 읽는 도중 오류가 발생했습니다: {ex.Message}");
            }

            return data;
        }

    }
}
