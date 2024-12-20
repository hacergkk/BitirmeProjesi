using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SearchApplication.Converters
{
    /// <summary>
    /// Bool değeri tam tersine çevirir.
    /// Aramaya başlandığında "Aramayı Başlat" butonunun aktif olmaması için.
    /// </summary>
    public class InvertBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

    }
}