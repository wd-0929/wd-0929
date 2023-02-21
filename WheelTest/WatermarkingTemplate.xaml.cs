using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WheelTest
{
    /// <summary>
    /// WatermarkingTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class WatermarkingTemplate : Window
    {
        public AddWatermarkViewModels _viewModels;
        public WatermarkingTemplate()
        {
            InitializeComponent();
            string exampleFile = System.IO.Path.Combine(AppUtility.ExecPath, "Images", "Example.png");
            //if (!System.IO.File.Exists(exampleFile))
            //{
            //    Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            //    dialog.Title = "请选择水印示例图片(建议大小600*450)";
            //    dialog.Filter = "图片文件|*.jpg";
            //    dialog.FilterIndex = 0;
            //    if (dialog.ShowDialog() == true)
            //    {
            //        exampleFile = dialog.FileName;
            //    }
            //    else
            //    {
            //        Close();
            //        return;
            //    }
            //}
            DataContext =_viewModels = new AddWatermarkViewModels(exampleFile,this);
        }
        private void Thumb_DragDelta(object sender, RoutedEventArgs e)
        {
            _viewModels.XPlocation();
            _viewModels.PreviewImages();
        }

        private void Add_Local_Click(object sender, RoutedEventArgs e)
        {
            Uri uri;
            if (SelectImageHelper.SelectLocalImage(out uri))
            {
                _viewModels.WatermarkSetting.DrawImageFile = uri.LocalPath;
                _viewModels.PreviewImages();
            }
        }
        private void Add_Online_Click(object sender, RoutedEventArgs e)
        {
            Uri uri;
            if (SelectImageHelper.SelectOnlineImage(out uri))
            {
                _viewModels.WatermarkSetting.DrawImageFile = uri.AbsoluteUri;
                _viewModels.PreviewImages();
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SearchProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var text = comboBox.SelectedItem.ToString();
            if (!_viewModels.IsTopLeftConvert)
                _topLeft.IsChecked = true;
            if (text == "Text")
            {
                _viewModels.WatermarkSetting.IsDrawImage = false;
                _text.Visibility = Visibility.Visible;
                _image.Visibility = Visibility.Collapsed;
                _viewModels._topLeftConvert();
            }
            else
            {
                _viewModels.WatermarkSetting.IsDrawImage = true;
                _text.Visibility = Visibility.Collapsed;
                _image.Visibility = Visibility.Visible;
                _viewModels.PreviewImages(true);
            }
        }

        private void _topLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModels?.WatermarkSetting != null)
            {
                if (_topLeft.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.TopLeft;
                }
                else if (_topRight.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.TopRight;
                }
                else if (_topMiddle.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.TopMiddle;
                }
                else if (_bottomLeft.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.BottomLeft;
                }
                else if (_bottomRight.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.BottomRight;
                }
                else if (_bottomMiddle.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.BottomMiddle;
                }
                else if (_middleLeft.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.MiddleLeft;
                }
                else if (_middleRight.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.MiddleRight;
                }
                else if (_center.IsChecked == true)
                {
                    _viewModels.WatermarkSetting.Position = WatermarkPosition.Center;
                }
                _viewModels._topLeftConvert();
            }
        }

        public void Preview_Click(object sender, EventArgs e)
        {
            if (_viewModels.IsPreview)
            {
                _viewModels.IsTopLeftConvert = true;
                _topLeft.IsChecked = true;
                _viewModels.IsTopLeftConvert = false;
                _viewModels.XPlocation();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModels.Thumb_DragDelta(null, new DragDeltaEventArgs(0, 0));
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _viewModels.Thumb_DragDelta(sender, e);
        }
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            _viewModels.Preview();
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            _viewModels.WatermarkSetting.DrawText = textBox.Text;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_viewModels != null)
            {
                if (_viewModels.PreviewImage != null)
                {
                    _viewModels.DisplayHeigh = _viewModels.PreviewImage.Height * _viewModels.WatermarkSetting.ScaleRatio * _viewModels.Percent;
                    _viewModels.DisplayWidth = _viewModels.PreviewImage.Width * _viewModels.WatermarkSetting.ScaleRatio * _viewModels.Percent;
                    _viewModels.PreviewImages(true);
                }
                else
                {
                    _viewModels.WatermarkSetting.FontSizeZoom = _viewModels.WatermarkSetting.FontSize * _viewModels.WatermarkSetting.ScaleRatio * _viewModels.Percent;
                }
            }

        }
    }
    public class AddWatermarkViewModels : INotifyPropertyChanged
    {
        #region Static
        #region INotifyPropertyChanged
        [NonSerialized]
        private PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged

        #region IsWaiting Property
        private bool _isWaiting;
        /// <summary>
        /// 是否等待中
        /// </summary>
        public bool IsWaiting
        {
            get
            {
                return _isWaiting;
            }
            set
            {
                if (_isWaiting != value)
                {
                    _isWaiting = value;
                    OnPropertyChanged(nameof(IsWaiting));
                    if (!_isWaiting)
                    {
                        WaitMessage = null;
                    }
                }
            }
        }
        #endregion IsWaiting Property

        #region WaitMessage Property
        private string _waitMessage;
        /// <summary>
        /// 等待消息
        /// </summary>
        public string WaitMessage
        {
            get
            {
                return _waitMessage;
            }
            set
            {
                if (_waitMessage != value)
                {
                    _waitMessage = value;
                    OnPropertyChanged(nameof(WaitMessage));
                }
            }
        }
        #endregion WaitMessage Property

        #region Reproductions Property
        private List<System.Drawing.Size> _reproductions = new List<System.Drawing.Size>()
        {
              new System.Drawing.Size(600,600),
              new System.Drawing.Size(800,800),
              new System.Drawing.Size(1000,1000)
        };
        /// <summary>
        /// 底图集合
        /// </summary>  
        public List<System.Drawing.Size> Reproductions
        {
            get
            {
                return _reproductions;
            }
            set
            {
                if (_reproductions != value)
                {
                    _reproductions = value;
                    OnPropertyChanged(nameof(Reproductions));
                }
            }
        }
        #endregion Reproductions Property

        #region Reproduction Property
        private System.Drawing.Size _reproduction;
        /// <summary>
        /// 选择底图
        /// </summary>
        public System.Drawing.Size Reproduction
        {
            get
            {
                return _reproduction;
            }
            set
            {
                if (_reproduction != value)
                {
                    _reproduction = value;
                    OnPropertyChanged(nameof(Reproduction));
                    WatermarkSetting.ReproductionWidth = Reproduction.Width;
                    WatermarkSetting.ReproductionHeight = Reproduction.Height;
                    SetWidthAHeight();
                    oldSize = value;
                }
            }
        }
        #endregion Reproduction Property

        private System.Drawing.Size oldSize;
        private void SetWidthAHeight()
        {
            bool isMultiply = true;
            double percent = 1;
            if (Reproduction.Width == 600)
            {
                Percent = 1;
            }
            else if (Reproduction.Width == 800)
            {
                Percent = 0.75;
            }
            else if (Reproduction.Width == 1000)
                Percent = 0.6;

            if (oldSize.Width <= 0)
            {
                percent = Percent;

                isMultiply = true;
            }
            else
            {
                if (oldSize.Width > Reproduction.Width)
                {
                    percent = (double)Reproduction.Width / (double)oldSize.Width;
                    isMultiply = false;
                }
                else
                {
                    percent = (double)oldSize.Width / (double)Reproduction.Width;
                    isMultiply = true;
                }
            }


            if (isMultiply)
            {
                DisplayHeigh = DisplayHeigh * percent;
                DisplayWidth = DisplayWidth * percent;
                WatermarkSetting.FontSizeZoom = WatermarkSetting.FontSizeZoom * percent;
                WatermarkSetting.Left = (int)(WatermarkSetting.Left / percent);
                WatermarkSetting.Right = (int)(WatermarkSetting.Right / percent);
                WatermarkSetting.Top = (int)(WatermarkSetting.Top / percent);
                WatermarkSetting.Bottom = (int)(WatermarkSetting.Bottom / percent);
            }
            else
            {
                DisplayHeigh = DisplayHeigh / percent;
                DisplayWidth = DisplayWidth / percent;
                WatermarkSetting.FontSizeZoom = WatermarkSetting.FontSizeZoom / percent;
                WatermarkSetting.Left = (int)(WatermarkSetting.Left * percent);
                WatermarkSetting.Right = (int)(WatermarkSetting.Right * percent);
                WatermarkSetting.Top = (int)(WatermarkSetting.Top * percent);
                WatermarkSetting.Bottom = (int)(WatermarkSetting.Bottom * percent);
            }

        }

        #region FontSizeItems Property
        private float[] _fontSizeItems = { 10, 11, 12, 14, 16, 18, 20, 24, 28, 36, 48, 72 };
        /// <summary>
        /// describe
        /// </summary>
        public float[] FontSizeItems
        {
            get
            {
                return _fontSizeItems;
            }
            set
            {
                if (_fontSizeItems != value)
                {
                    _fontSizeItems = value;
                    OnPropertyChanged(nameof(FontSizeItems));
                }
            }
        }
        #endregion FontSizeItems Property

        #region ScaleRatioItems Property
        private float[] _scaleRatioItems = { 5, 4, 3, 2, 1, 0.9F, 0.8F, 0.7F, 0.6F, 0.5F, 0.4F, 0.3F, 0.2F, 0.1F };
        /// <summary>
        /// describe
        /// </summary>
        public float[] ScaleRatioItems
        {
            get
            {
                return _scaleRatioItems;
            }
            set
            {
                if (_scaleRatioItems != value)
                {
                    _scaleRatioItems = value;
                    OnPropertyChanged(nameof(ScaleRatioItems));
                }
            }
        }
        #endregion ScaleRatioItems Property

        #region OpacityItems Property
        private float[] _opacityItems = { 1, 0.9F, 0.8F, 0.7F, 0.6F, 0.5F, 0.4F, 0.3F, 0.2F, 0.1F };
        /// <summary>
        /// describe
        /// </summary>
        public float[] OpacityItems
        {
            get
            {
                return _opacityItems;
            }
            set
            {
                if (_opacityItems != value)
                {
                    _opacityItems = value;
                    OnPropertyChanged(nameof(OpacityItems));
                }
            }
        }
        #endregion OpacityItems Property

        #region RotateFlipItems Property
        private RotateFlipType[] _rotateFlipItems = { RotateFlipType.RotateNoneFlipNone, RotateFlipType.Rotate90FlipNone, RotateFlipType.Rotate180FlipNone, RotateFlipType.Rotate270FlipNone };
        /// <summary>
        /// describe
        /// </summary>
        public RotateFlipType[] RotateFlipItems
        {
            get
            {
                return _rotateFlipItems;
            }
            set
            {
                if (_rotateFlipItems != value)
                {
                    _rotateFlipItems = value;
                    OnPropertyChanged(nameof(RotateFlipItems));
                }
            }
        }
        #endregion RotateFlipItems Property

        #region PositionItems Property
        private WatermarkPosition[] _positionItems = { WatermarkPosition.TopLeft, WatermarkPosition.TopMiddle, WatermarkPosition.TopRight, WatermarkPosition.MiddleLeft, WatermarkPosition.Center, WatermarkPosition.MiddleRight, WatermarkPosition.BottomLeft, WatermarkPosition.BottomMiddle, WatermarkPosition.BottomRight };
        /// <summary>
        /// describe
        /// </summary>
        public WatermarkPosition[] PositionItems
        {
            get
            {
                return _positionItems;
            }
            set
            {
                if (_positionItems != value)
                {
                    _positionItems = value;
                    OnPropertyChanged(nameof(PositionItems));
                }
            }
        }
        #endregion PositionItems Property

        #region WatermarkTypes Property
        private WatermarkType[] _watermarkType = { WatermarkType.Text, WatermarkType.Image };
        /// <summary>
        /// describe
        /// </summary>  
        public WatermarkType[] WatermarkTypes
        {
            get
            {
                return _watermarkType;
            }
            set
            {
                if (_watermarkType != value)
                {
                    _watermarkType = value;
                    OnPropertyChanged(nameof(WatermarkTypes));
                }
            }
        }
        #endregion WatermarkTypes Property

        #region WatermarkSetting Property
        private WatermarkSetting _watermarkSetting;
        /// <summary>
        /// describe
        /// </summary>
        public WatermarkSetting WatermarkSetting
        {
            get
            {
                return _watermarkSetting;
            }
            set
            {
                if (_watermarkSetting != value)
                {
                    _watermarkSetting = value;
                    OnPropertyChanged(nameof(WatermarkSetting));
                }
            }
        }
        #endregion WatermarkSetting Property

        #region PreviewImage Property
        private System.Windows.Media.Imaging.BitmapImage _previewImage;

        public System.Windows.Media.Imaging.BitmapImage PreviewImage
        {
            get { return _previewImage; }
            set
            {
                if (_previewImage != value)
                {
                    _previewImage = value;
                    OnPropertyChanged("PreviewImage");
                }
            }
        }
        #endregion

        #region ExampleFile Property
        private string _exampleFile;
        /// <summary>
        /// describe
        /// </summary>
        public string ExampleFile
        {
            get
            {
                return _exampleFile;
            }
            set
            {
                if (_exampleFile != value)
                {
                    _exampleFile = value;
                    OnPropertyChanged(nameof(ExampleFile));
                }
            }
        }
        #endregion ExampleFile Property

        #region Angle Property
        private double _angle;
        /// <summary>
        /// 旋转度数
        /// </summary>
        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    OnPropertyChanged(nameof(Angle));
                }
            }
        }
        #endregion Angle Property

        #region CenterX Property
        private double _centerX;
        /// <summary>
        /// 旋转x
        /// </summary>
        public double CenterX
        {
            get
            {
                return _centerX;
            }
            set
            {
                if (_centerX != value)
                {
                    _centerX = value;
                    OnPropertyChanged(nameof(CenterX));
                }
            }
        }
        #endregion CenterX Property

        #region CenterY Property
        private double _centerY;
        /// <summary>
        /// CenterY
        /// </summary>  
        public double CenterY
        {
            get
            {
                return _centerY;
            }
            set
            {
                if (_centerY != value)
                {
                    _centerY = value;
                    OnPropertyChanged(nameof(CenterY));
                }
            }
        }
        #endregion CenterY Property

        #region DisplayWidth Property
        private double _displayWidth;
        /// <summary>
        /// describe
        /// </summary>
        public double DisplayWidth
        {
            get
            {
                return _displayWidth;
            }
            set
            {
                if (_displayWidth != value)
                {
                    _displayWidth = value;
                    OnPropertyChanged(nameof(DisplayWidth));
                }
            }
        }
        #endregion DisplayWidth Property

        #region DisplayHeigh Property
        private double _displayHeigh;
        /// <summary>
        /// describe
        /// </summary>
        public double DisplayHeigh
        {
            get
            {
                return _displayHeigh;
            }
            set
            {
                if (_displayHeigh != value)
                {
                    _displayHeigh = value;
                    OnPropertyChanged(nameof(DisplayHeigh));
                }
            }
        }
        #endregion DisplayHeigh Property


        #region Percent Property
        private double _percent;
        /// <summary>
        /// 显示比例
        /// </summary>
        public double Percent
        {
            get
            {
                return _percent;
            }
            set
            {
                if (_percent != value)
                {
                    _percent = value;
                    OnPropertyChanged(nameof(Percent));
                }
            }
        }
        #endregion Percent Property




        #endregion

        /// <summary>
        /// 图片显示
        /// </summary>
        public void PreviewImages(bool IsDrawImageCombox = false)
        {
            if (WatermarkSetting.IsDrawImage)
            {
                WaitMessage = "正在加载....";
                IsWaiting = true;
                ThreadProxy.Execute(PreviewImages, WatermarkSetting.DrawImageFile, obj =>
                {
                    WaitMessage = string.Empty;
                    IsWaiting = false;
                    if (obj.HasError)
                    {
                        MessageBox.Show(obj.Error.Message);
                    }
                    else
                    {
                        if (bool.Parse(obj.Tag.ToString()) == true)
                        {
                            XPlocation();
                        }
                    }
                    IsTopLeftConvert = false;
                }, IsDrawImageCombox);
            }
        }
        public void PreviewImages(string filePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    System.Drawing.Image image = null;
                    Uri uri = new Uri(filePath);
                    if (uri.IsLoopback)
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            image = SelectImageHelper.GetImage(filePath);
                        }
                    }
                    else
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            image = System.Drawing.Image.FromStream(new MemoryStream(webClient.DownloadData(filePath)));
                        }
                    }
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            using (System.Drawing.Image img = image.SetImageBackground(System.Drawing.Color.White))
                            {
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = ms;
                            bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            ms.Close();
                            PreviewImage = bitmap;
                            DisplayHeigh = image.Height * WatermarkSetting.ScaleRatio * Percent;
                            DisplayWidth = image.Width * WatermarkSetting.ScaleRatio * Percent;
                        }
                    }));
                    while (true)
                    {
                        if (AddWatermark._thumb.ActualWidth > 0)
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private Watermarker _water;
        public bool IsPreview = true;
        private WatermarkingTemplate AddWatermark;
        public bool IsTopLeftConvert = true;
        public AddWatermarkViewModels(string exampleFile,WatermarkingTemplate addWatermark)
        {
            AddWatermark = addWatermark;
            WatermarkSetting = new WatermarkSetting();
            var _watermarkSetting = new WatermarkSetting();

            Reproduction = Reproductions.FirstOrDefault(o => o.Width == _watermarkSetting.ReproductionWidth && o.Height == _watermarkSetting.ReproductionHeight);

            WatermarkSetting = _watermarkSetting;

            if (WatermarkSetting.IsDrawImage)
                addWatermark._combexWatermark.SelectedIndex = 1;
            else
                addWatermark._combexWatermark.SelectedIndex = 0;
            _water = new Watermarker(exampleFile,  WatermarkSetting);


            switch (WatermarkSetting.Position)
            {
                case WatermarkPosition.TopLeft:
                    addWatermark._topLeft.IsChecked = true;
                    break;
                case WatermarkPosition.TopRight:
                    addWatermark._topRight.IsChecked = true;
                    break;
                case WatermarkPosition.TopMiddle:
                    addWatermark._topMiddle.IsChecked = true;
                    break;
                case WatermarkPosition.BottomLeft:
                    addWatermark._bottomLeft.IsChecked = true;
                    break;
                case WatermarkPosition.BottomRight:
                    addWatermark._bottomRight.IsChecked = true;
                    break;
                case WatermarkPosition.BottomMiddle:
                    addWatermark._bottomMiddle.IsChecked = true;
                    break;
                case WatermarkPosition.MiddleLeft:
                    addWatermark._middleLeft.IsChecked = true;
                    break;
                case WatermarkPosition.MiddleRight:
                    addWatermark._middleRight.IsChecked = true;
                    break;
                case WatermarkPosition.Center:
                    addWatermark._center.IsChecked = true;
                    break;
            }

            ExampleFile = exampleFile;
        }
        public void Preview()
        {
            switch (WatermarkSetting.RotateFlip)
            {
                case RotateFlipType.RotateNoneFlipNone:
                    Angle = 0;
                    break;
                case RotateFlipType.Rotate90FlipNone:
                    Angle = 90;
                    break;
                case RotateFlipType.Rotate180FlipNone:
                    Angle = 180;
                    break;
                case RotateFlipType.Rotate270FlipNone:
                    Angle = 270;
                    break;
            }
            CenterX = AddWatermark._thumb.ActualWidth / 2;
            CenterY = AddWatermark._thumb.ActualHeight / 2;
        }
        public void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Preview();
            AddWatermark._topLeft.IsChecked = true;
            Thumb myThumb = AddWatermark._thumb;

            var ActualWidth = myThumb.ActualWidth;
            var ActualHeight = myThumb.ActualHeight;
            var gHeight = AddWatermark.g.Height;
            var gWidth = AddWatermark.g.Width;

            if (WatermarkSetting.RotateFlip == RotateFlipType.RotateNoneFlipNone || WatermarkSetting.RotateFlip == RotateFlipType.Rotate180FlipNone)
            {
                double nTop = Canvas.GetTop(myThumb) + e.VerticalChange;
                double nLeft = Canvas.GetLeft(myThumb) + e.HorizontalChange;
                if (WatermarkSetting.RotateFlip == RotateFlipType.Rotate180FlipNone)
                {
                    nTop = Canvas.GetTop(myThumb) - e.VerticalChange;
                    nLeft = Canvas.GetLeft(myThumb) - e.HorizontalChange;
                }
                //防止Thumb控件被拖出容器。
                if (myThumb.ActualHeight < AddWatermark.g.Height)
                {
                    if (nTop <= 0)
                        nTop = 0;
                    if (nTop >= (gHeight - ActualHeight))
                        nTop = gHeight - ActualHeight;
                }
                if (myThumb.ActualWidth < AddWatermark.g.Width)
                {
                    if (nLeft <= 0)
                        nLeft = 0;
                    if (nLeft >= (gWidth - ActualWidth))
                        nLeft = gWidth - ActualWidth;
                }
                Canvas.SetTop(myThumb, nTop);
                Canvas.SetLeft(myThumb, nLeft);

                IsPreview = false;
                WatermarkSetting.Top = (int)nTop;
                WatermarkSetting.Left = (int)nLeft;
                WatermarkSetting.Bottom = (int)(gHeight - ActualHeight - nTop);
                WatermarkSetting.Right = (int)(gWidth - ActualWidth - nLeft);
                IsPreview = true;
            }
            else
            {
                double nTop = Canvas.GetTop(myThumb) - e.HorizontalChange;
                double nLeft = Canvas.GetLeft(myThumb) + e.VerticalChange;
                if (WatermarkSetting.RotateFlip == RotateFlipType.Rotate90FlipNone)
                {
                    nTop = Canvas.GetTop(myThumb) + e.HorizontalChange;
                    nLeft = Canvas.GetLeft(myThumb) - e.VerticalChange;
                }
                //防止Thumb控件被拖出容器。
                if (myThumb.ActualHeight < AddWatermark.g.Height)
                {
                    if (nTop <= ((ActualWidth - ActualHeight) / 2))
                        nTop = ((ActualWidth - ActualHeight) / 2);
                    if (nTop >= gHeight - ((ActualWidth + ActualHeight) / 2))
                        nTop = gHeight - ((ActualWidth + ActualHeight) / 2);
                }
                if (myThumb.ActualWidth < AddWatermark.g.Width)
                {
                    if (nLeft <= -((ActualWidth - ActualHeight) / 2))
                        nLeft = -((ActualWidth - ActualHeight) / 2);
                    if (nLeft >= (gWidth - (ActualWidth + ActualHeight) / 2))
                        nLeft = (gWidth - (ActualWidth + ActualHeight) / 2);
                }
                Canvas.SetTop(myThumb, nTop);
                Canvas.SetLeft(myThumb, nLeft);
                IsPreview = false;
                WatermarkSetting.Top = (int)(nTop - ((ActualWidth - ActualHeight) / 2));
                WatermarkSetting.Left = (int)(nLeft + ((ActualWidth - ActualHeight) / 2));
                WatermarkSetting.Bottom = (int)(gHeight - (nTop + (ActualWidth + ActualHeight) / 2));
                WatermarkSetting.Right = (int)(gHeight - (WatermarkSetting.Left + ActualHeight));
                IsPreview = true;
            }

            IsPreview = false;
            WatermarkSetting.Top = (int)(WatermarkSetting.Top / Percent);
            WatermarkSetting.Left = (int)(WatermarkSetting.Left / Percent);
            WatermarkSetting.Bottom = (int)(WatermarkSetting.Bottom / Percent);
            WatermarkSetting.Right = (int)(WatermarkSetting.Right / Percent);
            IsPreview = true;
        }
        /// <summary>
        /// 像素来确定位置
        /// </summary>
        public void XPlocation()
        {
            Preview();

            Thumb myThumb = AddWatermark._thumb;

            var ActualWidth = myThumb.ActualWidth;
            var ActualHeight = myThumb.ActualHeight;
            int top = (int)(WatermarkSetting.Top * Percent);
            int left = (int)(WatermarkSetting.Left * Percent);
            if (WatermarkSetting.RotateFlip == RotateFlipType.RotateNoneFlipNone || WatermarkSetting.RotateFlip == RotateFlipType.Rotate180FlipNone)
            {
                Canvas.SetTop(myThumb, top);
                Canvas.SetLeft(myThumb, left);
            }
            else
            {
                Canvas.SetTop(myThumb, top + ((ActualWidth - ActualHeight) / 2));
                Canvas.SetLeft(myThumb, left - ((ActualWidth - ActualHeight) / 2));
            }
        }
        public void _topLeftConvert()
        {
            if (!IsTopLeftConvert)
            {
                double Top = 0;
                double Left = 0;
                switch (WatermarkSetting.Position)
                {
                    case WatermarkPosition.TopLeft:
                        break;
                    case WatermarkPosition.TopRight:
                        Left = (AddWatermark.g.ActualWidth - AddWatermark._thumb.ActualWidth);
                        break;
                    case WatermarkPosition.TopMiddle:
                        Left = (AddWatermark.g.ActualWidth - AddWatermark._thumb.ActualWidth) / 2;
                        break;
                    case WatermarkPosition.BottomLeft:
                        Top = (AddWatermark.g.ActualHeight - AddWatermark._thumb.ActualHeight);
                        break;
                    case WatermarkPosition.BottomRight:
                        Top = (AddWatermark.g.ActualHeight - AddWatermark._thumb.ActualHeight);
                        Left = (AddWatermark.g.ActualWidth - AddWatermark._thumb.ActualWidth);
                        break;
                    case WatermarkPosition.BottomMiddle:
                        Top = (AddWatermark.g.ActualHeight - AddWatermark._thumb.ActualHeight);
                        Left = (AddWatermark.g.ActualWidth - AddWatermark._thumb.ActualWidth) / 2;
                        break;
                    case WatermarkPosition.MiddleLeft:
                        Top = (AddWatermark.g.ActualHeight - AddWatermark._thumb.ActualHeight) / 2;
                        break;
                    case WatermarkPosition.MiddleRight:
                        Top = (AddWatermark.g.ActualHeight - AddWatermark._thumb.ActualHeight) / 2;
                        Left = (AddWatermark.g.ActualWidth - AddWatermark._thumb.ActualWidth);
                        break;
                    case WatermarkPosition.Center:
                        Top = (AddWatermark.g.ActualHeight - AddWatermark._thumb.ActualHeight) / 2;
                        Left = (AddWatermark.g.ActualWidth - AddWatermark._thumb.ActualWidth) / 2;
                        break;
                }
                var ActualWidth = AddWatermark._thumb.ActualWidth;
                var ActualHeight = AddWatermark._thumb.ActualHeight;
                var gHeight = AddWatermark.g.Height;
                var gWidth = AddWatermark.g.Width;
                IsPreview = false;
                if (WatermarkSetting.RotateFlip == RotateFlipType.RotateNoneFlipNone || WatermarkSetting.RotateFlip == RotateFlipType.Rotate180FlipNone)
                {
                    WatermarkSetting.Top = (int)Top;
                    WatermarkSetting.Left = (int)Left;
                    WatermarkSetting.Bottom = (int)(gHeight - ActualHeight - Top);
                    WatermarkSetting.Right = (int)(gWidth - ActualWidth - Left);
                }
                else
                {
                    WatermarkSetting.Top = (int)(Top - ((ActualWidth - ActualHeight) / 2));
                    WatermarkSetting.Left = (int)(Left + ((ActualWidth - ActualHeight) / 2));
                    WatermarkSetting.Bottom = (int)(gHeight - (Top + (ActualWidth + ActualHeight) / 2));
                    WatermarkSetting.Right = (int)(gHeight - (WatermarkSetting.Left + ActualHeight));
                }
                WatermarkSetting.Top = (int)(WatermarkSetting.Top / Percent);
                WatermarkSetting.Left = (int)(WatermarkSetting.Left / Percent);
                WatermarkSetting.Bottom = (int)(WatermarkSetting.Bottom / Percent);
                WatermarkSetting.Right = (int)(WatermarkSetting.Right / Percent);
                IsPreview = true;
                Canvas.SetTop(AddWatermark._thumb, Top);
                Canvas.SetLeft(AddWatermark._thumb, Left);
                Preview();
            }
            if (!WatermarkSetting.IsDrawImage)
            {
                IsTopLeftConvert = false;
            }
        }
        public void Dispose()
        {
            _water.Dispose();
            PreviewImage = null;
        }
    }
    public enum WatermarkType : byte
    {
        [Display(Name = "文字")]
        Text = 0,
        [Display(Name = "图片")]
        Image = 1,
    }
}
