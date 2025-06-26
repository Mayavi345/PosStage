using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UIComponent.View
{
    public static class PasswordBoxHelper
    {
        /// <summary>
        /// 取得或設定密碼框的密碼
        /// </summary>
        public static readonly DependencyProperty Password =
             DependencyProperty.RegisterAttached("Password",
             typeof(string), typeof(PasswordBoxHelper),
             new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// 取得或設定是否可以更新文字
        /// </summary>
        private static readonly DependencyProperty IsUpdating =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
            typeof(PasswordBoxHelper));


        private static void OnPasswordPropertyChanged(DependencyObject sender,
          DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;
            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdating);
        }
        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdating, value);
        }
        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(Password, value);
        }
    }
}
