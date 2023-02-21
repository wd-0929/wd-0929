using JSON.GTA.ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WheelTest.Images
{
    /// <summary>
    /// InputDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog()
        {
            IsChanged = false;
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            value.Focus();
        }

        protected bool IsChanged { get; private set; }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (IsChanged && DialogResult != true)
            {
                if (MessageBox.Show("提示", "确定关闭当前窗口吗？", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }

        public string Message
        {
            get
            {
                return message.Text;
            }
            set
            {
                message.Text = value;
            }
        }

        public string Value
        {
            get
            {
                return this.value.Text;
            }
            set
            {
                this.value.Text = value;
            }
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
