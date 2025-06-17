using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace WheelTest
{

    public class MultiSelectListBox : ListBox
    {
        //static MultiSelectListBox()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectListBox), new FrameworkPropertyMetadata(typeof(MultiSelectListBox)));
        //}

        public MultiSelectListBox()
        {
            SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
        }


        public new IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(MultiSelectListBox), new PropertyMetadata(null, OnSelectedItemsChanged));

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiSelectListBox)d).OnSelectedItems(e.NewValue as IEnumerable, e.OldValue as IEnumerable);
        }

        private void OnSelectedItems(IEnumerable newValue, IEnumerable oldValue)
        {
            if (!_isSeftChanged)
            {
                _isSeftChanged = true;
                base.SelectedItems.Clear();
                if (newValue != null)
                {
                    foreach (var item in newValue)
                    {
                        base.SelectedItems.Add(item);
                    }
                }
                _isSeftChanged = false;
            }
        }
        private bool _isSeftChanged = false;

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (!_isSeftChanged)
            {
                _isSeftChanged = true;

                SetCurrentValue(SelectedItemsProperty, this.GetSelectedItems<object>());
                _isSeftChanged = false;
            }
        }
    }
}
