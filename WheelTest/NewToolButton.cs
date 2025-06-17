using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace WheelTest
{
    [TemplatePart(Name = ProgressElementName, Type = typeof(Rectangle))]
    public class NewToolButton : System.Windows.Controls. MenuItem
    {
        private const string PopupElementName = "popup";
        private const string ProgressElementName = "progressRectangle";

        static NewToolButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NewToolButton), new FrameworkPropertyMetadata(typeof(NewToolButton)));
        }

        public NewToolButton()
             : this(false)
        {

        }
        private void popup_Opened(object sender, EventArgs e)
        {

        }
        public NewToolButton(bool isMore)
        {
        }


        public bool IsLastButton
        {
            get { return (bool)GetValue(IsLastButtonProperty); }
            set { SetValue(IsLastButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLastButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLastButtonProperty =
            DependencyProperty.Register("IsLastButton", typeof(bool), typeof(NewToolButton), new PropertyMetadata(false));



        public UIElement ReferElement
        {
            get { return (UIElement)GetValue(ReferElementProperty); }
            set { SetValue(ReferElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReferElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReferElementProperty =
            DependencyProperty.Register("ReferElement", typeof(UIElement), typeof(NewToolButton), new PropertyMetadata(null));


        private Popup _popupElement;
        public override void OnApplyTemplate()
        {
            if (_popupElement != null)
            {
                _popupElement.Opened -= new EventHandler(_popupElement_Opened);
            }
            _popupElement = GetTemplateChild(PopupElementName) as Popup;
            if (_popupElement != null)
            {
                _popupElement.Opened += new EventHandler(_popupElement_Opened);
            }
            base.OnApplyTemplate();
        }
        private void _popupElement_Opened(object sender, EventArgs e)
        {
            this.Items.Clear();
        }



        public string KeyName
        {
            get { return (string)GetValue(KeyNameProperty); }
            set { SetValue(KeyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsHot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyNameProperty =
            DependencyProperty.Register("KeyName", typeof(string), typeof(NewToolButton), new PropertyMetadata(null));

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NewToolButton();
        }

        public new void OnClick()
        {
            base.OnClick();
        }
    }

}
