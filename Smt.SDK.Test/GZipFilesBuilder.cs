using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Smt.SDK.Test
{
    public class GZipFilesBuilder
    {   
        private readonly string _gzipPath;
        public GZipFilesBuilder()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            var ExecFile = entryAssembly.Location;
            var ExecPath = Path.GetDirectoryName(ExecFile);
            _gzipPath = Path.Combine(ExecPath, "GZip");
        }

        public string OnBuild()
        {
            var sourcePath = "D:\\自己的项目\\wd-0929\\CollectLogin\\bin\\Debug";
            if (!string.IsNullOrEmpty(sourcePath))
            {

                var targetPath = Path.Combine(_gzipPath, Guid.NewGuid().ToString("N"));
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                var files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);

                List<FileItem> fileList = new List<FileItem>();
                Dictionary<string, FileItem> maps = new Dictionary<string, FileItem>();

                var containExtensions = new List<string>();
                foreach (var file in files)
                {
                    string extension = Path.GetExtension(file).ToLower();
                    if (MatchExtension(extension))
                    {
                        if (!containExtensions.Contains(extension))
                            containExtensions.Add(extension);
                        using (var inputStream = File.OpenRead(file))
                        {
                            var identity = GetFileIdentity(inputStream);
                            var info = new FileItem(identity, GetRelativePath(file, sourcePath), DownloadUriFormat.Replace("[Identity]", identity), inputStream.Length);
                            if (!maps.ContainsKey(info.Identity))
                            {
                                inputStream.Position = 0;
                                using (var outStream = File.Create(Path.Combine(targetPath, info.Identity + ".gzip")))
                                {
                                    PackStream(inputStream, outStream);
                                }

                                maps.Add(info.Identity, info);
                            }
                            fileList.Add(info);
                        }
                    }
                }

                var config = Path.Combine(targetPath, "CollectLogin.json");
                var jsonStirng = JsonConvert.SerializeObject(fileList, new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
                File.WriteAllText(config, jsonStirng, Encoding.UTF8);

                Process.Start(targetPath);

                return string.Join(Environment.NewLine, "生成成功", $"目标文件：{targetPath}", $"包含后缀：{string.Join("|", containExtensions)}");
            }
            else
            {
                return "用户取消，执行中止";
            }
        }

        private bool MatchExtension(string extension)
        {
            if (_searchExtensions?.Count > 0)
            {

                return _searchExtensions.Contains(extension);
            }
            else
            {
                return true;
            }
        }

        #region DownloadUriFormat Property
        private string _downloadUriFormat = "https://gta-data.oss-cn-zhangjiakou.aliyuncs.com/CollectLoginCookie/[Identity].gzip";
        /// <summary>
        /// 下载链接格式
        /// </summary>
        public string DownloadUriFormat
        {
            get
            {
                return _downloadUriFormat;
            }
            set
            {
                if (_downloadUriFormat != value)
                {
                    _downloadUriFormat = value;
                }
            }
        }
        #endregion DownloadUriFormat Property

        #region SearchExtensions Property
        private SortedSet<string> _searchExtensions;
        /// <summary>
        /// 搜索后缀
        /// </summary>
        public string SearchExtensions
        {
            get
            {
                if (_searchExtensions == null)
                    return string.Empty;
                else
                    return string.Join("|", _searchExtensions);
            }
            set
            {
                _searchExtensions = ToSearchExtensions(value);
            }
        }

        private SortedSet<string> ToSearchExtensions(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var datas = value.ToLower().Split(new char[] { ';', '|', ' ', ',', '、' }, StringSplitOptions.RemoveEmptyEntries);

                if (datas.Length > 0)
                {

                    return new SortedSet<string>(datas);
                }
            }
            return null;
        }
        #endregion SearchExtensions Property



        private static void PackStream(FileStream inStream, FileStream outStream)
        {
            using (GZipStream stream = new GZipStream(outStream, CompressionMode.Compress))
            {
                inStream.CopyTo(stream);
                stream.Flush();
            }
        }

        private string GetRelativePath(string file, string path)
        {
            if (file.IndexOf(path, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                return file.Substring(path.Length).TrimStart('\\');
            }
            else
            {
                throw new Exception("当前文件不在当前目录中");
            }
        }

        private static string GetFileIdentity(FileStream stream)
        {
            stream.Position = 0;
            using (var sha256 = SHA256.Create())
            {
                return string.Concat(sha256.ComputeHash(stream).Select(b => b.ToString("x2")));
            }
        }

        public string GetParams()
        {
            return JsonConvert.SerializeObject(new { DownloadUriFormat, SearchExtensions });
        }

        public void SetParams(string paramContents)
        {
            try
            {
                var jData = JsonConvert.DeserializeObject<JObject>(paramContents);
                DownloadUriFormat = jData.Value<string>(nameof(DownloadUriFormat));
                SearchExtensions = jData.Value<string>(nameof(SearchExtensions));
            }
            catch
            {

            }
        }

        class FileItem
        {
            public FileItem(string id, string relativePath, string downloadUri, long fileSize)
            {
                Identity = id;
                RelativePath = relativePath;
                DownloadUri = downloadUri;
                FileSize = fileSize;
            }

            /// <summary>
            /// 唯一标识
            /// </summary>
            public string Identity { get; }

            /// <summary>
            /// 相对路径
            /// </summary>
            public string RelativePath { get; }

            /// <summary>
            /// 下载路径
            /// </summary>
            public string DownloadUri { get; }

            /// <summary>
            /// 文件大小
            /// </summary>
            public long FileSize { get; }
        }
    }
}
    

