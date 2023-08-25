using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace WebLoginBLL
{
    public class WebLoginInfoBLL
    {
        //MainLoginSetting = new MainLoginSetting
        //{
        //    AccountName = "cn1541248744bjyk",
        //    AccountPassword = "8Oz67354",
        //    LoginId = "fm-login-id",
        //    LoginPassword = "fm-login-password",
        //    Address = "https://csp.aliexpress.com/apps/home?channelId=36375",
        //    LoginHost = "login.aliexpress.com",
        //    OriginalString = "csp.aliexpress.com"
        //};
        //MainLoginSetting = new MainLoginSetting
        //{
        //    LoginId = "fm-login-id",
        //    LoginPassword = "fm-login-password",
        //    Address = "https://myseller.taobao.com/home.htm/QnworkbenchHome/",
        //    LoginHost = "login.taobao.com",
        //    OriginalString = "myseller.taobao.com"
        //};
        public string _path;
        public MainLoginSetting MainLoginSetting;
        public WebLoginInfoBLL(MainLoginSetting mainLoginSetting)
        {
            MainLoginSetting = mainLoginSetting;
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            var ExecFile = entryAssembly.Location;
            var ExecPath = Path.GetDirectoryName(ExecFile);
            _path = System.IO.Path.Combine(ExecPath, "WebLogin");
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }
        public string GetCookie()
        {
            var cookiePath = System.IO.Path.Combine(_path, "Cookie", MainLoginSetting.OriginalString, "cookie.txt");
            if (System.IO.File.Exists(cookiePath))
            {
                var cookie = System.IO.File.ReadAllText(cookiePath);
                List<string> list = new List<string>();
                var a = JArray.Parse(cookie);
                foreach (var item in a)
                {
                    list.Add($@"{item.Value<string>("Name")}={item.Value<string>("Value")}");
                }
                return string.Join(";", list);
            }
            return null;
        }
        public bool DeleteCookie()
        {
            var cookiePath = System.IO.Path.Combine(_path, "Cookie", MainLoginSetting.OriginalString, "cookie.txt");
            if (System.IO.File.Exists(cookiePath))
            {
                System.IO.File.Delete(cookiePath);
            }
            return true;
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public static void  Init()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            var ExecFile = entryAssembly.Location;
            var ExecPath = Path.GetDirectoryName(ExecFile);
            var path = System.IO.Path.Combine(ExecPath, "WebLogin");
            var FileName = System.IO.Path.Combine(path, "WebLogin.exe");
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                WebClient client = new WebClient();
                Uri uri = new Uri("https://gta-data.oss-cn-zhangjiakou.aliyuncs.com/Packs/WebLogin.zip");

                // Specify a DownloadFileCompleted handler here...

                // Specify a progress notification handler.
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);

                client.DownloadFileAsync(uri, System.IO.Path.Combine(path , "WebLogin.zip"));
            }
            while (ProgressPercentage != 100)
            {
                Thread.Sleep(1000);
                Console.WriteLine(" {0} % complete...", ProgressPercentage);
            };
            UnZip(System.IO.Path.Combine(path  ,"WebLogin.zip"), path);
        }
        /// <summary>  
        /// 解压缩文件(压缩文件中含有子目录)  
        /// </summary>  
        /// <param name="zipfilepath">待解压缩的文件路径</param>  
        /// <param name="unzippath">解压缩到指定目录</param>  
        /// <returns>解压后的文件列表</returns>  
        public static void UnZip(string zipfilepath, string unzippath)
        {
            //解压出来的文件列表  
            //List<string> unzipFiles = new List<string>();

            //检查输出目录是否以“\\”结尾  
            if (unzippath.EndsWith("\\") == false || unzippath.EndsWith(":\\") == false)
            {
                unzippath += "\\";
            }

            ZipInputStream s = new ZipInputStream(File.OpenRead(zipfilepath));
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(unzippath);
                string fileName = Path.GetFileName(theEntry.Name);

                //生成解压目录【用户解压到硬盘根目录时，不需要创建】  
                if (!string.IsNullOrEmpty(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (fileName != String.Empty)
                {
                    //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入  
                    if (theEntry.CompressedSize == 0)
                        continue;
                    //解压文件到指定的目录  
                    directoryName = Path.GetDirectoryName(unzippath + theEntry.Name);
                    //建立下面的目录和子目录  
                    Directory.CreateDirectory(directoryName);

                    //记录导出的文件  
                    //unzipFiles.Add(unzippath + theEntry.Name);

                    FileStream streamWriter = File.Create(unzippath + theEntry.Name);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                    streamWriter.Close();
                }
            }
            s.Close();
            GC.Collect();
        }
        private static int ProgressPercentage;
        private static void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressPercentage = e.ProgressPercentage;
        }
        public void ExecCommand()
        {
            var arguments = JsonConvert.SerializeObject(MainLoginSetting);
            System.IO.File.WriteAllText(System.IO.Path.Combine(_path, "MainLoginSetting.txt"), arguments);
            Process proc = new Process();
            proc.StartInfo.FileName = System.IO.Path.Combine(_path, "WebLogin.exe");
          
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            List<string> errors = new List<string>();
            errors.Add(arguments);
            proc.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                errors.Add(e.Data);
            });
            proc.Start();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
            if (proc.ExitCode != 0)
            {
                throw new Exception("获取cookie失败，详情请查看日志");
            }
        }
    }
}
