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
using System.Text.RegularExpressions;
using System.Net;
using WheelTest.Style;
using Newtonsoft.Json.Linq;
using WheelTest.Style.Multitask;
using NPOI.Util;
using CollectLoginBLL;
using NPOI.SS.Util;

namespace Smt.SDK.Test
{
    public class SmtTemplateFreight
    {
        private double UsCurrencyRate = 7.2894;
        private Action<string> SetSrogress;
        private SmtTemplateFreight(Action<string> setSrogress)
        {
            SetSrogress = setSrogress;
            //需要修改excel 有些有问题 菜鸟无忧物流-简易  菜鸟无忧物流-标准
            var excelFileName = System.IO.Path.Combine(AppContext.BaseDirectory, "Excel", "ef1ae29e-f0fb-49cd-998d-c40c293aa612.xlsx");
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(AppContext.BaseDirectory, "Excel")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(AppContext.BaseDirectory, "Excel"));
            }
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.DownloadFile("https://files.alicdn.com/tpsservice/c37222cc12fc4352b347c02c8771a341.xlsx?spm=5261.25812464.0.0.1c0b3648u3mgmw&file=c37222cc12fc4352b347c02c8771a341.xlsx", excelFileName);
            //}

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

                    var sheet = hssfworkbook.GetSheetAt(index++);
                    Datas = GenerateDataFromExcelTall(sheet);
                    Name = sheet.SheetName.Trim();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Sheet index"))
                    { break; }
                }
                var dataTables = GenerateDataFromExcel(Datas);
                sheets.Add(Name, dataTables);
            }

            List<LogisticsFreightDatas> FreightsNew = new List<LogisticsFreightDatas>();
            foreach (var item in sheets)
            {
                if (item.Value.Length > 0 && item.Key != "菜鸟无忧物流-优先")
                {
                    List<LogisticsFreightDatas> Freights = new List<LogisticsFreightDatas>();
                    foreach (var value in item.Value)
                    {
                        var codeIndexNew = value.Columns.IndexOf("Code");
                        var codeIndex = value.Columns.IndexOf("Code");
                        if (item.Key == "中邮e邮宝")
                        {
                            codeIndex = codeIndex + 1;
                        }
                        if (item.Key == "菜鸟无忧物流-标准")
                        {
                        }
                        var minimumNumberIndex = value.Columns.IndexOf("最低计费重量");
                        if (minimumNumberIndex == -1)
                        {
                            minimumNumberIndex = value.Columns.IndexOf("备注（适用于普货和带电）");
                        }
                        string name = string.Empty;
                        foreach (DataRow rows in value.Rows)
                        {
                            Country code = new Country
                            {
                                Code = rows.ItemArray[codeIndexNew].ToString(),
                                EnName = rows.ItemArray[codeIndexNew - 1].ToString(),
                                Name = rows.ItemArray[codeIndexNew - 2].ToString(),
                            };
                            int OneWeight = 1;

                            Regex regex = null;
                            if (minimumNumberIndex > -1)
                            {
                                regex = new Regex("(\\d+)g");
                                var oneweightText = rows.ItemArray[minimumNumberIndex].ToString();
                                if (regex.IsMatch(oneweightText))
                                {
                                    OneWeight = int.Parse(regex.Match(oneweightText).Groups[1].Value);
                                }
                            }
                            LogisticsFreightItem logisticsFreightItem = null;

                            for (int i = 0; i < (rows.ItemArray.Length - (codeIndex + 1)) / 2; i++)
                            {
                                var itemIndex = (codeIndex + i * 2) + 1;
                                var cloumn = value.Columns[itemIndex].ColumnName;
                                if (rows.ItemArray[itemIndex].ToString() == "暂无服务")
                                {
                                    continue;
                                }
                                LogisticsFreightDatas logistics = null;
                                var sheetNames = new string[] { "小包普货", "小包非普货", "大包计费", "带电" };
                                bool isNewMethod = false;
                                foreach (var sheetNameItem in sheetNames)
                                {
                                    if (cloumn.Contains(sheetNameItem))
                                    {
                                        var sheetName = item.Key + $"({sheetNameItem})";
                                        regex = new Regex("（([^）]+仓)）", RegexOptions.IgnoreCase);
                                        if (regex.IsMatch(cloumn))
                                        {
                                            sheetName = sheetName + $"({regex.Match(cloumn).Groups[1].Value})";
                                        }
                                        NewMethod(Freights, value, rows, code, OneWeight, regex, out logisticsFreightItem, itemIndex, cloumn, out logistics, sheetName);
                                        isNewMethod = true;
                                    }
                                }
                                if (!isNewMethod)
                                {
                                    NewMethod(Freights, value, rows, code, OneWeight, regex, out logisticsFreightItem, itemIndex, cloumn, out logistics, item.Key);
                                }
                            }
                        }
                    }
                    if (Freights.Count > 1 && Freights.FirstOrDefault(o => o.SheetName == item.Key) != null)
                    {
                        foreach (var Freight in Freights)
                        {
                            if (Freight.SheetName != item.Key)
                            {
                                Freight.Datas.AddRange(Freights.FirstOrDefault(o => o.SheetName == item.Key).Datas);
                            }
                        }
                        {
                            Freights.Remove(Freights.FirstOrDefault(o => o.SheetName == item.Key));
                        }
                    }
                    FreightsNew.AddRange(Freights);
                }
            }
            CountryAcquire();
            foreach (var item in FreightsNew)
            {
                {
                    Console.WriteLine($"开始对比模板------------{item.SheetName}");

                    using (Multitask<LogisticsFreightItem, string> s = new Multitask<LogisticsFreightItem, string>(10, item.Datas.ToArray(), (p) =>
                    {
                        Comparison(item.SheetName, p);
                        return "";
                    }))
                    {
                        s.WaitAll();
                    }
                }
            }
            foreach (var item in FreightsNew)
            {
                var pathName = System.IO.Path.Combine(AppContext.BaseDirectory, "Excel", "运费模板");
                if (!System.IO.Directory.Exists(pathName))
                {
                    System.IO.Directory.CreateDirectory(pathName);
                }
                pathName = System.IO.Path.Combine(pathName, item.SheetName + ".xlsx");
                string[] ColumnNames = new string[]
                {
                    "国家(二字码或中文)",
                    "开始重量(g)\r\n （*必填）",
                    "结束重量(g)\r\n(*必填)",
                    "首重/起重(g)",
                    "首重/起重运费/g(￥)",
                    "续重单位重量(g)\r\n（*必填）",
                    "单价(￥)/g\r\n（*必填）",
                    "挂号费(￥)"
                };
                using (FileStream stream = new FileStream(pathName, FileMode.Create))
                {
                    var workbook = new XSSFWorkbook();
                    var mySheet = workbook.CreateSheet("运费模板");//获取工作表
                    mySheet.CreateRow(0);
                    for (int i = 0; i < ColumnNames.Length; i++)
                    {
                        mySheet.GetRow(0).CreateCell(i).SetCellValue(ColumnNames[i]);
                    }
                    for (int i = 1; i <= item.Datas.Count; i++)
                    {
                        var data = item.Datas[i - 1];
                        mySheet.CreateRow(i);
                        mySheet.GetRow(i).CreateCell(0).SetCellValue(data.Country.Code);
                        mySheet.GetRow(i).CreateCell(1).SetCellValue(data.StartWeight ?? 0);
                        mySheet.GetRow(i).CreateCell(2).SetCellValue(data.EndWeight ?? 0);
                        mySheet.GetRow(i).CreateCell(3).SetCellValue(data.OneWeight ?? 0);
                        mySheet.GetRow(i).CreateCell(4).SetCellValue(data.OneWeightFreight ?? 0);
                        mySheet.GetRow(i).CreateCell(5).SetCellValue(data.ContinueWeight ?? 0);
                        mySheet.GetRow(i).CreateCell(6).SetCellValue(data.ContinueWeightFreight ?? 0);
                        mySheet.GetRow(i).CreateCell(7).SetCellValue(data.RegisteredFreight ?? 0);
                    }
                    workbook.Write(stream);
                    workbook.Close();
                }
            }
        }

        private void NewMethod(List<LogisticsFreightDatas> Freights, DataTable value, DataRow rows, Country code, int OneWeight, Regex regex, out LogisticsFreightItem logisticsFreightItem, int itemIndex, string cloumn, out LogisticsFreightDatas logistics, string sheetName)
        {
            {
                if (Freights.FirstOrDefault(v => v.SheetName == sheetName) == null)
                {
                    logistics = new LogisticsFreightDatas(sheetName);
                    Freights.Add(logistics);
                }
                else
                {
                    logistics = Freights.FirstOrDefault(v => v.SheetName == sheetName);
                }
                logisticsFreightItem = new LogisticsFreightItem();
                logistics.Datas.Add(logisticsFreightItem);
                logisticsFreightItem.Country = code;
                logisticsFreightItem.OneWeight = OneWeight;
                logisticsFreightItem.ContinueWeight = 1;
                //【普通的
                string[] regexes = new string[] { "(\\d+)(-|~)(\\d+)", "(\\d+)克\\((不含)\\)-(\\d+)克" };
                bool IsMatch = false;
                foreach (var regexekg in regexes)
                {
                    regex = new Regex(regexekg, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(cloumn))
                    {
                        logisticsFreightItem.StartWeight = int.Parse(regex.Match(cloumn).Groups[1].Value);
                        if (logisticsFreightItem.StartWeight % ((double)5) == 0)
                        {
                            logisticsFreightItem.StartWeight = logisticsFreightItem.StartWeight + 1;
                        }
                        logisticsFreightItem.EndWeight = int.Parse(regex.Match(cloumn).Groups[3].Value);
                        IsMatch = true;
                        break;
                    }
                }
                if (!IsMatch)
                {
                    regex = new Regex("(\\d+)(K)?G以内", RegexOptions.IgnoreCase);
                    var regex1 = new Regex("限重(\\d+)(k)?g", RegexOptions.IgnoreCase);
                    var regex2 = new Regex(">(\\d+)(k)?g", RegexOptions.IgnoreCase);
                    if (regex.IsMatch(cloumn))
                    {
                        logisticsFreightItem.StartWeight = 1;
                        logisticsFreightItem.EndWeight = int.Parse(regex.Match(cloumn).Groups[1].Value);
                        if (!string.IsNullOrWhiteSpace(regex.Match(cloumn).Groups[2].Value))
                        {
                            logisticsFreightItem.EndWeight = logisticsFreightItem.EndWeight * 1000;
                        }
                    }
                    else if (regex1.IsMatch(cloumn))
                    {
                        logisticsFreightItem.StartWeight = 1;
                        logisticsFreightItem.EndWeight = int.Parse(regex1.Match(cloumn).Groups[1].Value);
                        if (!string.IsNullOrWhiteSpace(regex1.Match(cloumn).Groups[2].Value))
                        {
                            logisticsFreightItem.EndWeight = logisticsFreightItem.EndWeight * 1000;
                        }
                        if (cloumn.ToLower().Contains("0.5kg首重"))
                            logisticsFreightItem.OneWeight = 500;
                    }
                    else if (regex2.IsMatch(cloumn))
                    {
                        logisticsFreightItem.StartWeight = int.Parse(regex2.Match(cloumn).Groups[1].Value);
                        if (!string.IsNullOrWhiteSpace(regex1.Match(cloumn).Groups[2].Value))
                        {
                            logisticsFreightItem.StartWeight = logisticsFreightItem.StartWeight * 1000;
                        }
                        logisticsFreightItem.EndWeight = 999999999;
                    }
                    else
                    {
                        throw new Exception("----");
                    }
                }
                for (int r = 0; r < 2; r++)
                {
                    var itemIndexNew = itemIndex + r;
                    var cloumnNew = value.Columns[itemIndexNew].ColumnName;
                    if (cloumnNew.Contains("/包裹"))
                    {
                        if (cloumnNew.Contains("RMB"))
                        {
                            logisticsFreightItem.RegisteredFreight = double.Parse(rows.ItemArray[itemIndexNew].ToString());
                        }
                        else if (cloumnNew.Contains("美元"))
                        {
                            logisticsFreightItem.RegisteredFreight = double.Parse(rows.ItemArray[itemIndexNew].ToString()) * UsCurrencyRate;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (cloumnNew.Contains("/KG"))
                        {
                            if (cloumnNew.Contains("RMB"))
                            {
                                if (rows.ItemArray[itemIndexNew].ToString() != "-")
                                    logisticsFreightItem.ContinueWeightFreight = double.Parse(rows.ItemArray[itemIndexNew].ToString()) / 1000;

                            }
                            else if (cloumnNew.Contains("美元"))
                            {
                                logisticsFreightItem.ContinueWeightFreight = (double.Parse(rows.ItemArray[itemIndexNew].ToString()) * UsCurrencyRate) / 1000;
                            }
                        }
                        else if (cloumnNew.Contains("/500G"))
                        {
                            if (cloumnNew.Contains("RMB"))
                            {
                                logisticsFreightItem.ContinueWeightFreight = double.Parse(rows.ItemArray[itemIndexNew].ToString()) / 500;
                            }
                            else if (cloumnNew.Contains("美元"))
                            {
                                logisticsFreightItem.ContinueWeightFreight = (double.Parse(rows.ItemArray[itemIndexNew].ToString()) * UsCurrencyRate) / 500;
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                    regex = new Regex("续重\\(每(\\d+)(k)?G\\)", RegexOptions.IgnoreCase);
                    if (regex.IsMatch(cloumnNew))
                    {
                        logisticsFreightItem.ContinueWeight = int.Parse(regex.Match(cloumnNew).Groups[1].Value);
                        if (!string.IsNullOrWhiteSpace(regex.Match(cloumnNew).Groups[2].Value))
                        {
                            logisticsFreightItem.ContinueWeight = logisticsFreightItem.ContinueWeight * 1000;
                        }
                    }
                }
                if (logisticsFreightItem.OneWeight == 1)
                {
                    logisticsFreightItem.OneWeight = logisticsFreightItem.ContinueWeight;
                }
                logisticsFreightItem.OneWeightFreight = logisticsFreightItem.OneWeight * logisticsFreightItem.ContinueWeightFreight;
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
                                            else if (!string.IsNullOrWhiteSpace(data[q]))
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
                                            if (!string.IsNullOrWhiteSpace(columnName))
                                            {
                                                string[] strings = new string[] { "Code", "最低计费重量", "备注（适用于普货和带电）" };
                                                if (strings.Contains(columnName.Trim()))
                                                {
                                                    dt.Columns.Add(new DataColumn(columnName.Trim(), typeof(string)));
                                                }
                                                else
                                                    dt.Columns.Add(new DataColumn(string.Format("{0}.{1}", columnIndex, columnName), typeof(string)));
                                                columnIndex++;
                                            }
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
                                    }
                                    else break;
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
        public List<CellRangeAddress> GetCellRangeAddresses(ISheet sheet)
        {
            int numMergedRegions = sheet.NumMergedRegions;
            var datas = new List<CellRangeAddress>();
            for (int i = 0; i < numMergedRegions; i++)
            {
                datas.Add(sheet.GetMergedRegion(i));
            }
            return datas;
        }
        public List<List<string>> GenerateDataFromExcelTall(ISheet sheet)
        {
            List<List<string>> Datass = new List<List<string>>();
            try
            {
                var Addresses = GetCellRangeAddresses(sheet);
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    List<string> datas = new List<string>();
                    if (row != null)
                    {
                        for (int c = 0; c < row.Cells.Count; c++)
                        {
                            var data = GetCellValue(row.Cells[c]);
                            if (Addresses.FirstOrDefault(o => o.FirstColumn == c && o.FirstRow == i && o.LastColumn != o.FirstColumn) != null)
                            {
                                var addresse = Addresses.FirstOrDefault(o => o.FirstColumn == c && o.FirstRow == i);
                                for (int r = 0; r < (addresse.LastColumn - addresse.FirstColumn + 1); r++)
                                {
                                    datas.Add(data);
                                }
                                c = c + (addresse.LastColumn - addresse.FirstColumn);
                            }
                            else
                            {
                                datas.Add(data);
                            }
                        }
                    }
                    if (i == 12)
                    {
                    }
                    Datass.Add(datas);
                }
            }
            catch (Exception)
            {

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
        public static SmtTemplateFreight Create(Action<string> setSrogress)
        {
            return new SmtTemplateFreight(setSrogress);
        }
        Dictionary<string, string> headerDic = new Dictionary<string, string>();
        Dictionary<string, string> fromCountryLists = new Dictionary<string, string>();
        Dictionary<string, string> packageTypeLists = new Dictionary<string, string>();
        Dictionary<string, string> toCountryLists = new Dictionary<string, string>();
        string _csrf_token_ = "0ccc0eef-69cb-4ca1-bbe3-51b8884dc48f";
        public void CountryAcquire()
        {
            var MainLoginSetting = new MainLoginSetting
            {
                AccountName = "cn1541248744bjyk",
                AccountPassword = "8Oz67354",
                LoginId = "fm-login-id",
                LoginPassword = "fm-login-password",
                Address = "https://csp.aliexpress.com/apps/home?channelId=36375",
                LoginHost = "login.aliexpress.com",
                OriginalString = "csp.aliexpress.com"
            };
            CollectLoginInfoBLL collectLoginInfoBLL = new CollectLoginInfoBLL(MainLoginSetting);
            CollectLoginInfoBLL.Init(progressValue => { SetSrogress?.Invoke($"组件初始化...{progressValue:0.00%}"); });
        rety:
            var cookie = collectLoginInfoBLL.GetCookie();
            if (string.IsNullOrWhiteSpace(cookie))
            {
                collectLoginInfoBLL.ExecCommand();
                goto rety;
            }
            headerDic.Clear();
            headerDic.Add("Cookie", cookie);
            headerDic.Add("Accept-Encoding", "gzip, deflate, br");
            headerDic.Add("Accept-Language", "zh-CN,zh;q=0.9");
            var relust = WebClientHelper.HttpWebClient("https://sg-cgmp.aliexpress.com/seller-center/logistics/service/init?_csrf_token_=" + _csrf_token_, headerDic);
            JObject jObject = JObject.Parse(relust);
            if (!jObject.Value<bool>("success"))
            {
                collectLoginInfoBLL.DeleteCookie();
                goto rety;
            }
            var fromCountryList = jObject["data"].Value<JArray>("fromCountryList");
            foreach (var fromCountry in fromCountryList)
            {
                fromCountryLists.Add(fromCountry.Value<string>("id"), fromCountry.Value<string>("name"));
            }
            var packageTypeList = jObject["data"].Value<JArray>("packageTypeList");
            foreach (var packageType in packageTypeList)
            {
                packageTypeLists.Add(packageType.Value<string>("code"), packageType.Value<string>("name"));
            }
            var toCountryList = jObject["data"].Value<JArray>("toCountryList");
            foreach (var toCountry in toCountryList)
            {
                toCountryLists.Add(toCountry.Value<string>("id"), toCountry.Value<string>("searchName"));
            }
        }
        public void Comparison(string name, LogisticsFreightItem logisticsFreightItem)
        {
            string fromCountryId = fromCountryLists.FirstOrDefault(o => o.Value == "中国").Key;
            if (name.Contains("香港仓"))
            {
                fromCountryId = fromCountryLists.FirstOrDefault(o => o.Value.Contains("香港")).Key;
            }
            var toCountryId = toCountryLists.FirstOrDefault(o => o.Value == logisticsFreightItem.Country.Name).Key;
            var packageType = packageTypeLists.FirstOrDefault(o => o.Value.Contains("普通货物")).Key;
            if (name.Contains("非普货"))
            {
                packageType = packageTypeLists.FirstOrDefault(o => o.Value.Contains("带电")).Key;
            }
            string packageLength = "1";
            string packageHeight = "1";
            string packageWidth = "1";
            Random ran = new Random();
            var StartWeight = ((logisticsFreightItem.StartWeight ?? 0) + 1);
            if (StartWeight <= 10)
            {
                StartWeight = 11;
            }
            if (name.Contains("大包"))
            {
                packageLength = "5";
                packageHeight = "100";
                packageWidth = "16";
                if (StartWeight <= 2000)
                {
                    StartWeight = 2001;
                }
            }
            var EndWeight = logisticsFreightItem.EndWeight ?? 0;
            if (EndWeight > 99000)
            {
                EndWeight = 98999;
            }
            int n = ran.Next(StartWeight, EndWeight);
            var n1 = (n / ((double)1000)).ToRound();
            var packageWeight = n1.ToString();
            if (string.IsNullOrWhiteSpace(toCountryId))
            {
                Console.WriteLine($"国家:{logisticsFreightItem.Country.Name}  获取失败");
                return;
            }
            var json = new
            {
                fromCountryId = fromCountryId,
                toCountryId = toCountryId,
                packageType = packageType,
                PackageQueryRequest = new
                {
                    packageWeight = packageWeight,
                    packageLength = packageLength,
                    packageHeight = packageHeight,
                    packageWidth = packageWidth,
                },
                useTemplate = true,
                _csrf_token_ = _csrf_token_,
            };
            var relust = WebClientHelper.HttpWebClient("http://sg-cgmp.aliexpress.com/seller-center/logistics/service/query", headerDic, json.ToJsonData(), true);
            Dictionary<string, double> logisticsPrice = new Dictionary<string, double>();
            JObject jo = JObject.Parse(relust);
            foreach (var item in jo["data"].Value<JArray>("otherList"))
            {
                var amount = item.Value<JObject>("estimateFee")?.Value<string>("amount");
                double amountdouble = 0;
                if (!string.IsNullOrWhiteSpace(amount) && double.TryParse(amount, out amountdouble))
                {
                    logisticsPrice.Add(item["serviceInfo"].Value<string>("serviceName"), amountdouble);
                }
            }
            foreach (var item in jo["data"].Value<JArray>("recommendList"))
            {
                var amount = item.Value<JObject>("estimateFee")?.Value<string>("amount");
                double amountdouble = 0;
                if (!string.IsNullOrWhiteSpace(amount) && double.TryParse(amount, out amountdouble))
                {
                    logisticsPrice.Add(item["serviceInfo"].Value<string>("serviceName"), amountdouble);
                }
            }
            Regex regexName = new Regex("^([^\\(]+)");
            if (regexName.IsMatch(name))
            {
                name = regexName.Match(name).Groups[1].Value;
            }
            if (logisticsPrice.ContainsKey(name))
            {
                var a = ((n1 * 1000 * logisticsFreightItem.ContinueWeightFreight + logisticsFreightItem.RegisteredFreight) ?? 0).ToRound();
                if (!(a == logisticsPrice.FirstOrDefault(i => i.Key == name).Value))
                {
                    Console.WriteLine($"国家:{logisticsFreightItem.Country.Name} 对比不一致 重量:{n1}g   接口为{logisticsPrice.FirstOrDefault(i => i.Key == name).Value},自己计算{a.ToRound()}");
                }
            }
            else
            {
                Console.WriteLine($"国家:{logisticsFreightItem.Country.Name} 重量:{n1}g 获取失败");
            }
        }
    }
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
    }
    public class LogisticsFreightDatas
    {
        public LogisticsFreightDatas(string sheetName)
        {
            SheetName = sheetName;
            Datas = new List<LogisticsFreightItem>();
        }
        public string SheetName { get; private set; }
        public List<LogisticsFreightItem> Datas { get; set; }
    }
    public class LogisticsFreightItem
    {
        #region CountryCode Property
        private Country _countryCode;
        /// <summary>
        /// 国家简码
        /// </summary>
        public Country Country
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
