using SearchApplication.Files;
using SearchApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchApplication.Results
{
    public class ResultItemViewModel : BaseViewModel
    {
        ///Eğer dosyanın resmini de koyacaksan icon türünde tanımlaman gerekiyor.

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => RaisePropertyChanged(ref _fileName, value);
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set => RaisePropertyChanged(ref _filePath, value);
        }

        private long _fileSizeBytes;
        public long FileSizeBytes
        {
            get => _fileSizeBytes;
            set => RaisePropertyChanged(ref _fileSizeBytes, value);
        }

        private string _selection;
        public string Selection
        {
            get => _selection;
            set => RaisePropertyChanged(ref _selection, value);
        }
        public FileType Type { get; set; }

    }
}
