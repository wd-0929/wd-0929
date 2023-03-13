using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;

namespace Smt.SDK.Test
{
    public class SmtTemplateFreight
    {
        private string filePath = "D:\\自己的项目\\wd-0929\\Smt.SDK.Test\\Excel";
        private SmtTemplateFreight()
        {
            DirectoryInfo root = new DirectoryInfo(filePath);
            FileInfo files = root.GetFiles().FirstOrDefault();
            var excelFileName = files.FullName;
            IWorkbook hssfworkbook = null;
            if (excelFileName != null && excelFileName.IndexOf(".xlsx") < 0)
                using (FileStream file = new FileStream(excelFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            else if (excelFileName != null && excelFileName.IndexOf(".xlsx") > 0)
            {
                using (FileStream file = new FileStream(excelFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    hssfworkbook = new XSSFWorkbook(file);
                }
            }
            int index = 1;
            Dictionary<string, DataTable[]> sheets = new Dictionary<string, DataTable[]>();
            while (true)
            {
                List<List<string>> Datas = null;
                var Name = string.Empty;
                try
                {
                   var sheet= hssfworkbook.GetSheetAt(index++);
                    Datas = GenerateDataFromExcelTall(sheet);
                    Name = sheet.SheetName;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Sheet index"))
                    { break; }
                }
                var dataTables = GenerateDataFromExcel(Datas);
                sheets.Add(Name, dataTables);
            }

            List<LogisticsFreightDatas> Freights = new List<LogisticsFreightDatas>();
            foreach (var item in sheets)
            {
                if (item.Value.Length > 0)
                {
                    foreach (var value in item.Value)
                    {
                        var codeIndex = value.Columns.IndexOf("Code");

                        foreach (DataRow row in value.Rows)
                        {
                            
                        }   
                    }
                }
            }
        }
        public DataTable[] GenerateDataFromExcel(List<List<string>> Datas) 
        {
            List<DataTable> tables = new List<DataTable>();
            for (int i = 0; i < Datas.Count(); i++)
            {
                if (Datas[i].Count() > 0)
                {
                    var name = Datas[i][0];
                    if (name == "运达国家/地区")
                    {
                        var index = i;
                        List<string> Columns = new List<string>();
                        for (int q = 0; q < Datas[index].Count; q++)
                        {
                            if (Columns.Count < q + 1)
                            {
                                Columns.Add(Datas[index][q]);
                            }
                            else
                            {
                                Columns[q] = Columns[q] + "\r\n" + Datas[index][q];
                            }
                        }
                        index++;
                        DataTable dt = new DataTable();
                        bool isContent = false;
                        rety:
                        for (; index < Datas.Count(); index++) 
                        {
                            var data = Datas[index];
                            if (!isContent) 
                            {
                                if (data.Count > 0)
                                {
                                    if (string.IsNullOrWhiteSpace(data[0]))
                                    {
                                        for (int q = 0; q < data.Count; q++)
                                        {
                                            if (Columns.Count < q + 1)
                                            {
                                                Columns.Add(data[q]);
                                            }
                                            else
                                            {
                                                Columns[q] = Columns[q] + "\r\n" + data[q];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        isContent = true;
                                        int columnIndex = 1;
                                        foreach (var columnName in Columns)
                                        {
                                            if (columnName == "Code")
                                            {
                                                dt.Columns.Add(new DataColumn(columnName, typeof(string)));
                                            }
                                            else
                                                dt.Columns.Add(new DataColumn(string.Format("{0}.{1}", columnIndex, columnName), typeof(string)));
                                            columnIndex++;
                                        }
                                        goto rety;
                                    }
                                }
                            }
                            else 
                            {
                                if (data.Count > 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(data[0]))
                                    {
                                        DataRow dr = dt.NewRow();
                                        for (int q = 0; q < dt.Columns.Count; q++)
                                        {
                                            if (q < data.Count())
                                                dr[q] = data[q];
                                        }
                                        dt.Rows.Add(dr); 
                                    }else break;
                                }
                                else break;
                            }
                        }
                        tables.Add(dt);
                    }
                }
            }
            return tables.ToArray();
        }
        public List<List<string>> GenerateDataFromExcelTall(ISheet sheet)
        {
            List<List<string>> Datass = new List<List<string>>();
            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                List<string> datas = new List<string>();
                if (row != null)
                {
                    foreach (var item in row.Cells)
                    {
                        datas.Add(GetCellValue(item));
                    }
                }
                Datass.Add(datas);
            }
            return Datass;
        }
        private string GetCellValue(ICell cell)
        {
            if (cell != null)
            {
                try
                {
                    switch (cell.CellType)
                    {
                        case CellType.Boolean:
                            return cell.BooleanCellValue.ToString();
                        case CellType.Numeric:
                            try
                            {
                                return Convert.ToDecimal(cell.NumericCellValue).ToString();
                            }
                            catch
                            {
                                return cell.NumericCellValue.ToString();
                            }
                        case CellType.String:
                            return cell.StringCellValue;
                    }
                }
                catch { }
            }
            return string.Empty;
        }
        public static SmtTemplateFreight Create()
        {
            return new SmtTemplateFreight();
        }
    }
    public class LogisticsFreightDatas
    {
        private LogisticsFreightDatas(string sheetName) 
        {
            SheetName = sheetName;
            Datas = new List<LogisticsFreightItem>();
        }
        public string SheetName { get;private set; }
        public List<LogisticsFreightItem> Datas { get; set; }  
    }
    public class LogisticsFreightItem 
    {
        #region CountryCode Property
        private string _countryCode;
        /// <summary>
        /// 国家简码
        /// </summary>
        public string CountryCode
        {
            get
            {
                return _countryCode;
            }
            set
            {
                if (_countryCode != value)
                {
                    _countryCode = value;
                    
                }
            }
        }
        #endregion CountryCode Property


        #region StartWeight Property
        private int? _startWeight;
        /// <summary>
        /// 开始重量(g)
        /// </summary>
        public int? StartWeight
        {
            get
            {
                return _startWeight;
            }
            set
            {
                if (_startWeight != value)
                {
                    _startWeight = value;
                    
                }
            }
        }
        #endregion StartWeight Property


        #region EndWeight Property
        private int? _endWeight;
        /// <summary>
        /// 结束重量(g)
        /// </summary>
        public int? EndWeight
        {
            get
            {
                return _endWeight;
            }
            set
            {
                if (_endWeight != value)
                {
                    _endWeight = value;
                    
                }
            }
        }
        #endregion EndWeight Property


        #region OneWeight Property
        private int? _oneWeight;
        /// <summary>
        /// 首重(g)
        /// </summary>
        public int? OneWeight
        {
            get
            {
                return _oneWeight;
            }
            set
            {
                if (_oneWeight != value)
                {
                    _oneWeight = value;
                    
                }
            }
        }
        #endregion OneWeight Property


        #region OneWeightFreight Property
        private double? _oneWeightFreight;
        /// <summary>
        /// 首重运费(￥)
        /// </summary>
        public double? OneWeightFreight
        {
            get
            {
                return _oneWeightFreight;
            }
            set
            {
                if (_oneWeightFreight != value)
                {
                    _oneWeightFreight = value;
                    
                }
            }
        }
        #endregion OneWeightFreight Property


        #region ContinueWeight Property
        private int? _continueWeight;
        /// <summary>
        /// 续重单位重量(g)
        /// </summary>
        public int? ContinueWeight
        {
            get
            {
                return _continueWeight;
            }
            set
            {
                if (_continueWeight != value)
                {
                    _continueWeight = value;
                    
                }
            }
        }
        #endregion ContinueWeight Property


        #region ContinueWeightFreight Property
        private double? _continueWeightFreight;
        /// <summary>
        /// 续费（￥）
        /// </summary>
        public double? ContinueWeightFreight
        {
            get
            {
                return _continueWeightFreight;
            }
            set
            {
                if (_continueWeightFreight != value)
                {
                    _continueWeightFreight = value;
                    
                }
            }
        }
        #endregion ContinueWeightFreight Property


        #region RegisteredFreight Property
        private double? _registeredFreight;
        /// <summary>
        /// 挂号费(￥)
        /// </summary>
        public double? RegisteredFreight
        {
            get
            {
                return _registeredFreight;
            }
            set
            {
                if (_registeredFreight != value)
                {
                    _registeredFreight = value;
                    
                }
            }
        }
        #endregion RegisteredFreight Property
    }
}
