using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using SkiaSharp;
using ImageMagick.Drawing;
using ImageMagick;

namespace WheelTest
{
    /// <summary>
    /// imageConvert.xaml 的交互逻辑
    /// </summary>
    public partial class imageConvert : Window
    {
        public imageConvert()
        {
            InitializeComponent();

            using (WebClient webClient = new WebClient())
            {
                var Url = "https://ae01.alicdn.com/kf/S63f9363e64d14230a7b291c27a967095p.jpg";
                var b = webClient.DownloadData(Url);
                using (var ms = new MemoryStream(b))
                {
                   var a= LoadAvifImageFromStream(ms);
                }
            }
        }
        public Bitmap LoadAvifImageFromStream(Stream avifStream)
        {

            using (var image = new MagickImage(avifStream))
            {

                // 调整大小
                image.Resize(300, 200);

                // 添加水印文字
                var drawables = new Drawables();

                drawables.Draw(image);
                // 将 MagickImage 转换为 System.Drawing.Bitmap
                using (var memoryStream = new MemoryStream())
                {
                    // 将 MagickImage 保存为 PNG 格式
                    image.Write(memoryStream, MagickFormat.Png);

                    // 将内存流重置为开头
                    memoryStream.Position = 0;

                    // 从内存流中创建 System.Drawing.Bitmap
                    return new Bitmap(memoryStream);
                }
                // 保存为 PNG
            }
        }
    }
}
