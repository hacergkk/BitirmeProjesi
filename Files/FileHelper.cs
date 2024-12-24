using SearchApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchApplication.Files
{
    public static class FileHelper
    {
        /// <summary>
        /// Bu path'e sahip dosya var mı?
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFile(this string path)
        {
            return !string.IsNullOrEmpty(path) && File.Exists(path);
        }

        /// <summary>
        /// Bu path'e sahip klasör var mı?
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectory(this string path)
        {
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }

        /// <summary>
        /// Dosyanın yolundan adını getirir.
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string GetFileName(this string fullpath)
        {
            return Path.GetFileName(fullpath);
        }

        /// <summary>
        /// Dosyanın bulunduğu klasörün yolunu getirir.
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string GetParentDirectory(this string fullpath)
        {
            return Path.GetDirectoryName(fullpath);
        }

        /// <summary>
        /// Klasörün adını getirir.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectoryName(this string path)
        {
            return path.IsDirectory() ? new DirectoryInfo(path).Name : "";
        }

    }
}
