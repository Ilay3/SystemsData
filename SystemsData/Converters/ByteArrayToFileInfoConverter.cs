using System;
using System.Globalization;
using System.Windows.Data;

namespace SystemsData.Converters
{
    public class ByteArrayToFileInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                return $"{byteArray.Length} байт";
            }
            return "Нет файла";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
