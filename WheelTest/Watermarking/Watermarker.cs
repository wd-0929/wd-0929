using JSON.GTA.ControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WheelTest
{

    #region WatermarkPosition
    public enum WatermarkPosition
    {
        Absolute,
        TopLeft,
        TopRight,
        TopMiddle,
        BottomLeft,
        BottomRight,
        BottomMiddle,
        MiddleLeft,
        MiddleRight,
        Center
    }
    #endregion

    #region Watermarker
    public class Watermarker : IDisposable
    {

        #region Private Fields
        private Image m_image;
        private Image m_originalImage;
        private Image m_watermark;

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the image with drawn watermarks
        /// </summary>
        [Browsable(false)]
        public Image Image { get { return m_image; } }

        /// <summary>
        /// 普通水印
        /// </summary>
        private WatermarkSetting Setting;
        /// <summary>
        /// 营销水印
        /// </summary>
        #endregion

        #region Constructors
        public Watermarker(Image image, WatermarkSetting setting)
        {
            Setting = setting;
            LoadImage(image);
        }
        public Watermarker(string filename, WatermarkSetting setting)
        {
            Setting = setting;
            LoadImage(SelectImageHelper.GetImage(filename));
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Resets image, clearing all drawn watermarks
        /// </summary>
        public void ResetImage()
        {
            if (m_originalImage != null)
            {
                var bitmap = new Bitmap(m_originalImage);
                m_image = bitmap;
            }
        }

        public Image DrawImage(string filename)
        {
            DrawImage(SelectImageHelper.GetImage(filename));
            return Image;
        }

        public Image DrawWatermark(WatermarkSetting setting)
        {
            Setting = setting;
            if (setting.IsDrawImage)
            {
                Uri uri = new Uri(Setting.DrawImageFile);
                if (uri.IsLoopback)
                {
                    if (System.IO.File.Exists(Setting.DrawImageFile))
                    {
                        return DrawImage(Setting.DrawImageFile);
                    }
                }
                else
                {
                    using (WebClient webClient = new WebClient())
                    {
                        DrawImage(Image.FromStream(new MemoryStream(webClient.DownloadData(Setting.DrawImageFile))));
                        return Image;
                    }
                }
                return null;
            }
            else
            {
                return DrawText(Setting.DrawText);
            }
        }
        public static System.Drawing.Bitmap GetProductSize(double width, double height, System.Drawing.Image image)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)width, (int)height);
            //新建一个画板  
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.DrawImage(image, 0, 0, (int)width, (int)height);
            return bitmap;
        }
        public void DrawWatermark(WatermarkSetting setting, string saveFile)
        {
            ImageFormat imgFormat = ImageFormat.Jpeg;
            var extension = System.IO.Path.GetExtension(saveFile).ToLower();
            if (extension == ".png")
            {
                imgFormat = ImageFormat.Png;
            }
            else if (extension == ".gif")
            {
                imgFormat = ImageFormat.Gif;
            }
            else if (extension == ".bmp")
            {
                imgFormat = ImageFormat.Bmp;
            }
            else if (extension == ".tiff")
            {
                imgFormat = ImageFormat.Tiff;
            }
            else if (extension == ".icon")
            {
                imgFormat = ImageFormat.Icon;
            }
            else if (extension == ".wmf")
            {
                imgFormat = ImageFormat.Wmf;
            }
            Setting = setting;
            if (Setting != null)
            {
                Image img = null;
                if (Setting.IsDrawImage)
                {
                    img = DrawImage(Setting.DrawImageFile);
                    //EncoderParameter p;
                    //EncoderParameters ps;
                    //ps = new EncoderParameters(1);
                    //p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                    //ps.Param[0] = p;
                    //ImageCodecInfo ii = GetCodecInfo("image/jpeg");
                    //img.Save(saveFile, ii, ps);
                }
                else
                {
                    img = DrawText(Setting.DrawText);
                    //EncoderParameter p;
                    //EncoderParameters ps;
                    //ps = new EncoderParameters(1);
                    //p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                    //ps.Param[0] = p;
                    //ImageCodecInfo ii = GetCodecInfo("image/jpeg");
                }
                img.Save(saveFile, imgFormat);
            }
        }

        #region 图片

        public static System.Drawing.Image GetPictureImage(string Url)
        {
            using (WebClient webClient = new WebClient())
            {
                var b = webClient.DownloadData(Url);
                try
                {
                    Stream stream = new MemoryStream(b, false);
                    return System.Drawing.Image.FromStream(stream);
                }
                catch (Exception)
                {
                    return (System.Drawing.Image)SelectImageHelper.WebpConvrtJpg(b);
                }
            }
        }
        public static void DrawString(System.Drawing.Graphics g, string text, Font font, SolidBrush solidBrush, float left, float top)
        {
            g.DrawString(text, font, solidBrush, left, top);
        }
        #endregion

        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }
        public void DrawImage(Image watermark)
        {
            if (watermark == null)
                throw new ArgumentOutOfRangeException("Watermark");

            if (Setting.Opacity < 0 || Setting.Opacity > 1)
                throw new ArgumentOutOfRangeException("Opacity");

            if (Setting.ScaleRatio <= 0)
                throw new ArgumentOutOfRangeException("ScaleRatio");

            m_watermark = GetWatermarkImage(watermark);

            //m_watermark.RotateFlip(Setting.RotateFlip);

            Point waterPos = GetWatermarkPosition();

            Rectangle destRect = new Rectangle(waterPos.X, waterPos.Y, m_watermark.Width, m_watermark.Height);

            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][] {
                    new float[] { 1, 0f, 0f, 0f, 0f},
                    new float[] { 0f, 1, 0f, 0f, 0f},
                    new float[] { 0f, 0f, 1, 0f, 0f},
                    new float[] { 0f, 0f, 0f, Setting.Opacity, 0f},
                    new float[] { 0f, 0f, 0f, 0f, 1}
                });

            ImageAttributes attributes = new ImageAttributes();

            attributes.SetColorMatrix(colorMatrix);

            if (Setting.TransparentColor != Color.Empty)
            {
                attributes.SetColorKey(Setting.TransparentColor, Setting.TransparentColor);
            }
            using (Graphics g = Graphics.FromImage(m_image))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(m_watermark, destRect, 0, 0, m_watermark.Width, m_watermark.Height, GraphicsUnit.Pixel, attributes);
                g.Dispose();
            }
        }

        public Image DrawText(string text)
        {
            // Convert text to image, so we can use opacity etc.
            Image textWatermark = GetTextWatermark(text);

            DrawImage(textWatermark);
            return Image;
        }
        #endregion

        #region Private Methods
        private void LoadImage(Image image)
        {
            m_originalImage = image;
            ResetImage();
        }

        private Image GetTextWatermark(string text)
        {
            Font font = Setting.GetFont();
            Color setColor = SelectImageHelper.ToColor(Setting.FontColor);
            Brush brush = new SolidBrush(setColor);
            SizeF size;

            // Figure out the size of the box to hold the watermarked text
            using (Graphics g = Graphics.FromImage(m_image))
            {
                size = g.MeasureString(text, font);
            }

            // Create a new bitmap for the text, and, actually, draw the text
            Bitmap bitmap = new Bitmap((int)size.Width, (int)size.Height);
            bitmap.SetResolution(m_image.HorizontalResolution, m_image.VerticalResolution);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawString(text, font, brush, 0, 0);
            }

            return bitmap;
        }

        private Image GetWatermarkImage(Image watermark)
        {

            watermark.RotateFlip(Setting.RotateFlip);
            Padding margin = Setting.GetMargin();
            // If there are no margins specified and scale ration is 1, no need to create a new bitmap
            if (margin.All == 0 && Setting.ScaleRatio == 1.0f)
                return watermark;

            var Left = margin.Left;
            var Top = margin.Top;
            var Right = margin.Right;
            var Bottom = margin.Bottom;
            int newWidth = Convert.ToInt32(watermark.Width * Setting.ScaleRatio);
            int newHeight = Convert.ToInt32(watermark.Height * Setting.ScaleRatio);
            if (Setting.ReproductionWidth > m_image.Width && Setting.IsDrawImage)
            {
                if ((m_image.Width / Setting.ReproductionWidth) * newWidth > 0 && (m_image.Width / Setting.ReproductionWidth) * newHeight > 0)
                {
                    newWidth = Convert.ToInt32((m_image.Width / Setting.ReproductionWidth) * newWidth);
                    newHeight = Convert.ToInt32((m_image.Width / Setting.ReproductionWidth) * newHeight);
                }
            }
            if (Setting.ReproductionHeight != m_originalImage.Height || Setting.ReproductionWidth != m_originalImage.Width)
            {
                if ((margin.Left + margin.Right) == Setting.ReproductionWidth)
                {
                    if (margin.Left > 0)
                        Left = (int)(((double)margin.Left / (double)(margin.Left + margin.Right)) * (m_originalImage.Width - newWidth));
                    Right = (int)(((double)margin.Right / (double)(margin.Left + Right)) * (m_originalImage.Width - newWidth));
                }
                if ((margin.Top + margin.Bottom) == Setting.ReproductionHeight)
                {
                    if (margin.Top > 0)
                        Top = (int)(((double)margin.Top / (double)(margin.Top + margin.Bottom)) * (m_originalImage.Height - newHeight));
                    Bottom = (int)(((double)margin.Bottom / (double)(Top + margin.Bottom)) * (m_originalImage.Height - newHeight));
                }
            }
            Rectangle sourceRect = new Rectangle(Left, Top, newWidth, newHeight);
            Rectangle destRect = new Rectangle(0, 0, watermark.Width, watermark.Height);
            Bitmap bitmap = new Bitmap(newWidth + Math.Abs(Left) + Math.Abs(Right), newHeight + Math.Abs(Top) + Math.Abs(Bottom));
            bitmap.SetResolution(watermark.HorizontalResolution, watermark.VerticalResolution);
            bitmap.SetImageBackground(Color.White);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(watermark, sourceRect, destRect, GraphicsUnit.Pixel);
                g.Dispose();
            }
            return bitmap;
        }

        private Point GetWatermarkPosition()
        {
            int x = 0;
            int y = 0;

            switch (Setting.Position)
            {
                case WatermarkPosition.TopRight:
                    x = m_image.Width - m_watermark.Width; y = 0;
                    break;
                case WatermarkPosition.TopMiddle:
                    x = (m_image.Width - m_watermark.Width) / 2; y = 0;
                    break;
                case WatermarkPosition.BottomLeft:
                    x = 0; y = m_image.Height - m_watermark.Height;
                    break;
                case WatermarkPosition.BottomRight:
                    x = m_image.Width - m_watermark.Width; y = m_image.Height - m_watermark.Height;
                    break;
                case WatermarkPosition.BottomMiddle:
                    x = (m_image.Width - m_watermark.Width) / 2; y = m_image.Height - m_watermark.Height;
                    break;
                case WatermarkPosition.MiddleLeft:
                    x = 0; y = (m_image.Height - m_watermark.Height) / 2;
                    break;
                case WatermarkPosition.MiddleRight:
                    x = m_image.Width - m_watermark.Width; y = (m_image.Height - m_watermark.Height) / 2;
                    break;
                case WatermarkPosition.Center:
                    x = (m_image.Width - m_watermark.Width) / 2; y = (m_image.Height - m_watermark.Height) / 2;
                    break;
                default:
                    break;
            }

            return new Point(x, y);
        }
        #endregion


        public void Dispose()
        {
            if (m_image != null)
                m_image.Dispose();
            if (m_originalImage != null)
                m_originalImage.Dispose();
            if (m_watermark != null)
                m_watermark.Dispose();
            if (Image != null)
                Image.Dispose();
        }
    }
    #endregion
}
