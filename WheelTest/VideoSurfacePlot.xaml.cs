using MediaToolkit.Model;
using MediaToolkit;
using System;
using System.Collections.Generic;
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
using MediaToolkit.Options;
using System.IO;

namespace WheelTest
{
    /// <summary>
    /// VideoSurfacePlot.xaml 的交互逻辑
    /// </summary>
    public partial class VideoSurfacePlot : Window
    {
        public VideoSurfacePlot()
        {
            InitializeComponent();
            //ShellFile shellFile = ShellFile.FromFilePath("C:\\Users\\天命长存\\Desktop\\mda-pa6436sgeqj0b9cg.mp4");
            //shellFile.Thumbnail.ExtraLargeBitmap.Save("C:\\Users\\天命长存\\Desktop\\test.jpg");


        }

        public string ExtractSnapshot(string videoFilePath, string outputImagePath, TimeSpan position)
        {
            try
            {
                if (!File.Exists(videoFilePath))
                {
                    Console.WriteLine("文件不存在！！！");
                    return null ;
                }
                // 创建 MediaFile 对象，指定输入和输出文件路径
                var inputFile = new MediaFile { Filename = videoFilePath };
                var outputFile = new MediaFile { Filename = outputImagePath };

                // 创建 ConversionOptions 对象，并设置截图的时间点
                var conversionOptions = new ConversionOptions { Seek = position };

                // 使用Engine类来处理视频截图
                using (var engine = new Engine())
                {
                    // 获取视频文件的元数据信息
                    engine.GetMetadata(inputFile);

                    // 截取指定时间点的视频帧，并保存为图像文件
                    engine.GetThumbnail(inputFile, outputFile, conversionOptions);
                }

                return outputImagePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"视频截图失败：{ex.Message}");
            }
        }
    }
}
