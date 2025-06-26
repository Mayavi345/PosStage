using System;
using System.Globalization;
using System.Windows.Data;
using Utilities;

namespace UIComponent.Converter
{
    public class PasswordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string password = value as string;

            if (!string.IsNullOrEmpty(password))
            {
                return TextHelper.ChageStringToPassword(password);

            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
