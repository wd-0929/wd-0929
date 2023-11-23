using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using WheelTest.Style;
using WheelTest.Translate;

namespace WheelTest
{
    /// <summary>
    /// WheelTestManage.xaml 的交互逻辑
    /// </summary>
    public partial class WheelTestManage : Window
    {
        public WheelTestManage()
        {
            InitializeComponent();
        }

        private void Translate_Click(object sender, RoutedEventArgs e)
        {
            Translate.Translate.YouDaoTranslate("啊实打实");
        }

        private void AESdecrypt_Click(object sender, RoutedEventArgs e)
        {
            Translate.Translate.AesDecrypt();
        }

        private void HotKeysList_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecCommand(System.IO.Path.Combine(AppUtility.ExecPath, "HotKeysList.exe"), "/sxml " + System.IO.Path.Combine(AppUtility.ExecPath, "HotKeysList.Txt"));
        }

        private void Json_Click(object sender, RoutedEventArgs e)
        {
            bool a = true;
            var datas = a.ToJsonData();
        }

        private void UpdateVSoutputPath_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            //System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            //if (result == System.Windows.Forms.DialogResult.Cancel)
            //{
            //    return;
            //}
            var SelectedPath = "D:\\助手的项目\\GTA-BUG\\GTA\\Logistics";
            List<string> paths = new List<string>();
            List<PropertyGroup> PropertyGroups = new List<PropertyGroup>();
             var datas = LoadAllFile(SelectedPath, paths);
            List<string> NewPath = new List<string>();
             foreach (var data in datas)
            {
                //var data = "D:\\助手的项目\\GTA-BUG\\GTA\\JSON.GTA.AliChoice\\JSON.GTA.POPChoice\\JSON.GTA.POPChoice.csproj";
                XmlDocument doc = new XmlDocument();
                doc.Load(data);
                foreach (XmlNode childNodes in doc.ChildNodes)
                {
                    #region x64

                    //PropertyGroup propertyGroupDebugAnyCPU = null;
                    //bool IspropertyGroupDebugx64 = false;
                    //PropertyGroup propertyGroupReleaseAnyCPU = null;
                    //bool IspropertyGroupReleasex64 = false;
                    //XmlNode InsertBefore = null;
                    //foreach (XmlNode childNode in childNodes.ChildNodes)
                    //{
                    //    if (childNode.Name == "PropertyGroup" && childNode.Attributes["Condition"] != null)
                    //    {
                    //        if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'")
                    //        {
                    //            propertyGroupDebugAnyCPU = new PropertyGroup();
                    //            foreach (XmlNode item in childNode.ChildNodes)
                    //            {
                    //                switch (item.Name)
                    //                {
                    //                    case "DebugSymbols":
                    //                        propertyGroupDebugAnyCPU.DebugSymbols = item.InnerText;
                    //                        break;
                    //                    case "OutputPath":
                    //                        propertyGroupDebugAnyCPU.OutputPath = item.InnerText;
                    //                        break;
                    //                    case "DefineConstants":
                    //                        propertyGroupDebugAnyCPU.DefineConstants = item.InnerText;
                    //                        break;
                    //                    case "DebugType":
                    //                        propertyGroupDebugAnyCPU.DebugType = item.InnerText;
                    //                        break;
                    //                    case "PlatformTarget":
                    //                        propertyGroupDebugAnyCPU.PlatformTarget = "x64";
                    //                        break;
                    //                    case "ErrorReport":
                    //                        propertyGroupDebugAnyCPU.ErrorReport = item.InnerText;
                    //                        break;
                    //                    case "CodeAnalysisRuleSet":
                    //                        propertyGroupDebugAnyCPU.CodeAnalysisRuleSet = item.InnerText;
                    //                        break;
                    //                    case "Prefer32Bit":
                    //                        propertyGroupDebugAnyCPU.Prefer32Bit = item.InnerText;
                    //                        break;
                    //                }
                    //            }
                    //            PropertyGroups.Add(propertyGroupDebugAnyCPU);
                    //        }
                    //        else if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Release|AnyCPU'")
                    //        {
                    //            propertyGroupReleaseAnyCPU = new PropertyGroup();
                    //            foreach (XmlNode item in childNode.ChildNodes)
                    //            {
                    //                switch (item.Name)
                    //                {
                    //                    case "DebugSymbols":
                    //                        propertyGroupReleaseAnyCPU.DebugSymbols = item.InnerText;
                    //                        break;
                    //                    case "OutputPath":
                    //                        propertyGroupReleaseAnyCPU.OutputPath = item.InnerText;
                    //                        break;
                    //                    case "DefineConstants":
                    //                        propertyGroupReleaseAnyCPU.DefineConstants = item.InnerText;
                    //                        break;
                    //                    case "DebugType":
                    //                        propertyGroupReleaseAnyCPU.DebugType = item.InnerText;
                    //                        break;
                    //                    case "PlatformTarget":
                    //                        propertyGroupReleaseAnyCPU.PlatformTarget = "x64";
                    //                        break;
                    //                    case "ErrorReport":
                    //                        propertyGroupReleaseAnyCPU.ErrorReport = item.InnerText;
                    //                        break;
                    //                    case "CodeAnalysisRuleSet":
                    //                        propertyGroupReleaseAnyCPU.CodeAnalysisRuleSet = item.InnerText;
                    //                        break;
                    //                    case "Prefer32Bit":
                    //                        propertyGroupReleaseAnyCPU.Prefer32Bit = item.InnerText;
                    //                        break;
                    //                }
                    //            }
                    //            PropertyGroups.Add(propertyGroupReleaseAnyCPU);
                    //        }

                    //        if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Debug|x64'")
                    //        {
                    //            IspropertyGroupDebugx64 = true;
                    //            foreach (XmlNode item in childNode.ChildNodes)
                    //            {
                    //                switch (item.Name)
                    //                {
                    //                    case "PlatformTarget":
                    //                        item.InnerText = "x64";
                    //                        break;
                    //                }
                    //            }
                    //            if (!NewPath.Contains(data))
                    //                NewPath.Add(data);
                    //        }
                    //        else if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Release|x64'")
                    //        {
                    //            IspropertyGroupReleasex64 = true;
                    //            foreach (XmlNode item in childNode.ChildNodes)
                    //            {
                    //                switch (item.Name)
                    //                {
                    //                    case "PlatformTarget":
                    //                        item.InnerText = "x64";
                    //                        break;
                    //                }
                    //            }
                    //            if (!NewPath.Contains(data))
                    //                NewPath.Add(data);
                    //        }
                    //    }
                    //    if (childNode.Name == "PropertyGroup" && childNode.Attributes["Condition"] == null) 
                    //    {
                    //        InsertBefore = childNode;
                    //    }
                    //}
                    //if (propertyGroupDebugAnyCPU != null && !IspropertyGroupDebugx64)
                    //{
                    //    XmlElement book1 = doc.CreateElement("PropertyGroup", childNodes.Attributes["xmlns"].Value);
                    //    book1.SetAttribute("Condition", "'$(Configuration)|$(Platform)' == 'Debug|x64'");
                    //    InsertBefore.ParentNode.InsertBefore(book1, InsertBefore);
                    //    XmlElement debugSymbols = doc.CreateElement("DebugSymbols", childNodes.Attributes["xmlns"].Value);
                    //    debugSymbols.InnerText = propertyGroupDebugAnyCPU.DebugSymbols;
                    //    book1.AppendChild(debugSymbols);

                    //    XmlElement OutputPath = doc.CreateElement("OutputPath", childNodes.Attributes["xmlns"].Value);
                    //    OutputPath.InnerText = propertyGroupDebugAnyCPU.OutputPath.Replace("Debug\\", "x64\\Debug\\");
                    //    book1.AppendChild(OutputPath);

                    //    XmlElement DefineConstants = doc.CreateElement("DefineConstants", childNodes.Attributes["xmlns"].Value);
                    //    DefineConstants.InnerText = "TRACE";
                    //    book1.AppendChild(DefineConstants);

                    //    XmlElement DebugType = doc.CreateElement("DebugType", childNodes.Attributes["xmlns"].Value);
                    //    DebugType.InnerText = propertyGroupDebugAnyCPU.DebugType;
                    //    book1.AppendChild(DebugType);

                    //    XmlElement PlatformTarget = doc.CreateElement("PlatformTarget", childNodes.Attributes["xmlns"].Value);
                    //    PlatformTarget.InnerText = "x64";
                    //    book1.AppendChild(PlatformTarget);

                    //    XmlElement ErrorReport = doc.CreateElement("ErrorReport", childNodes.Attributes["xmlns"].Value);
                    //    ErrorReport.InnerText = propertyGroupDebugAnyCPU.ErrorReport;
                    //    book1.AppendChild(ErrorReport);

                    //    XmlElement CodeAnalysisRuleSet = doc.CreateElement("CodeAnalysisRuleSet", childNodes.Attributes["xmlns"].Value);
                    //    CodeAnalysisRuleSet.InnerText = propertyGroupDebugAnyCPU.CodeAnalysisRuleSet;
                    //    book1.AppendChild(CodeAnalysisRuleSet);

                    //    XmlElement Prefer32Bit = doc.CreateElement("Prefer32Bit", childNodes.Attributes["xmlns"].Value);
                    //    Prefer32Bit.InnerText = propertyGroupDebugAnyCPU.Prefer32Bit;
                    //    book1.AppendChild(Prefer32Bit);
                    //    if (!NewPath.Contains(data))
                    //        NewPath.Add(data);
                    //}
                    //if (propertyGroupReleaseAnyCPU != null && !IspropertyGroupReleasex64)
                    //{
                    //    XmlElement book1 = doc.CreateElement("PropertyGroup", childNodes.Attributes["xmlns"].Value);
                    //    book1.SetAttribute("Condition", "'$(Configuration)|$(Platform)' == 'Release|x64'");
                    //    InsertBefore.ParentNode.InsertBefore(book1, InsertBefore);
                    //    XmlElement debugSymbols = doc.CreateElement("DebugSymbols", childNodes.Attributes["xmlns"].Value);
                    //    debugSymbols.InnerText = propertyGroupReleaseAnyCPU.DebugSymbols;
                    //    book1.AppendChild(debugSymbols);

                    //    XmlElement OutputPath = doc.CreateElement("OutputPath", childNodes.Attributes["xmlns"].Value);
                    //    OutputPath.InnerText = propertyGroupReleaseAnyCPU.OutputPath.Replace("Release\\", "x64\\Release\\");
                    //    book1.AppendChild(OutputPath);

                    //    XmlElement DefineConstants = doc.CreateElement("DefineConstants", childNodes.Attributes["xmlns"].Value);
                    //    DefineConstants.InnerText = "TRACE";
                    //    book1.AppendChild(DefineConstants);

                    //    XmlElement DebugType = doc.CreateElement("DebugType", childNodes.Attributes["xmlns"].Value);
                    //    DebugType.InnerText = propertyGroupReleaseAnyCPU.DebugType;
                    //    book1.AppendChild(DebugType);

                    //    XmlElement PlatformTarget = doc.CreateElement("PlatformTarget", childNodes.Attributes["xmlns"].Value);
                    //    PlatformTarget.InnerText = "x64";
                    //    book1.AppendChild(PlatformTarget);

                    //    XmlElement ErrorReport = doc.CreateElement("ErrorReport", childNodes.Attributes["xmlns"].Value);
                    //    ErrorReport.InnerText = propertyGroupReleaseAnyCPU.ErrorReport;
                    //    book1.AppendChild(ErrorReport);

                    //    XmlElement CodeAnalysisRuleSet = doc.CreateElement("CodeAnalysisRuleSet", childNodes.Attributes["xmlns"].Value);
                    //    CodeAnalysisRuleSet.InnerText = propertyGroupReleaseAnyCPU.CodeAnalysisRuleSet;
                    //    book1.AppendChild(CodeAnalysisRuleSet);

                    //    XmlElement Prefer32Bit = doc.CreateElement("Prefer32Bit", childNodes.Attributes["xmlns"].Value);
                    //    Prefer32Bit.InnerText = propertyGroupReleaseAnyCPU.Prefer32Bit;
                    //    book1.AppendChild(Prefer32Bit);
                    //    if (!NewPath.Contains(data))
                    //        NewPath.Add(data);
                    //}
                    #endregion

                    #region 不复制本地
                    foreach (XmlNode childNode in childNodes.ChildNodes) 
                    {
                        if (childNode.Name == "ItemGroup") 
                        {
                            foreach (XmlNode item in childNode.ChildNodes)
                            {
                                if ((item.Name == "ProjectReference"|| item.Name == "Reference") && item.Attributes["Include"]?.Value?.Contains("JSON.GTA")==true)
                                {
                                    if (item.ChildNodes.OfType<XmlNode>().FirstOrDefault(o => o.Name == "Private") != null)
                                    {
                                        item.ChildNodes.OfType<XmlNode>().FirstOrDefault(o => o.Name == "Private").InnerText = "False";
                                    }
                                    else
                                    {
                                        XmlElement debugSymbols = doc.CreateElement("Private", childNodes.Attributes["xmlns"].Value);
                                        debugSymbols.InnerText = "False";
                                        item.AppendChild(debugSymbols);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                doc.Save(data);
            }
        }

        // 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class PropertyGroup
        {


            private string debugSymbolsField;

            private string outputPathField;

            private string defineConstantsField;

            private string debugTypeField;

            private string platformTargetField;

            private string errorReportField;

            private string codeAnalysisRuleSetField;

            private string prefer32BitField;

            private string conditionField;

            /// <remarks/>
            public string DebugSymbols
            {
                get
                {
                    return this.debugSymbolsField;
                }
                set
                {
                    this.debugSymbolsField = value;
                }
            }

            /// <remarks/>
            public string OutputPath
            {
                get
                {
                    return this.outputPathField;
                }
                set
                {
                    this.outputPathField = value;
                }
            }

            /// <remarks/>
            public string DefineConstants
            {
                get
                {
                    return this.defineConstantsField;
                }
                set
                {
                    this.defineConstantsField = value;
                }
            }

            /// <remarks/>
            public string DebugType
            {
                get
                {
                    return this.debugTypeField;
                }
                set
                {
                    this.debugTypeField = value;
                }
            }

            /// <remarks/>
            public string PlatformTarget
            {
                get
                {
                    return this.platformTargetField;
                }
                set
                {
                    this.platformTargetField = value;
                }
            }

            /// <remarks/>
            public string ErrorReport
            {
                get
                {
                    return this.errorReportField;
                }
                set
                {
                    this.errorReportField = value;
                }
            }

            /// <remarks/>
            public string CodeAnalysisRuleSet
            {
                get
                {
                    return this.codeAnalysisRuleSetField;
                }
                set
                {
                    this.codeAnalysisRuleSetField = value;
                }
            }

            /// <remarks/>
            public string Prefer32Bit
            {
                get
                {
                    return this.prefer32BitField;
                }
                set
                {
                    this.prefer32BitField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Condition
            {
                get
                {
                    return this.conditionField;
                }
                set
                {
                    this.conditionField = value;
                }
            }

        }


        /// <summary>
        /// 读取文件夹中指定文件
        /// </summary>
        public List<string> LoadAllFile(string path, List<string> paths)
       {
            string mFilePath = path;
            string[] directories = Directory.GetDirectories(mFilePath);
            string[] fileNames = Directory.GetFiles(mFilePath);
            if (directories.IsEmptyAndGreaterThanZero())
                foreach (string fileName in directories)
                {
                    var Extension = System.IO.Path.GetExtension(fileName);
                    string[] noPath = new string[] { ".git", ".idea", ".vs" };
                    if (!noPath.Contains(Extension)) 
                    {
                        LoadAllFile(fileName, paths);
                    }
                }
            if (fileNames.IsEmptyAndGreaterThanZero()) 
            {
                foreach (var item in fileNames)
                {
                    var Extension = System.IO.Path.GetExtension(item);
                    if (IsTexture(Extension))
                    {
                        paths.Add(item);
                        break;
                    }
                }
            }
            return paths;
        }
       private bool IsTexture(string ext)
       {
           string[] exts = { ".csproj" };
           return exts.Contains(ext.ToLower());
       }
    }
}
