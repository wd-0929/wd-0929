using System.IO;
using System.Text;
using WheelTest.Style;
using System.Runtime.Serialization.Json;

namespace WheelTest.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                bitmap.StreamSource = ms;
                ms.Close();
               var a= Serialize(new SerializeTest() { PreviewImage = bitmap });
            }
            //Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);

            ////设置代理可用
            //rk.SetValue("ProxyEnable", 1);
            ////设置代理IP和端口
            //rk.SetValue("ProxyServer", "127.0.0.1:8888");
            //rk.Close();
            //using (WebClient webClient = new WebClient())
            //{
            //    var webDatas = webClient.DownloadData("https://www.mzfort.com/items/CA29327-CA29328/3.jpg");
            //}
        }
        public static string Serialize<T>(T value)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, value);
                var result = Encoding.UTF8.GetString(ms.ToArray());
                return result;
            }
        }
    }
}
