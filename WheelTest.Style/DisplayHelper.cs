using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace WheelTest.Style
{

    public static class DisplayHelper
    {
        /// <summary>
        /// 判断该集合是否不为null，并且集合数量大于零
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmptyAndGreaterThanZero<TSource>(this IEnumerable<TSource> source)
        {
            if (source != null && source?.Count() > 0)
            {
                return true;
            }
            return false;
        }
        public static bool TryGetCustomAttribute<T>(Type type, string propertyName, out T attr) where T : Attribute
        {
            MemberInfo[] infos = type.GetMember(propertyName);
            if (infos != null && infos.Length > 0)
            {
                attr = infos[0].GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
            }
            else
            {
                attr = null;
            }
            return attr != null;
        }
        private static string GetDisplayName(Type type, string memberName)
        {
            DisplayAttribute attr;
            if (TryGetCustomAttribute(type, memberName, out attr))
            {
                if (attr.ResourceType != null && !string.IsNullOrEmpty(attr.Name))
                {
                    return GetValue<string>(attr.ResourceType, attr.Name);
                }
                else
                {
                    return attr.GetName();
                }
            }
            else
            {
                return memberName;
            }
        }
        public static string GetDisplayName(this Enum value)
        {
            Type type = value.GetType();
            string name = value.ToString();
            if (name.Contains(", "))
            {
                return string.Join(",", name.Split(',').Select(o => GetDisplayName(type, o.Trim())));
            }
            return GetDisplayName(type, name);
        }
        /// <summary>
        /// 读取数据文件里的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceType"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static T GetValue<T>(Type resourceType, string resourceName)
        {
            if (resourceType == null) throw new ArgumentNullException("resourceType");
            if (string.IsNullOrWhiteSpace(resourceName)) throw new ArgumentNullException("resourceName");

            ResourceManager rm = new ResourceManager(resourceType);
            var result = rm.GetObject(resourceName);
            if (result != null)
            {
                return (T)result;
            }
            else
            {
                throw new Exception(string.Format("未找到资源文件“{0}”下的资源名称“{1}”", resourceType, resourceName));
            }
        }
        /// <summary>
        /// 获取枚举类型的描述
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] memInfo = type.GetMember(enumeration.ToString());
            if (null != memInfo && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (null != attrs && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return enumeration.ToString();
        }


        public static IEnumerable<T> FindChildren<T>(DependencyObject parent) where T : class
        {
            //子元素
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    var t = child as T;
                    if (t != null)
                        yield return t;
                    //递归
                    var children = FindChildren<T>(child);
                    foreach (var item in children)
                        yield return item;
                }
            }
        }
        private static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //是否移动光标
                bool _is = false;
                //选中的TextBox控件
                TextBox text = sender as TextBox;
                Grid grid = VisualTreeHelper.GetParent(text) as Grid;
                var contentcontrol = VisualTreeHelper.GetParent(grid);
                //取到多个文本框的容器
                var stackpanel = VisualTreeHelper.GetParent(contentcontrol);
                //取到多个文本框
                IEnumerable<TextBox> AllButtons = FindChildren<TextBox>(stackpanel);
                foreach (var item in AllButtons)
                {
                    if (_is && item.Name == text.Name)
                    {
                        //移动光标
                        item.Focus();

                        item.SelectionStart = item.Text.Length;
                        item.SelectAll();

                        _is = false;
                    }
                    else
                    {
                        if (item == text)
                        {
                            _is = true;
                        }
                    }
                }
            }
        }

        public static t[] GetValues<t>(this Enum value)
        {
            return Enum.GetValues(typeof(t)).Cast<t>().ToArray();
        }


        public static string GetDisplayDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = value.ToString();

            DisplayAttribute attr;
            if (TryGetCustomAttribute(type, name, out attr))
            {
                if (!string.IsNullOrWhiteSpace(attr.Description))
                {
                    return attr.Description;
                }
            }
            return name;
        }

        public static string GetDisplayShortName(this Enum value)
        {
            Type type = value.GetType();
            string name = value.ToString();

            DisplayAttribute attr;
            if (TryGetCustomAttribute(type, name, out attr))
            {
                return attr.ShortName;
            }
            return name;
        }
    }
}
