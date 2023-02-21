using Microsoft.WindowsAPICodePack.Shell;
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
            ShellFile shellFile = ShellFile.FromFilePath("C:\\Users\\天命长存\\Desktop\\mda-pa6436sgeqj0b9cg.mp4");
            shellFile.Thumbnail.ExtraLargeBitmap.Save("C:\\Users\\天命长存\\Desktop\\test.jpg");

             
        }
    }
}
