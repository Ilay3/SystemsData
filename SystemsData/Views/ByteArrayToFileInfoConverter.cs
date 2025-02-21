using System;
using System.Globalization;
using System.Windows.Data;

namespace SystemsData.Views
{
    public class ByteArrayToFileInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] arr && arr.Length > 0)
                return $"Файл загружен ({arr.Length} байт)";
            return "Нет файла";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
