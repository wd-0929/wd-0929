using CefSharp.Wpf;
using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WebLogin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainLoginWindow : Window
    {
        int IntervalMilliseconds = 1000;
        private MainLoginSetting MainLoginSetting;
        private string BaseDirectory;
        private string OriginalStringPath;
        public MainLoginWindow()
        {
            try
            {
                InitializeComponent();
                BaseDirectory = System.IO.Path.Combine(AppContext.BaseDirectory);
                MainLoginSetting = System.IO.File.ReadAllText(System.IO.Path.Combine(BaseDirectory, "MainLoginSetting.txt")).ToJsonData<MainLoginSetting>();
                _cefSharpWPF.Address = MainLoginSetting.Address;
                _cefSharpWPF.IsBrowserInitializedChanged += WebBrowser_IsBrowserInitializedChanged;
                _cefSharpWPF.FrameLoadEnd += WebBrowser_FrameLoadEnd;
                OriginalStringPath = System.IO.Path.Combine(BaseDirectory, "Cookie", MainLoginSetting.OriginalString);
                if (!Directory.Exists(OriginalStringPath))
                    Directory.CreateDirectory(OriginalStringPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task InputPassword(ChromiumWebBrowser webBrowser)
        {
            try
            {
                await Task.Delay(IntervalMilliseconds * 5);
                var frame = webBrowser.GetMainFrame();
                if (frame.IsValid && frame.IsFocused && !string.IsNullOrWhiteSpace(MainLoginSetting.AccountName) && !string.IsNullOrWhiteSpace(MainLoginSetting.AccountPassword))
                {
                    string loginId = MainLoginSetting.LoginId;
                    string loginPassword = MainLoginSetting.LoginPassword;
                    //监控账号密码修改
                    var result = frame.EvaluateScriptAsync("CefSharp.BindObjectAsync('boundAsync');\r\n" +
                                                           $"document.getElementById('{loginId}').onblur = function() {{ boundAsync.setAccount(this.value); }}\r\n" +
                                                           $"document.getElementById('{loginPassword}').onblur = function() {{ boundAsync.setPassword(this.value); }}").Result;
                    //输入账号
                    result = frame.EvaluateScriptAsync($"document.getElementById('{loginId}').focus();").Result;
                    if (result.Success)
                    {
                        Task.Delay(IntervalMilliseconds).Wait();
                        foreach (var item in MainLoginSetting.AccountName)
                        {
                            webBrowser.GetBrowserHost().SendKeyEvent((int)258u, (int)item, 0);
                        }
                    }
                    else
                    {
                        //LogHelper.WriteLog("设置账号输入框焦点出错，出错原因：" + result.Message);
                    }

                    //输入密码
                    result = frame.EvaluateScriptAsync($"document.getElementById('{loginPassword}').focus();").Result;
                    if (result.Success)
                    {
                        Task.Delay(IntervalMilliseconds).Wait();
                        foreach (var item in MainLoginSetting.AccountPassword)
                        {
                            webBrowser.GetBrowserHost().SendKeyEvent((int)258u, (int)item, 0);
                        }
                    }
                    else
                    {
                        //LogHelper.WriteLog("设置密码输入框焦点出错，出错原因：" + result.Message);
                    }

                    //移除显示密码按钮
                    result = frame.EvaluateScriptAsync("Array.from(document.getElementsByClassName('comet-input-suffix')).forEach(function(element,index,array){ element.remove(); });").Result;
                    if (!result.Success)
                    {
                        //LogHelper.WriteLog("移除显示密码按钮出错，出错原因：" + result.Message);
                    }
                    //Task.Delay(IntervalMilliseconds).Wait();
                    //var script = @"document.getElementsByClassName('comet-btn comet-btn-primary comet-btn-large comet-btn-block login-submit')[0].click(); ";
                    //var a= frame.EvaluateScriptAsync(script);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void WebBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            try
            {
                if (e.Frame.IsMain)
                {
                    if (Uri.TryCreate(e.Browser.MainFrame.Url, UriKind.Absolute, out Uri uri))
                    {
                        if (MainLoginSetting.OriginalString == uri.Host.ToLower() && (MainLoginSetting.OriginalAbsolutePath == null || uri.AbsolutePath == MainLoginSetting.OriginalAbsolutePath))
                        {
                            await CheckShopLogin((ChromiumWebBrowser)sender);
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                this.Close();
                            }));
                        }
                        else if (MainLoginSetting.LoginHost == uri.Host.ToLower() && (MainLoginSetting.LoginAbsolutePath == null || uri.AbsolutePath == MainLoginSetting.LoginAbsolutePath))
                        {
                            await InputPassword((ChromiumWebBrowser)sender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task CheckShopLogin(ChromiumWebBrowser sender)
        {
            var cookies = await sender.GetCookieManager().VisitUrlCookiesAsync("https://" + MainLoginSetting.OriginalString, true);
            SetCookie(cookies.ToJsonData());
            return;
        }
        public string GetCookie()
        {
            var psss = System.IO.Path.Combine(OriginalStringPath, "cookie.txt");
            if (System.IO.File.Exists(psss))
                return System.IO.File.ReadAllText(psss);
            return null;
        }
        public void SetCookie(string cookies)
        {
            System.IO.File.WriteAllText(System.IO.Path.Combine(OriginalStringPath, "cookie.txt"), cookies);
        }
        /// <summary>
        /// WebBrowser初始化后加载Cookies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var webBrowser = (ChromiumWebBrowser)sender;
                List<Cookie> cks = GetCookie().ToJsonData<List<Cookie>>();
                var cookieManager = webBrowser.GetCookieManager();
                if (cks != null && cks.Count() > 0)
                {
                    foreach (var cookie in cks)
                    {
                        cookieManager.SetCookie("https://" + MainLoginSetting.OriginalString, cookie);
                    }
                }
                else
                {
                    cookieManager.DeleteCookies();
                }
            }
        }
    }
}
