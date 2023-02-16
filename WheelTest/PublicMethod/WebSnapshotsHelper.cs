using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace WheelTest
{

    public class WebSnapshotsHelper
    {
        Bitmap _bitmap;
        string _htmlText;
        bool _isAotu;
        int _browserWidth, _browserHeight, _thumbnailWidth, _thumbnailHeight;
        public WebSnapshotsHelper(string HtmlText, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            _isAotu = false;
            _htmlText = HtmlText;
            _browserHeight = BrowserHeight;
            _browserWidth = BrowserWidth;
            _thumbnailWidth = ThumbnailWidth;
            _thumbnailHeight = ThumbnailHeight;
        }

        public WebSnapshotsHelper(string HtmlText)
        {
            _htmlText = HtmlText;
            _isAotu = true;
        }
        public static Bitmap GetWebSiteThumbnail(string HtmlText, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            WebSnapshotsHelper thumbnailGenerator = new WebSnapshotsHelper(HtmlText, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
            return thumbnailGenerator.GenerateWebSiteThumbnailImage();
        }

        public static Bitmap GetWebSiteThumbnail(string HtmlText)
        {
            WebSnapshotsHelper thumbnailGenerator = new WebSnapshotsHelper(HtmlText);
            return thumbnailGenerator.GenerateWebSiteThumbnailImage();
        }
        public Bitmap GenerateWebSiteThumbnailImage()
        {
            Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.Start();
            m_thread.Join();
            return _bitmap;
        }
        private void Delay(int Millisecond) //延迟系统时间，但系统又能同时能执行其它任务；
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(Millisecond) > DateTime.Now)
            {
                Application.DoEvents();//转让控制权            
            }
            return;
        }
        private void _GenerateWebSiteThumbnailImage()
        {
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.ScrollBarsEnabled = false;
            Delay(50);
            webBrowser.DocumentText = CreateHtml(_htmlText);
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            while (!WaitWebLoad(webBrowser))
                Application.DoEvents();
            webBrowser.Dispose();
        }

        private bool WaitWebLoad(WebBrowser webBrowser1)
        {
            int i = 0;
            string sUrl;
            while (true)
            {
                Delay(50);//系统延迟50毫秒
                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                {
                    if (!webBrowser1.IsBusy)
                    {
                        i = i + 1;
                        if (i == 2)
                        {
                            sUrl = webBrowser1.Url.ToString();
                            if (sUrl.Contains("res"))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        continue;
                    }
                    i = 0;
                }
            }
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser webBrowser = (WebBrowser)sender;
            int ImageWidth = _thumbnailWidth, ImageHeight = _thumbnailHeight;
            if (_isAotu)
            {
                HtmlElement div = webBrowser.Document.GetElementsByTagName("div")[0];
                if (div != null)
                {
                    ImageWidth = div.OffsetRectangle.Right;
                    ImageHeight = div.OffsetRectangle.Bottom;
                }
            }

            webBrowser.ClientSize = new Size(ImageWidth, ImageHeight);
            webBrowser.ScrollBarsEnabled = false;
            _bitmap = new Bitmap(webBrowser.Bounds.Width, webBrowser.Bounds.Height);
            webBrowser.BringToFront();
            webBrowser.DrawToBitmap(_bitmap, webBrowser.Bounds);
            //Graphics gr = Graphics.FromImage(_bitmap);
            //gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


            //gr.DrawImage(_bitmap, webBrowser.Bounds);
            //gr.Dispose(); 
            //_bitmap = (Bitmap)_bitmap.GetThumbnailImage(_thumbnailWidth, _thumbnailHeight, null, IntPtr.Zero);
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public string CreateHtml(string htmlText)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("<html>");
            str.AppendLine("<body style=\"margin:0px\">");
            str.Append(htmlText);
            str.AppendLine("</body>");
            str.AppendLine("</html>");
            return str.ToString();
        }
    }
}
