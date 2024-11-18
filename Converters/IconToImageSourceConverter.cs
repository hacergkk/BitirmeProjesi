using SearchApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SearchApplication.Converters
{
    public class IconToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Icon ico) //value nesnesi'nin Icon türünde olup olmadığı kontrol edilir.
            {
                ImageSource img = ico.ToImageSource(); //Dönüştürülür.
                ico.Dispose(); //Artık kullanılmayacağından nesne temizleme işlemi yapılır.
                return img;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
