using Imazen.WebP;
using JSON.GTA.ControlLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WheelTest
{
    public static class SelectImageHelper
    {
        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color">带#号的16进制颜色</param>
        /// <returns></returns>
        public static Color ToColor(string strHxColor)
        {
            try
            {
                if (strHxColor.Length == 0)
                {//如果为空
                    return System.Drawing.Color.FromArgb(0, 0, 0);//设为黑色
                }
                else
                {//转换颜色
                    return System.Drawing.Color.FromArgb(System.Int32.Parse(strHxColor.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(7, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }
            catch
            {//设为黑色
                return System.Drawing.Color.FromArgb(0, 0, 0);
            }
        }
        public static Bitmap WebpConvrtJpg(byte[] b)
        {
            try
            {
                var det = new SimpleDecoder();
                return det.DecodeFromBytes(b, b.Length);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static Image GetImage(string filePath)
        {
            if (File.Exists(filePath))
            {
                var b = File.ReadAllBytes(filePath);
                try
                {
                    Stream stream = new MemoryStream(b, false);
                    return Image.FromStream(stream);
                }
                catch (Exception ex)
                {
                    return (Image)WebpConvrtJpg(b);
                }
            }
            else
            {
                return default(Image);
            }
        }
        public static Bitmap SetImageBackground(this Image source, Color color)
        {
            Bitmap target = new Bitmap(source.Width, source.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                using (Brush brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, 0, 0, source.Width, source.Height);
                    g.DrawImage(source, 0, 0, source.Width, source.Height);
                }
            }
            return target;
        }
        private static string GetImageExtension(string path)
        {
            try
            {
                Image image = Image.FromFile(path);
                if (image.RawFormat.Guid == ImageFormat.Bmp.Guid) return ".bmp";
                if (image.RawFormat.Guid == ImageFormat.Emf.Guid) return ".wmf";
                if (image.RawFormat.Guid == ImageFormat.Exif.Guid) return ".exif";
                if (image.RawFormat.Guid == ImageFormat.Gif.Guid) return ".gif";
                if (image.RawFormat.Guid == ImageFormat.Icon.Guid) return ".icon";
                if (image.RawFormat.Guid == ImageFormat.Jpeg.Guid) return ".jpg";
                if (image.RawFormat.Guid == ImageFormat.MemoryBmp.Guid) return ".bmp";
                if (image.RawFormat.Guid == ImageFormat.Png.Guid) return ".png";
                if (image.RawFormat.Guid == ImageFormat.Tiff.Guid) return ".tiff";
                if (image.RawFormat.Guid == ImageFormat.Wmf.Guid) return ".wmf";

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static bool SelectLocalImage(out Uri image)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "图像文件(*.jpg;*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.jpeg;*.gif;*.png";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ReadOnlyChecked = true;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true)
            {
                Uri uri;
                if (Uri.TryCreate(dialog.FileName, UriKind.Absolute, out uri))
                {
                    image = uri;
                    return true;
                }
            }
            image = null;
            return false;
        }
        public static bool SelectOnlineImage(out Uri image)
        {
            WheelTest.Images.InputDialog dialog = new WheelTest.Images.InputDialog();
            dialog.Title = "请输入网上图片链接";
            bool result = false;
            image = null;
            if (dialog.ShowDialog() == true)
            {
                IEnumerable<Uri> uris = ConvetToUri(dialog.Value.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));
                if (uris != null && uris.Count() > 0)
                {
                    image = uris.First();
                    result = true;
                }
            }
            dialog = null;
            return result;
        }
        private static IEnumerable<Uri> ConvetToUri(IEnumerable<string> array)
        {
            if (array != null)
            {
                List<Uri> uris = new List<Uri>();
                Uri uri;
                int index = 0;
                foreach (var item in array)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        if (Uri.TryCreate(item.CompletionUri(), UriKind.Absolute, out uri))
                        {
                            uris.Add(uri);
                        }
                    }
                    index++;
                }
                return uris;
            }
            else { return null; }
        }
    }
}
