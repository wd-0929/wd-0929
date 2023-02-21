using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WheelTest
{

    [Serializable]
    public class WatermarkSetting : INotifyPropertyChanged, IDataErrorInfo
    {
        private float m_opacity = 1.0f;
        private WatermarkPosition m_position = WatermarkPosition.BottomRight;
        private int m_x = 0;
        private int m_y = 0;
        private Color m_transparentColor = Color.Empty;
        private RotateFlipType m_rotateFlip = RotateFlipType.RotateNoneFlipNone;
        //private Padding m_margin = new Padding(0);
        //private Font m_font;
        private string m_fontColor = "Black";
        private float m_scaleRatio = 1.0f;


        public event EventHandler PreviewChanged;
        public void OnPreviewChanged()
        {
            if (PreviewChanged != null)
            {
                PreviewChanged.Invoke(null, EventArgs.Empty);
            }
        }
        public WatermarkSetting()
        {
            FontFamily = System.Drawing.FontFamily.GenericSansSerif.Name;
            FontSize = 24;
            //Font = new Font(new FontFamily(FontFamily), FontSize);
        }

        #region CreateTime Property
        private DateTime _createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
            set
            {
                if (_createTime != value)
                {
                    _createTime = value;
                    OnPropertyChanged(nameof(CreateTime));
                }
            }
        }
        #endregion CreateTime Property

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region TemplateName Property
        private string _templateName;
        /// <summary>
        /// 模板名
        /// </summary>
        public string TemplateName
        {
            get
            {
                return _templateName;
            }
            set
            {
                if (_templateName != value)
                {
                    _templateName = value;
                    OnPropertyChanged(nameof(TemplateName));
                }
            }
        }
        #endregion TemplateName Property


        #region Id Property
        private int _id;
        /// <summary>
        /// id`
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        #endregion Id Property

        #region FontFamily Property
        private string _fontFamily;

        public string FontFamily
        {
            get { return _fontFamily; }
            set
            {
                if (_fontFamily != value)
                {
                    _fontFamily = value;
                    OnPropertyChanged("FontFamily");
                }
            }
        }
        #endregion

        #region FontSize Property
        private float _fontSize;

        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged("FontSize");
                    FontSizeZoom = FontSize * ScaleRatio * 600 / ReproductionWidth;
                }
            }
        }
        #endregion

        #region FontSizeZoom Property
        private double _fontSizeZoom;
        /// <summary>
        /// describe
        /// </summary>
        public double FontSizeZoom
        {
            get
            {
                return _fontSizeZoom;
            }
            set
            {
                if (_fontSizeZoom != value)
                {
                    _fontSizeZoom = value;
                    OnPropertyChanged(nameof(FontSizeZoom));
                }
            }
        }
        #endregion FontSizeZoom Property

        #region Bold Property
        private bool _bold;

        public bool Bold
        {
            get { return _bold; }
            set
            {
                if (_bold != value)
                {
                    _bold = value;
                    OnPropertyChanged("Bold");
                }
            }
        }
        #endregion

        #region Italic Property
        private bool _italic;

        public bool Italic
        {
            get { return _italic; }
            set
            {
                if (_italic != value)
                {
                    _italic = value;
                    OnPropertyChanged("Italic");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        #region Strikeout Property
        private bool _strikeout;

        public bool Strikeout
        {
            get { return _strikeout; }
            set
            {
                if (_strikeout != value)
                {
                    _strikeout = value;
                    OnPropertyChanged("Strikeout");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        #region Underline Property
        private bool _underline;

        public bool Underline
        {
            get { return _underline; }
            set
            {
                if (_underline != value)
                {
                    _underline = value;
                    OnPropertyChanged("Underline");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// Watermark position relative to the image sizes. 
        /// If Absolute is chosen, watermark positioning is being done via PositionX and PositionY 
        /// properties (0 by default)\n
        /// </summary>        
        public WatermarkPosition Position
        {
            get { return m_position; }
            set
            {
                m_position = value; OnPropertyChanged("Position"); ;

            }
        }

        /// <summary>
        /// Watermark X coordinate (works if Position property is set to WatermarkPosition.Absolute)
        /// </summary>
        public int PositionX { get { return m_x; } set { m_x = value; OnPropertyChanged("PositionX"); ; } }

        /// <summary>
        /// Watermark Y coordinate (works if Position property is set to WatermarkPosition.Absolute)
        /// </summary>
        public int PositionY { get { return m_y; } set { m_y = value; OnPropertyChanged("PositionY"); ; } }

        /// <summary>
        /// Watermark opacity. Can have values from 0.0 to 1.0
        /// </summary>
        public float Opacity
        {
            get { return m_opacity; }
            set
            {
                m_opacity = value; OnPropertyChanged("Opacity");
            }
        }

        /// <summary>
        /// Transparent color
        /// </summary>
        public Color TransparentColor { get { return m_transparentColor; } set { m_transparentColor = value; } }

        /// <summary>
        /// Watermark rotation and flipping
        /// </summary>
        public RotateFlipType RotateFlip
        {
            get { return m_rotateFlip; }
            set
            {
                m_rotateFlip = value; OnPropertyChanged("RotateFlip");
            }
        }

        ///// <summary>
        ///// Spacing between watermark and image edges
        ///// </summary>
        //public Padding Margin { get { return m_margin; } set { m_margin = value; } }

        /// <summary>
        /// Watermark scaling ratio. Must be greater than 0. Only for image watermarks
        /// </summary>
        public float ScaleRatio
        {
            get { return m_scaleRatio; }
            set
            {
                m_scaleRatio = value; OnPropertyChanged("ScaleRatio");

            }
        }

        ///// <summary>
        ///// Font of the text to add
        ///// </summary>
        //[NonSerialized]
        //public Font Font;

        /// <summary>
        /// Color of the text to add
        /// </summary>
        public string FontColor
        {
            get { return m_fontColor; }
            set
            {
                m_fontColor = value; OnPropertyChanged("FontColor");
            }
        }

        #region ReproductionWidth Property
        private int _reproductionWidth = 600;
        /// <summary>
        /// 底图宽度
        /// </summary>
        public int ReproductionWidth
        {
            get
            {
                return _reproductionWidth;
            }
            set
            {
                if (_reproductionWidth != value)
                {
                    _reproductionWidth = value;
                    OnPropertyChanged(nameof(ReproductionWidth));
                }
            }
        }
        #endregion ReproductionWidth Property

        #region ReproductionHeight Property
        private int _reproductionHeight = 600;
        /// <summary>
        /// 底图高度
        /// </summary>
        public int ReproductionHeight
        {
            get
            {
                return _reproductionHeight;
            }
            set
            {
                if (_reproductionHeight != value)
                {
                    _reproductionHeight = value;
                    OnPropertyChanged(nameof(ReproductionHeight));
                }
            }
        }
        #endregion ReproductionHeight Property

        #region DrawText Property
        private string _drawText = "";
        /// <summary>
        /// 要绘制的文字
        /// </summary>
        public string DrawText
        {
            get { return _drawText; }
            set
            {
                if (_drawText != value)
                {
                    _drawText = value;
                    OnPropertyChanged("DrawText");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        #region DrawImageFile Property
        private string _drawImageFile = "";
        /// <summary>
        /// 要绘制的图片地址
        /// </summary>
        public string DrawImageFile
        {
            get { return _drawImageFile; }
            set
            {
                if (_drawImageFile != value)
                {
                    _drawImageFile = value;
                    OnPropertyChanged("DrawImageFile");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        #region IsDrawImage Property
        private bool _isDrawImage;

        public bool IsDrawImage
        {
            get { return _isDrawImage; }
            set
            {
                if (_isDrawImage != value)
                {
                    _isDrawImage = value;
                    OnPropertyChanged("IsDrawImage");
                    OnPropertyChanged("DrawImageFile");
                    OnPropertyChanged("DrawText");
                }
            }
        }
        #endregion

        #region Left Property
        private int _left;

        public int Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    OnPropertyChanged("Left");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        #region Right Property
        private int _right;

        public int Right
        {
            get { return _right; }
            set
            {
                if (_right != value)
                {
                    _right = value;
                    OnPropertyChanged("Right");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        #region Top Property
        private int _top;

        public int Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    OnPropertyChanged("Top");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        #region Bottom Property
        private int _bottom;

        public int Bottom
        {
            get { return _bottom; }
            set
            {
                if (_bottom != value)
                {
                    _bottom = value;
                    OnPropertyChanged("Bottom");
                    OnPreviewChanged();
                }
            }
        }
        #endregion

        private bool autoWatermark = false;
        /// <summary>
        /// 自动加水印
        /// </summary>
        public bool AutoWatermark
        {
            get
            {
                return autoWatermark;
            }
            set
            {
                autoWatermark = value;
            }
        }

        #region ImagePecies Property
        private string _imagePecies = "所有图片";

        public string ImagePecies
        {
            get { return _imagePecies; }
            set
            {
                if (_imagePecies != value)
                {
                    _imagePecies = value;
                    OnPropertyChanged("ImagePecies");
                }
            }
        }
        #endregion

        #region ImagePeciess Property
        private string[] _imagePeciess;
        /// <summary>
        /// 水印配置的情况
        /// </summary>
        public string[] ImagePeciess
        {
            get
            {
                return _imagePeciess;
            }
            set
            {
                if (_imagePeciess != value)
                {
                    _imagePeciess = value;
                    OnPropertyChanged(nameof(ImagePeciess));
                }
            }
        }
        #endregion ImagePeciess Property

        /// <summary>f
        /// 保存此设置
        /// </summary>
        /// <param name="settingfile">保存的XML路径</param>
        public void Save(string settingfile)
        {
            if (IsDrawImage)
            {
                if (System.IO.File.Exists(DrawImageFile))
                {
                    string extension = System.IO.Path.GetExtension(DrawImageFile);
                    string markImageFile = "wm" + extension;
                    string saveDir = System.IO.Path.GetDirectoryName(settingfile);
                    string saveFile = System.IO.Path.Combine(saveDir, markImageFile);

                    if (string.Compare(DrawImageFile, saveFile, true) != 0)
                        System.IO.File.Copy(DrawImageFile, saveFile, true);

                    DrawImageFile = saveFile;
                }
            }
            XmlSerializer xs = new XmlSerializer(this.GetType());
            using (Stream stream = new FileStream(settingfile, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                xs.Serialize(stream, this);
            }
        }


        public Font GetFont()
        {
            FontStyle fontStyle = FontStyle.Regular;
            if (Bold)
                fontStyle |= FontStyle.Bold;
            if (Italic)
                fontStyle |= FontStyle.Italic;
            if (Strikeout)
                fontStyle |= FontStyle.Strikeout;
            if (Underline)
                fontStyle |= FontStyle.Underline;

            return new Font(FontFamily, FontSize, fontStyle, GraphicsUnit.Pixel);
        }

        public Padding GetMargin()
        {
            return new Padding(Left, Top, Right, Bottom);
        }

        /// <summary>
        /// 读取设置
        /// </summary>
        /// <param name="settingFile">读取的设置文件</param>
        /// <returns></returns>
        public static WatermarkSetting ReadSetting(string settingFile)
        {
            try
            {
                if (!System.IO.File.Exists(settingFile))
                    return null;
                XmlSerializer xs = new XmlSerializer(typeof(WatermarkSetting));
                Stream stream = new FileStream(settingFile, FileMode.Open, FileAccess.Read, FileShare.Delete);
                WatermarkSetting p = xs.Deserialize(stream) as WatermarkSetting;


                return p;
            }
            catch
            {
                return null;
            }
        }

        public string Error
        {
            get
            {
                List<string> strs = new List<string>();
                if (IsDrawImage)
                {
                    strs.Add(this["DrawImageFile"]);
                }
                else
                {
                    strs.Add(this["DrawText"]);
                }
                strs.Add(this["TemplateName"]);
                strs.Add(this["Top"]);
                strs.Add(this["Left"]);
                strs.Add(this["Right"]);
                strs.Add(this["Bottom"]);
                return string.Join("\r\n", strs.Where(o => !string.IsNullOrWhiteSpace(o)));
            }
        }

        public WatermarkSetting Clone()
        {
            WatermarkSetting watermarkSetting = new WatermarkSetting();
            watermarkSetting.TemplateName = TemplateName.Trim();
            watermarkSetting.Id = Id;
            watermarkSetting.FontFamily = FontFamily;
            watermarkSetting.FontSize = FontSize;
            watermarkSetting.Bold = Bold;
            watermarkSetting.Italic = Italic;
            watermarkSetting.Strikeout = Strikeout;
            watermarkSetting.Underline = Underline;
            watermarkSetting.Position = Position;
            watermarkSetting.Opacity = Opacity;
            watermarkSetting.RotateFlip = RotateFlip;
            watermarkSetting.ScaleRatio = ScaleRatio;
            watermarkSetting.FontColor = FontColor;
            watermarkSetting.DrawText = DrawText;
            watermarkSetting.DrawImageFile = DrawImageFile;
            watermarkSetting.IsDrawImage = IsDrawImage;
            watermarkSetting.ReproductionWidth = ReproductionWidth;
            watermarkSetting.ReproductionHeight = ReproductionHeight;
            watermarkSetting.Left = Left;
            watermarkSetting.Right = Right;
            watermarkSetting.Top = Top;
            watermarkSetting.Bottom = Bottom;
            return watermarkSetting;
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DrawImageFile")
                {
                    if (IsDrawImage && string.IsNullOrEmpty(DrawImageFile))
                    {
                        return "请选择水印图片";
                    }
                }
                else if (columnName == "DrawText")
                {
                    if (!IsDrawImage && string.IsNullOrEmpty(DrawText))
                    {
                        return "请输入水印文字";
                    }
                }
                else if (columnName == "TemplateName")
                {
                    if (string.IsNullOrWhiteSpace(TemplateName))
                    {
                        return "模板名为空";
                    }
                }
                else if (columnName == "Left")
                {
                    if (Left > 200000)
                    {
                        return "左间距最大不能超过200000";
                    }
                }
                else if (columnName == "Right")
                {
                    if (Right > 200000)
                    {
                        return "右间距最大不能超过200000";
                    }
                }
                else if (columnName == "Top")
                {
                    if (Top > 200000)
                    {
                        return "上间距最大不能超过200000";
                    }
                }
                else if (columnName == "Bottom")
                {
                    if (Bottom > 200000)
                    {
                        return "下间距最大不能超过200000";
                    }
                }
                return string.Empty;
            }
        }
    }
}
