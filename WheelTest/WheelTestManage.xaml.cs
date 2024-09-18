using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
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
            var SelectedPath = "D:\\助手的项目\\GTA";
            List<string> paths = new List<string>();
            List<PropertyGroup> PropertyGroups = new List<PropertyGroup>();
            var datas = LoadAllFile(SelectedPath, paths);
            List<string> NewPath = new List<string>();

            #region 更改记录
            {
                // foreach (var data in datas)
                //{
                //    //var data = "D:\\助手的项目\\GTA-BUG\\GTA\\JSON.GTA.AliChoice\\JSON.GTA.POPChoice\\JSON.GTA.POPChoice.csproj";
                //    XmlDocument doc = new XmlDocument();
                //    doc.Load(data);
                //    foreach (XmlNode childNodes in doc.ChildNodes)
                //    {
                //        #region x64

                //        //PropertyGroup propertyGroupDebugAnyCPU = null;
                //        //bool IspropertyGroupDebugx64 = false;
                //        //PropertyGroup propertyGroupReleaseAnyCPU = null;
                //        //bool IspropertyGroupReleasex64 = false;
                //        //XmlNode InsertBefore = null;
                //        //foreach (XmlNode childNode in childNodes.ChildNodes)
                //        //{
                //        //    if (childNode.Name == "PropertyGroup" && childNode.Attributes["Condition"] != null)
                //        //    {
                //        //        if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'")
                //        //        {
                //        //            propertyGroupDebugAnyCPU = new PropertyGroup();
                //        //            foreach (XmlNode item in childNode.ChildNodes)
                //        //            {
                //        //                switch (item.Name)
                //        //                {
                //        //                    case "DebugSymbols":
                //        //                        propertyGroupDebugAnyCPU.DebugSymbols = item.InnerText;
                //        //                        break;
                //        //                    case "OutputPath":
                //        //                        propertyGroupDebugAnyCPU.OutputPath = item.InnerText;
                //        //                        break;
                //        //                    case "DefineConstants":
                //        //                        propertyGroupDebugAnyCPU.DefineConstants = item.InnerText;
                //        //                        break;
                //        //                    case "DebugType":
                //        //                        propertyGroupDebugAnyCPU.DebugType = item.InnerText;
                //        //                        break;
                //        //                    case "PlatformTarget":
                //        //                        propertyGroupDebugAnyCPU.PlatformTarget = "x64";
                //        //                        break;
                //        //                    case "ErrorReport":
                //        //                        propertyGroupDebugAnyCPU.ErrorReport = item.InnerText;
                //        //                        break;
                //        //                    case "CodeAnalysisRuleSet":
                //        //                        propertyGroupDebugAnyCPU.CodeAnalysisRuleSet = item.InnerText;
                //        //                        break;
                //        //                    case "Prefer32Bit":
                //        //                        propertyGroupDebugAnyCPU.Prefer32Bit = item.InnerText;
                //        //                        break;
                //        //                }
                //        //            }
                //        //            PropertyGroups.Add(propertyGroupDebugAnyCPU);
                //        //        }
                //        //        else if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Release|AnyCPU'")
                //        //        {
                //        //            propertyGroupReleaseAnyCPU = new PropertyGroup();
                //        //            foreach (XmlNode item in childNode.ChildNodes)
                //        //            {
                //        //                switch (item.Name)
                //        //                {
                //        //                    case "DebugSymbols":
                //        //                        propertyGroupReleaseAnyCPU.DebugSymbols = item.InnerText;
                //        //                        break;
                //        //                    case "OutputPath":
                //        //                        propertyGroupReleaseAnyCPU.OutputPath = item.InnerText;
                //        //                        break;
                //        //                    case "DefineConstants":
                //        //                        propertyGroupReleaseAnyCPU.DefineConstants = item.InnerText;
                //        //                        break;
                //        //                    case "DebugType":
                //        //                        propertyGroupReleaseAnyCPU.DebugType = item.InnerText;
                //        //                        break;
                //        //                    case "PlatformTarget":
                //        //                        propertyGroupReleaseAnyCPU.PlatformTarget = "x64";
                //        //                        break;
                //        //                    case "ErrorReport":
                //        //                        propertyGroupReleaseAnyCPU.ErrorReport = item.InnerText;
                //        //                        break;
                //        //                    case "CodeAnalysisRuleSet":
                //        //                        propertyGroupReleaseAnyCPU.CodeAnalysisRuleSet = item.InnerText;
                //        //                        break;
                //        //                    case "Prefer32Bit":
                //        //                        propertyGroupReleaseAnyCPU.Prefer32Bit = item.InnerText;
                //        //                        break;
                //        //                }
                //        //            }
                //        //            PropertyGroups.Add(propertyGroupReleaseAnyCPU);
                //        //        }

                //        //        if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Debug|x64'")
                //        //        {
                //        //            IspropertyGroupDebugx64 = true;
                //        //            foreach (XmlNode item in childNode.ChildNodes)
                //        //            {
                //        //                switch (item.Name)
                //        //                {
                //        //                    case "PlatformTarget":
                //        //                        item.InnerText = "x64";
                //        //                        break;
                //        //                }
                //        //            }
                //        //            if (!NewPath.Contains(data))
                //        //                NewPath.Add(data);
                //        //        }
                //        //        else if (childNode.Attributes["Condition"].Value.Trim() == "'$(Configuration)|$(Platform)' == 'Release|x64'")
                //        //        {
                //        //            IspropertyGroupReleasex64 = true;
                //        //            foreach (XmlNode item in childNode.ChildNodes)
                //        //            {
                //        //                switch (item.Name)
                //        //                {
                //        //                    case "PlatformTarget":
                //        //                        item.InnerText = "x64";
                //        //                        break;
                //        //                }
                //        //            }
                //        //            if (!NewPath.Contains(data))
                //        //                NewPath.Add(data);
                //        //        }
                //        //    }
                //        //    if (childNode.Name == "PropertyGroup" && childNode.Attributes["Condition"] == null) 
                //        //    {
                //        //        InsertBefore = childNode;
                //        //    }
                //        //}
                //        //if (propertyGroupDebugAnyCPU != null && !IspropertyGroupDebugx64)
                //        //{
                //        //    XmlElement book1 = doc.CreateElement("PropertyGroup", childNodes.Attributes["xmlns"].Value);
                //        //    book1.SetAttribute("Condition", "'$(Configuration)|$(Platform)' == 'Debug|x64'");
                //        //    InsertBefore.ParentNode.InsertBefore(book1, InsertBefore);
                //        //    XmlElement debugSymbols = doc.CreateElement("DebugSymbols", childNodes.Attributes["xmlns"].Value);
                //        //    debugSymbols.InnerText = propertyGroupDebugAnyCPU.DebugSymbols;
                //        //    book1.AppendChild(debugSymbols);

                //        //    XmlElement OutputPath = doc.CreateElement("OutputPath", childNodes.Attributes["xmlns"].Value);
                //        //    OutputPath.InnerText = propertyGroupDebugAnyCPU.OutputPath.Replace("Debug\\", "x64\\Debug\\");
                //        //    book1.AppendChild(OutputPath);

                //        //    XmlElement DefineConstants = doc.CreateElement("DefineConstants", childNodes.Attributes["xmlns"].Value);
                //        //    DefineConstants.InnerText = "TRACE";
                //        //    book1.AppendChild(DefineConstants);

                //        //    XmlElement DebugType = doc.CreateElement("DebugType", childNodes.Attributes["xmlns"].Value);
                //        //    DebugType.InnerText = propertyGroupDebugAnyCPU.DebugType;
                //        //    book1.AppendChild(DebugType);

                //        //    XmlElement PlatformTarget = doc.CreateElement("PlatformTarget", childNodes.Attributes["xmlns"].Value);
                //        //    PlatformTarget.InnerText = "x64";
                //        //    book1.AppendChild(PlatformTarget);

                //        //    XmlElement ErrorReport = doc.CreateElement("ErrorReport", childNodes.Attributes["xmlns"].Value);
                //        //    ErrorReport.InnerText = propertyGroupDebugAnyCPU.ErrorReport;
                //        //    book1.AppendChild(ErrorReport);

                //        //    XmlElement CodeAnalysisRuleSet = doc.CreateElement("CodeAnalysisRuleSet", childNodes.Attributes["xmlns"].Value);
                //        //    CodeAnalysisRuleSet.InnerText = propertyGroupDebugAnyCPU.CodeAnalysisRuleSet;
                //        //    book1.AppendChild(CodeAnalysisRuleSet);

                //        //    XmlElement Prefer32Bit = doc.CreateElement("Prefer32Bit", childNodes.Attributes["xmlns"].Value);
                //        //    Prefer32Bit.InnerText = propertyGroupDebugAnyCPU.Prefer32Bit;
                //        //    book1.AppendChild(Prefer32Bit);
                //        //    if (!NewPath.Contains(data))
                //        //        NewPath.Add(data);
                //        //}
                //        //if (propertyGroupReleaseAnyCPU != null && !IspropertyGroupReleasex64)
                //        //{
                //        //    XmlElement book1 = doc.CreateElement("PropertyGroup", childNodes.Attributes["xmlns"].Value);
                //        //    book1.SetAttribute("Condition", "'$(Configuration)|$(Platform)' == 'Release|x64'");
                //        //    InsertBefore.ParentNode.InsertBefore(book1, InsertBefore);
                //        //    XmlElement debugSymbols = doc.CreateElement("DebugSymbols", childNodes.Attributes["xmlns"].Value);
                //        //    debugSymbols.InnerText = propertyGroupReleaseAnyCPU.DebugSymbols;
                //        //    book1.AppendChild(debugSymbols);

                //        //    XmlElement OutputPath = doc.CreateElement("OutputPath", childNodes.Attributes["xmlns"].Value);
                //        //    OutputPath.InnerText = propertyGroupReleaseAnyCPU.OutputPath.Replace("Release\\", "x64\\Release\\");
                //        //    book1.AppendChild(OutputPath);

                //        //    XmlElement DefineConstants = doc.CreateElement("DefineConstants", childNodes.Attributes["xmlns"].Value);
                //        //    DefineConstants.InnerText = "TRACE";
                //        //    book1.AppendChild(DefineConstants);

                //        //    XmlElement DebugType = doc.CreateElement("DebugType", childNodes.Attributes["xmlns"].Value);
                //        //    DebugType.InnerText = propertyGroupReleaseAnyCPU.DebugType;
                //        //    book1.AppendChild(DebugType);

                //        //    XmlElement PlatformTarget = doc.CreateElement("PlatformTarget", childNodes.Attributes["xmlns"].Value);
                //        //    PlatformTarget.InnerText = "x64";
                //        //    book1.AppendChild(PlatformTarget);

                //        //    XmlElement ErrorReport = doc.CreateElement("ErrorReport", childNodes.Attributes["xmlns"].Value);
                //        //    ErrorReport.InnerText = propertyGroupReleaseAnyCPU.ErrorReport;
                //        //    book1.AppendChild(ErrorReport);

                //        //    XmlElement CodeAnalysisRuleSet = doc.CreateElement("CodeAnalysisRuleSet", childNodes.Attributes["xmlns"].Value);
                //        //    CodeAnalysisRuleSet.InnerText = propertyGroupReleaseAnyCPU.CodeAnalysisRuleSet;
                //        //    book1.AppendChild(CodeAnalysisRuleSet);

                //        //    XmlElement Prefer32Bit = doc.CreateElement("Prefer32Bit", childNodes.Attributes["xmlns"].Value);
                //        //    Prefer32Bit.InnerText = propertyGroupReleaseAnyCPU.Prefer32Bit;
                //        //    book1.AppendChild(Prefer32Bit);
                //        //    if (!NewPath.Contains(data))
                //        //        NewPath.Add(data);
                //        //}
                //        #endregion

                //        #region 不复制本地
                //        foreach (XmlNode childNode in childNodes.ChildNodes) 
                //        {
                //            if (childNode.Name == "ItemGroup") 
                //            {
                //                foreach (XmlNode item in childNode.ChildNodes)
                //                {
                //                    if ((item.Name == "ProjectReference"|| item.Name == "Reference") && item.Attributes["Include"]?.Value?.Contains("JSON.GTA")==true)
                //                    {
                //                        if (item.ChildNodes.OfType<XmlNode>().FirstOrDefault(o => o.Name == "Private") != null)
                //                        {
                //                            item.ChildNodes.OfType<XmlNode>().FirstOrDefault(o => o.Name == "Private").InnerText = "False";
                //                        }
                //                        else
                //                        {
                //                            XmlElement debugSymbols = doc.CreateElement("Private", childNodes.Attributes["xmlns"].Value);
                //                            debugSymbols.InnerText = "False";
                //                            item.AppendChild(debugSymbols);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        #endregion
                //    }
                //    doc.Save(data);
                //    }
            }
            #endregion

            #region 获取整个项目的一个引用的全部路径     
            Dictionary<string,string> HintPath = new Dictionary<string, string>();
            foreach (var data in datas)
            {
                //var data = "D:\\助手的项目\\GTA\\JSON.GTA.Shopyy\\JSON.GTA.Shopyy.API\\JSON.GTA.Shopyy.API.csproj";
                XmlDocument doc = new XmlDocument();
                doc.Load(data);
                foreach (XmlNode childNodes in doc.ChildNodes)
                {
                    foreach (XmlNode childNode in childNodes.ChildNodes)
                    {
                        if (childNode.Name == "ItemGroup")
                        {
                            if (childNode.ChildNodes.Count > 0)
                                foreach (XmlNode childNode1 in childNode.ChildNodes)
                                {
                                    if (childNode1.Attributes["Include"] != null&& childNode1.Attributes["Include"].Value.Contains("Newtonsoft.Json"))
                                    {
                                        foreach (XmlNode item in childNode1.ChildNodes)
                                        {
                                            if(item.Name== "HintPath")
                                            HintPath.Add(data, item.InnerText);
                                        }
                                    }
                                }
                        }
                    }
                }
            }
            #endregion
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

        private void translate_Click(object sender, RoutedEventArgs e)
        {
            TranslateText translateText = new TranslateText();
            translateText.ShowDialog();
        }

        private void compoundIco_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "请选择一个文件夹";

            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var directoryPath = folderBrowser.SelectedPath;
                List<Bitmap> bitmaps = new List<Bitmap>();

                // 加载所有图片
                foreach (string imagePath in Directory.GetFiles(directoryPath, "*.png"))
                {
                    Bitmap bitmap = new Bitmap(imagePath);

                    bitmaps.Add(bitmap);
                }

                if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 创建ICO文件
                    CreateIconFile(folderBrowser.SelectedPath, bitmaps);
                }
            }

        }

        private static void CreateIconFile(string path, List<Bitmap> bitmaps)
        {
            // 创建一个空的IconGroup结构体
            IconGroup iconGroup = new IconGroup();

            // 添加每个Bitmap到IconGroup
            foreach (Bitmap bitmap in bitmaps)
            {
                AddBitmapToIconGroup(ref iconGroup, bitmap);
            }

            // 将IconGroup保存为ICO文件
            SaveIconGroupToFile(iconGroup, Path.Combine(path, Guid.NewGuid() + ".ico"));
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IconDir
        {
            public ushort Reserved;
            public ushort Type;
            public ushort Count;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IconDirEntry
        {
            public byte Width;
            public byte Height;
            public byte ColorCount;
            public byte Reserved;
            public short Planes;
            public short BitCount;
            public int BytesInRes;
            public int ImageOffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IconGroup
        {
            public IconDir Dir;
            public List<IconDirEntry> Entries;
            public List<byte[]> Data;
        }
        
        public static byte[] BitmapToIconBytes(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap), "The bitmap cannot be null.");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                // 将Bitmap转换为Icon
                using (Icon icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon()))
                {
                    icon.Save(ms);
                }

                // 重置流的位置以便读取整个流
                ms.Position = 0;

                // 从流中读取字节
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);

                return bytes;
            }
        }
        private static void AddBitmapToIconGroup(ref IconGroup iconGroup, Bitmap bitmap)
        {
            if (iconGroup.Entries == null)
            {
                iconGroup.Entries = new List<IconDirEntry>();
                iconGroup.Data = new List<byte[]>();
            }

            byte[] data = BitmapToIconBytes(bitmap);

            IconDirEntry entry = new IconDirEntry
            {
                Width = (byte)bitmap.Width,
                Height = (byte)bitmap.Height,
                ColorCount = 0,
                Reserved = 0,
                Planes = 1,
                BitCount = 32,
                BytesInRes = (int)data.Length,
                ImageOffset = (int)iconGroup.Data.Count * data.Length
            };

            iconGroup.Entries.Add(entry);
            iconGroup.Data.Add(data);
        }

        private static void SaveIconGroupToFile(IconGroup iconGroup, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                iconGroup.Dir.Reserved = 0;
                iconGroup.Dir.Type = 1; // 1 for icons
                iconGroup.Dir.Count = (ushort)iconGroup.Entries.Count;

                // 写入IconDir
                fs.Write(BitConverter.GetBytes(iconGroup.Dir.Reserved), 0, 2);
                fs.Write(BitConverter.GetBytes(iconGroup.Dir.Type), 0, 2);
                fs.Write(BitConverter.GetBytes(iconGroup.Dir.Count), 0, 2);

                // 写入IconDirEntry
                foreach (var entry in iconGroup.Entries)
                {
                    fs.WriteByte(entry.Width);
                    fs.WriteByte(entry.Height);
                    fs.WriteByte(entry.ColorCount);
                    fs.WriteByte(entry.Reserved);
                    fs.Write(BitConverter.GetBytes(entry.Planes), 0, 2);
                    fs.Write(BitConverter.GetBytes(entry.BitCount), 0, 2);
                    fs.Write(BitConverter.GetBytes(entry.BytesInRes), 0, 4);
                    fs.Write(BitConverter.GetBytes(entry.ImageOffset), 0, 4);
                }

                // 写入Bitmap数据
                foreach (var data in iconGroup.Data)
                {
                    fs.Write(data, 0, data.Length);
                }
            }
        }

        private void jsonTransition_Click(object sender, RoutedEventArgs e)
        {
            TranslateText translateText = new TranslateText();
            translateText._translate.Visibility = Visibility.Collapsed;
            translateText.ShowDialog();
        }
    }
}
