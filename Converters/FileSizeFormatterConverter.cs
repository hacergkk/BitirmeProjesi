using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SearchApplication.Converters
{
    //Dosya boyutunu kullanıcının anlayacağı şekilde dönüştürme işlemi yapılır.
    public class FileSizeFormatterConverter : IValueConverter
    {
        /// <summary>
        /// Dosyanın boyunu kullanıcın anlayacağı hale çevirir.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Geçersiz Boyut";

            try
            {
                //Dosya yol olarak gelmişse;
                //Dosya yoluna sahipse ve bu dosya yolu gerçekte varsa
                if (value is string filePath && File.Exists(filePath))
                {
                    //FileInfo dosya hakkında boyutu, create zamanı uzantısı gibi ona ait bilgileri almamızı sağlar.
                    FileInfo fileInfo = new FileInfo(filePath);
                    return GetFormattedSize(fileInfo.Length);
                }
                //dosyanın boyutu bayt cinsinden gelmişse burada çevrilir
                else if (value is long sizeInBytes)
                {
                    return GetFormattedSize(sizeInBytes);
                }
                else
                {
                    throw new ArgumentException("Invalid input type. Must be a file path or size in bytes.");
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (long)0;
        }

        private string GetFormattedSize(long sizeInBytes)
        {
            string[] sizeUnits = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int unitIndex = 0;
            double size = (double)sizeInBytes;

            while (size >= 1024 && unitIndex < sizeUnits.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return $"{size:F2} {sizeUnits[unitIndex]}";
        }
    }
}
