using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OfficeOpenXml;

namespace WheelTest
{
    /// <summary>
    /// Excel转成sql.xaml 的交互逻辑
    /// </summary>
    public partial class Excel转成sql : Window
    {
        public Excel转成sql()
        {
            InitializeComponent();
            Main1();
        }

        static void Main1()
        {
            string excelFilePath = "C:\\Users\\Administrator\\Desktop\\工作簿1.xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // 读取Excel文件
            DataTable dataTable = ReadExcelFile(excelFilePath);

            string insertDataSql = GenerateInsertDataSql(dataTable);


            Console.WriteLine("数据已成功导入到SQL数据库。");
        }
  
        static DataTable ReadExcelFile(string filePath)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                DataTable dataTable = new DataTable();

                // 添加列
                for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
                {
                    dataTable.Columns.Add(worksheet.Cells[1, i].Text);
                }

                // 添加行
                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    DataRow row = dataTable.NewRow();
                    for (int j = 1; j <= worksheet.Dimension.End.Column; j++)
                    {
                        row[j - 1] = worksheet.Cells[i, j].Text;
                    }
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
        }

        static string GenerateCreateTableSql(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CREATE TABLE users (");

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append($"{dataTable.Columns[i].ColumnName} VARCHAR(255)");
            }

            sb.AppendLine(");");
            return sb.ToString();
        }

        static string GenerateInsertDataSql(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            

           


            foreach (DataRow row in dataTable.Rows)
            {
                sb.Append("countryCodes.Add(new CountryCode(\""+ row[0].ToString() + "\"){");
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(", ");
                    string value =  row[i].ToString() ;
                    string name = dataTable.Columns[i].ColumnName.ToString();
                    string[] names = new []{ "Code", "EnName", "CnName", "EmsCode", "Id" };
                    if (names.Contains(name)) 
                    {
                        value = "\""+ value + "\"";
                    }
                    if (string.IsNullOrEmpty(value)) 
                    {
                        value = "null";
                    }
                    if (name == "CountryArea") 
                    {
                        value = "(CountryArea)" + value;
                    }
                    sb.Append($"{name}={value}");
                }
                sb.AppendLine("});");
            }


            return sb.ToString();
        }

        static void ExecuteLiteDbSql(string dbFilePath, string sql)
        {
        
        }

    }
}
