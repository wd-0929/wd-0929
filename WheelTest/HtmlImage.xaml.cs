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
using System.Windows.Shapes;

namespace WheelTest
{
    /// <summary>
    /// HtmlImage.xaml 的交互逻辑
    /// </summary>
    public partial class HtmlImage : Window
    {

        public HtmlImage()
        {
            InitializeComponent();
        }
        public bool IsWaiting
        {
            get { return (bool)GetValue(IsWaitingProperty); }
            set { SetValue(IsWaitingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsWaitingProperty =
            DependencyProperty.Register("IsWaiting", typeof(bool), typeof(HtmlImage), new PropertyMetadata(false));
        public string WaitMessage
        {
            get { return (string)GetValue(WaitMessageProperty); }
            set { SetValue(WaitMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaitMessageProperty =
            DependencyProperty.Register("WaitMessage", typeof(string), typeof(HtmlImage), new PropertyMetadata(string.Empty));
        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(int), typeof(HtmlImage), new PropertyMetadata(50));
        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.Register("Html", typeof(string), typeof(HtmlImage), new PropertyMetadata(OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;
            (d as HtmlImage).ImageHeight = 50;
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                (d as HtmlImage).IsWaiting = true;

                StringBuilder htmlString = new StringBuilder();
                htmlString.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                htmlString.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                htmlString.AppendLine("<head>");
                htmlString.AppendLine("</head>");
                htmlString.AppendLine("<body>");
                htmlString.AppendLine("<div>");
                htmlString.AppendLine(newValue);
                htmlString.AppendLine("</div>");
                htmlString.AppendLine("</body>");
                htmlString.AppendLine("</html>");
                var htmlstring = htmlString.ToString();
                var a = WebSnapshotsHelper.GetWebSiteThumbnail(htmlstring);
                MemoryStream ms = new MemoryStream();
                a.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bytes = ms.GetBuffer();  
                ms.Close();
                var Result = new object[] { bytes, a.Height };
                (d as HtmlImage).HtmlStringImage = bytes;
                (d as HtmlImage).ImageHeight = a.Height;
            }
        }
        public byte[] HtmlStringImage
        {
            get { return (byte[])GetValue(HtmlStringImageProperty); }
            set { SetValue(HtmlStringImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlStringImageProperty =
            DependencyProperty.Register("HtmlStringImage", typeof(byte[]), typeof(HtmlImage), null);
    }
}
