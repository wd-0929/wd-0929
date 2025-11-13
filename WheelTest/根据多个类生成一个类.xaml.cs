using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WheelTest.Style;

namespace WheelTest
{
    /// <summary>
    /// 根据多个类生成一个类.xaml 的交互逻辑
    /// </summary>
    public partial class 根据多个类生成一个类 : Window
    {
        public 根据多个类生成一个类()
        {
            InitializeComponent();
        }
        private static string selectedFolderPath;
        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "选择文件夹"; // 这个技巧可以让OpenFileDialog选择文件夹

            if (dialog.ShowDialog() == true)
            {
                selectedFolderPath = Path.GetDirectoryName(dialog.FileName);
                LoadClassFiles();
            }
        }
        public List<string> classFiles { get; set; } = new List<string>();
        private void LoadClassFiles()
        {
            if (string.IsNullOrEmpty(selectedFolderPath) || !Directory.Exists(selectedFolderPath))
                return;

            // 清空列表

            // 查找常见的类文件扩展名
            string[] extensions = { "*.cs", };
            foreach (string extension in extensions)
            {
                string[] files = Directory.GetFiles(selectedFolderPath, extension, SearchOption.AllDirectories);
                classFiles.AddRange(files);
            }

            var uniqueFields = new List<FieldInfo>();
            var existingFields = new HashSet<string>();

            foreach (var field in classFiles)
            {
               var datas= System.IO.File.ReadAllText(field);
                uniqueFields.AddRange(ExtractCppFields(datas));
            }
            uniqueFields=uniqueFields.DistinctBy(o => o.Name).ToList();
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in uniqueFields)
            {
                stringBuilder.AppendLine(item.AccessModifier);
                stringBuilder.AppendFormat("public {0} {1} ", item.Type, item.Name);
                stringBuilder.Append("{ get; set; }");
                stringBuilder.AppendLine();
            }
            var a = stringBuilder.ToString();
        }
        public int MyProperty { get; set; }
        private List<FieldInfo> ExtractCppFields(string content)
        {
            var fields = new List<FieldInfo>();
            var fieldPattern = @"(?<comment>///[^/]+///[^<]+</summary>[^p]+)?public\s+(?<type>\S+)\s+(?<name>\S+)";

            var matches = Regex.Matches(content, fieldPattern);

            foreach (Match match in matches)
            {
                if (match.Groups["type"].Value != "class")
                {
                    var field = new FieldInfo
                    {
                        Name = match.Groups["name"].Value,
                        Type = match.Groups["type"].Value,
                        AccessModifier = match.Groups["comment"].Value,
                    };

                    fields.Add(field);
                }
            }

            return fields;
        }// FieldInfo.cs
        public class FieldInfo
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string AccessModifier { get; set; }

            public override string ToString()
            {
                return Name.ToString();
            }
        }

    }
}
