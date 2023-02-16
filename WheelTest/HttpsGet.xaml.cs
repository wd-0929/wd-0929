using JSON.GTA.ControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
    /// HttpsGet.xaml 的交互逻辑
    /// </summary>
    public partial class HttpsGet : Window
    {
        ViewModel viewModel;
        public HttpsGet()
        {
            InitializeComponent();
            DataContext = viewModel = new ViewModel();
        }

        private void TestRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.HtmlStringImage = viewModel.PostWebRequest();
                viewModel.ErrorText = string.Empty;
            }
            catch (Exception ex)
            {
                viewModel.HtmlStringImage = null;
                viewModel.ErrorText = ex.Message;
            }
        }
        class ViewModel : INotifyPropertyChanged
        {
            #region ImageUrl Property
            private string _imageUrl = "https://fb-es.mrvcdn.com/kf/S4d9d49a574c84454aa0e394923d53417v.jpg";
            /// <summary>
            /// describe
            /// </summary>
            public string ImageUrl
            {
                get
                {
                    return _imageUrl;
                }
                set
                {
                    if (_imageUrl != value)
                    {
                        _imageUrl = value;
                        OnPropertyChanged(nameof(ImageUrl));
                    }
                }
            }
            #endregion ImageUrl Property

            #region ErrorText Property
            private string _errorText;
            /// <summary>
            /// 错误
            /// </summary>
            public string ErrorText 
            {
                get
                {
                    return _errorText;
                }
                set
                {
                    if (_errorText != value)
                    {
                        _errorText = value;
                        OnPropertyChanged(nameof(ErrorText));
                    }
                }
            }
            #endregion ErrorText Property


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

            #region HtmlStringImage Property
            private byte[] _htmlStringImage;
            /// <summary>
            /// describe
            /// </summary>
            public byte[] HtmlStringImage
            {
                get
                {
                    return _htmlStringImage;
                }
                set
                {
                    if (_htmlStringImage != value)
                    {
                        _htmlStringImage = value;
                        OnPropertyChanged(nameof(HtmlStringImage));
                    }
                }
            }
            #endregion HtmlStringImage Property

            private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            }
            public byte[] PostWebRequest()
            {
                try
                {
                    var version = Environment.Version; 
                    if (version.Revision >= 42000)
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)48
                                                    | (SecurityProtocolType)192
                                                    | (SecurityProtocolType)768
                                                    | (SecurityProtocolType)3072;
                    else
                    {
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(192 | 48);
                    }
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(ImageUrl));
                    webReq.ProtocolVersion = HttpVersion.Version11;
                    webReq.KeepAlive = true;
                    webReq.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    webReq.ReadWriteTimeout = 100000;
                    webReq.Timeout = 120000;
                    webReq.Method = "GET";
                    webReq.Accept= "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    //webReq.Headers[HttpRequestHeader.AcceptEncoding]= "gzip, deflate, br";
                    //webReq.Headers[HttpRequestHeader.AcceptEncoding]= "gzip, deflate, br";
                    //webReq.Headers[HttpRequestHeader.AcceptLanguage]= "zh-CN,zh;q=0.9";
                    webReq.Referer= ImageUrl;
                    webReq.Headers.Add("sec-ch-ua", "\";Not A Brand\";v=\"99\", \"Chromium\";v=\"94\"");
                    webReq.Headers.Add("sec-ch-ua-mobile", "?0");
                    webReq.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    //webReq.Headers.Add("sec-fetch-dest", "document");
                    //webReq.Headers.Add("sec-fetch-mode", "navigate");
                    //webReq.Headers.Add("sec-fetch-site", "none");
                    //webReq.Headers.Add("sec-fetch-user", "?1");
                    //webReq.Headers.Add("upgrade-insecure-requests", "1");
                    webReq.UserAgent= "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36 Core/1.94.188.400 QQBrowser/11.4.5226.400";
                    HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                    Stream resultStream = response.GetResponseStream();
                    MemoryStream ms = new MemoryStream();
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int sz = resultStream.Read(buffer, 0, 1024);
                        if (sz == 0) break;
                        ms.Write(buffer, 0, sz);
                    }
                    response.Close();
                    return ms.ToArray();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
