using Ookii.Dialogs.Wpf;
using SearchApplication.Files;
using SearchApplication.Results;
using SearchApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        
        private bool _caseSensitive;
        /// <summary>
        /// Diğer Seçenekler - Büyük/Küçük Harf Duyarlı
        /// </summary>
        public bool CaseSensitive
        {
            get => _caseSensitive;
            set => RaisePropertyChanged(ref _caseSensitive, value);
        }       

        private string _startFolder;
        /// <summary>
        /// Klasörü seç
        /// </summary>
        public string StartFolder
        {
            get => _startFolder;
            set => RaisePropertyChanged(ref _startFolder, value);
        }

        private SearchType _searchType;
        public SearchType SearchType
        {
            get => _searchType;
            set => RaisePropertyChanged(ref _searchType, value);
        }

        /// <summary>
        /// Verileri birbirine bağlamak için wpf'de bu metot tercih edilir.
        /// </summary>
        public ObservableCollection<ResultItemViewModel> Results { get; set; }

        // ICommand MainWindowda tnaımlanan buton gibi değişkenlerin ViewModel ile bağlantı kurmasını sağlar.

        /// <summary>
        /// Aramayı Başlat Butonu
        /// </summary>
        public ICommand SearchCommand { get; } 

        /// <summary>
        /// İptal Butonu
        /// </summary>
        public ICommand CancelSearchCommand { get; }

        /// <summary>
        /// ... Butonu klasör seçmek için
        /// </summary>
        public ICommand SelectStartFolderCommand { get; }

        /// <summary>
        /// Dosyanın Yolunu Kopyala Butonu
        /// </summary>
        public ICommand ExportResultsCommand { get; }

        /// <summary>
        /// Temizle Butonu
        /// </summary>
        public ICommand ClearResultsCommand { get; }

        public MainViewModel()
        {
            Results=new ObservableCollection<ResultItemViewModel>();
            Results.Add(new ResultItemViewModel()
            {
                FileName = "deneme dosyası.txt",
                FilePath = @"C:\Users\HACER\Universite\7.YARIYIL\Bitirme Projesi 1\deneme dosyası.txt",
                FileSizeBytes = 123456,
                Image=IconHelper.GetIconOfFile(@"C:\Users\HACER\Universite\7.YARIYIL\Bitirme Projesi 1\deneme dosyası",false,false),
                Selection ="test"
            });

        }

        /// <summary>
        /// Klasörü seç kısmına yazılan yolda dosya aramaya başlaması için gerekli kontroller sağlanır. 
        /// </summary>
        public void SelectStartFolderPath()
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Aramayı başlatmak için bir klasör seçiniz.";
            if (fbd.ShowDialog() == true)
            {
                if (fbd.SelectedPath.IsDirectory())
                    StartFolder = fbd.SelectedPath;
                else
                    MessageBox.Show("Seçilen klasör mevcut değil.");
            }
        }

        public void Clear()
        {
            Results.Clear();
        }




    }
}
