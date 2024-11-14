using SearchApplication.Results;
using SearchApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchApplication.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        private string _searchFor;
        /// <summary>
        /// Dosya Ara
        /// </summary>
        public string SearchFor
        {
            get => _searchFor;
            set => RaisePropertyChanged(ref _searchFor, value);
        }

        private string _selectFolder;
        /// <summary>
        /// Klasörü Seç
        /// </summary>
        public string SelectFolder
        {
            get => _selectFolder;
            set => RaisePropertyChanged(ref _selectFolder, value);
        }

        private SearchType _folderSearch;
        /// <summary>
        /// Arama Türü - Klasör
        /// </summary>
        public SearchType FolderSearch
        {
            get => _folderSearch;
            set => RaisePropertyChanged(ref _folderSearch, value);
        }
        
        private SearchType _fileSearch;
        /// <summary>
        /// Arama Türü - Dosya
        /// </summary>
        public SearchType FileSearch
        {
            get => _fileSearch;
            set => RaisePropertyChanged(ref _fileSearch, value);
        }
        
        private string _caseSensitive;
        /// <summary>
        /// Diğer Seçenekler - Büyük/Küçük Harf Duyarlı
        /// </summary>
        public string CaseSensitive
        {
            get => _caseSensitive;
            set => RaisePropertyChanged(ref _caseSensitive, value);
        }

    }
}
