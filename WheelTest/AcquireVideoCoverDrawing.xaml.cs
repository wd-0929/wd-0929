using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
    /// AcquireVideoCoverDrawing.xaml 的交互逻辑
    /// </summary>
    public partial class AcquireVideoCoverDrawing : Window
    {
        public AcquireVideoCoverDrawing()
        {
            InitializeComponent();
            GetWin32PicFromVideo();
        }
        //创建显示器的DC
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr CreateDC
        (
              string Driver,   //驱动名称
              string Device,   //设备名称
              string Output,   //无用，可以设定为null
              IntPtr PrintData //任意的打印机数据
        );


        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);//寻找窗口列表中第一个符合指定条件的顶级窗口
                                                                                        //获得待测程序主窗体句柄
        public static IntPtr FindMainWindowHandle(string caption, int delay, int maxTries)
        {
            IntPtr mwh = IntPtr.Zero;
            bool formFound = false;
            int attempts = 0;
            while (!formFound && attempts < maxTries)
            {
                if (mwh == IntPtr.Zero)
                {
                    Console.WriteLine("Form not yet found");
                    Thread.Sleep(delay);
                    ++attempts;
                    mwh = FindWindow(null, caption);
                }
                else
                {
                    Console.WriteLine("Form has been found");
                    formFound = true;
                }
            }
            if (mwh == IntPtr.Zero)
                throw new Exception("Could not find main window");
            else
                return mwh;
        }

        // 获得窗口矩形,包括非客户区
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hWnd, out RECT lpRect);
        // 矩形结构
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;//坐标x
            public int Top;//坐标y
            public int Right;//宽
            public int Bottom;//高
        }

        //从指定源设备上下文到目标设备上下文的像素矩形相对应的颜色数据的位块传输
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern bool BitBlt(
                    IntPtr hdcDest,     //目标设备的句柄
                    int XDest,          //目标对象的左上角X坐标
                    int YDest,          //目标对象的左上角的Y坐标
                    int Width,          //目标对象的宽度
                    int Height,         //目标对象的高度
                    IntPtr hdcScr,      //源设备的句柄
                    int XScr,           //源设备的左上角X坐标
                    int YScr,           //源设备的左上角Y坐标
                    Int32 drRop         //光栅的操作值
        );

        #region 截图
        public void GetWin32PicFromVideo()
        {
            //创建显示器的DC
            IntPtr dcScreen = CreateDC("DISPLAY", null, null, (IntPtr)null);
            IntPtr mwh = FindMainWindowHandle("[F4]载入  [F5]截图", 100, 25);
            //获取应用窗体大小
            GetWindowRect(mwh, out RECT rect);

            //要截取画面的坐标，可以自行调整
            int x = rect.Left + 250;
            int y = rect.Top + 35;

            //836  470,截取大小
            Graphics g1 = Graphics.FromHdc(dcScreen);
            System.Drawing.Image Myimage = new Bitmap(836, 470, g1);
            Graphics g2 = Graphics.FromImage(Myimage);
            IntPtr dc1 = g1.GetHdc();
            IntPtr dc2 = g2.GetHdc();

            BitBlt(dc2, 0, 0, 836, 470, dc1, x, y, 13369376);
            g1.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2);

            //保存图片
            Myimage.Save($@"D:\Videos\img\test.png");

            MessageBox.Show("保存成功！");

        }


        #endregion
    }
}
