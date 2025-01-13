using Microsoft.JScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZXing;

namespace WheelTest
{
    /// <summary>
    /// QrCodeRecognize.xaml 的交互逻辑
    /// </summary>
    public partial class QrCodeRecognize : Window
    {
        public QrCodeRecognize()
        {
            InitializeComponent();
           var datass= System.IO.File.ReadAllBytes("C:\\Users\\Administrator\\Desktop\\7d9eaee0a49946829ef2ce399f2bcc39.pdf");
            var datas = ExtractQrCodesFromPdf(datass);
        }
        public string ExtractQrCodesFromPdf(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            var pdfDocument = PdfiumViewer.PdfDocument.Load(ms);
            {
                List<string> pdfText = new List<string>();
                for (int page = 0; page < pdfDocument.PageCount; page++)
                {
                    var image = ConvertPageToImage(pdfDocument, page);
                    pdfText.Add(ReadQRCodeFromImage(image));
                }
                return "";
            }
        }
        public static Bitmap CropImage(System.Drawing.Image sourceImage)
        {
            var cropArea = new System.Drawing.Rectangle(22, 70, 750, 110);
            // 创建一个新的Bitmap对象，大小与裁剪区域相同
            Bitmap croppedImage = new Bitmap(cropArea.Width, cropArea.Height);

            // 使用Graphics对象将原始图像的指定区域绘制到新的Bitmap上
            using (Graphics g = Graphics.FromImage(croppedImage))
            {
                g.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, cropArea.Width, cropArea.Height),
                              cropArea, GraphicsUnit.Pixel);
            }

            return croppedImage;
        }
        public static System.Drawing.Bitmap ConvertPageToImage(PdfiumViewer.PdfDocument pdfDocument, int pageNumber)
        {
            int height = (int)(800 * (pdfDocument.PageSizes[0].Height / pdfDocument.PageSizes[0].Width));
            // 创建一个新的Bitmap对象，大小与裁剪区域相同
            Bitmap croppedImage = new Bitmap(761, 250);
            System.Drawing.Image image = pdfDocument.Render(pageNumber, 800, height, 300, 300, true);

            return CropImage(image);
        }
        public static string ReadQRCodeFromImage(System.Drawing.Bitmap bitmap)
        {
            var barcodeReader = new BarcodeReader();
            barcodeReader.Options.PossibleFormats = Enum.GetValues(typeof(BarcodeFormat)).OfType<BarcodeFormat>().ToArray();
            barcodeReader.AutoRotate = true;
            barcodeReader.Options.TryHarder = true;
            barcodeReader.Options.PureBarcode = false;
            var result = barcodeReader.Decode(bitmap);
            if (result != null)
                return result.Text;
            else
            {
                barcodeReader.Options.PossibleFormats = new[]
                    {
                        BarcodeFormat.AZTEC ,
                        BarcodeFormat.CODABAR ,
                        BarcodeFormat.CODE_39 ,
                        BarcodeFormat.CODE_93 ,
                        BarcodeFormat.CODE_128 ,
                        BarcodeFormat.DATA_MATRIX ,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.EAN_13 ,
                        BarcodeFormat.ITF,
                        BarcodeFormat.MAXICODE ,
                        BarcodeFormat.PDF_417 ,
                        BarcodeFormat.QR_CODE ,
                        BarcodeFormat.RSS_14 ,
                        BarcodeFormat.RSS_EXPANDED,
                        BarcodeFormat.UPC_A ,
                        BarcodeFormat.UPC_E ,
                        BarcodeFormat.All_1D,
                        BarcodeFormat.UPC_EAN_EXTENSION,
                        BarcodeFormat.MSI ,
                        BarcodeFormat.PLESSEY,
                        BarcodeFormat.IMB ,
                    };

                // 进行条形码解码
                var barcodes = barcodeReader.Decode(bitmap);
                if (barcodes != null)
                    return barcodes.Text;
            }
            return "";
        }
    }
}
