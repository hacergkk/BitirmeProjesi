using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SearchApplication.Converters
{
    public class FileNameToExtensionConverter : IValueConverter
    {
        //Dosya adından dosya uzantısını elde etme
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string name ? Path.GetExtension(name) : "Folder";
            //Eğer string tipinde ise uzantısını alır değilse folder yazar
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
