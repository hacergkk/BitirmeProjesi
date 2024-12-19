using Ookii.Dialogs.Wpf;
using SearchApplication.Files;
using SearchApplication.Results;
using SearchApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SearchApplication.ViewModels
{
    public class MainViewModel : BaseViewModel
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
        /// Diğer Seçenekler -> Büyük/Küçük Harf Duyarlı
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
        /// Uzantıyı yoksay, kullanıcıya gösterme.
        /// </summary>
        private bool _ignoreExtension;
        public bool IgnoreExtension
        {
            get => _ignoreExtension;
            set => RaisePropertyChanged(ref _ignoreExtension, value);
        }

        //seçilen sonuç
        private ResultItemViewModel _selectedResult;
        public ResultItemViewModel SelectedResult
        {
            get => _selectedResult;
            set => RaisePropertyChanged(ref _selectedResult, value);
        }
        /// <summary>
        /// Verileri birbirine bağlamak için wpf'de bu metot tercih edilir.
        /// </summary>
        public ObservableCollection<ResultItemViewModel> Results { get; set; }

        // ICommand MainWindowda tnaımlanan buton gibi değişkenlerin ViewModel ile bağlantı kurmasını sağlar.

        /// <summary>
        /// Aramayı Başlat Butonu için komutun tanımlanması
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// İptal Butonu için komutun tanımlanması
        /// </summary>
        public ICommand CancelSearchCommand { get; }

        /// <summary>
        /// ... Butonu klasör seçmek için komutun tanımlanması
        /// </summary>
        public ICommand SelectStartFolderPathCommand { get; }

        /// <summary>
        /// Dosyanın Yolunu Kopyala Butonu
        /// </summary>
        public ICommand ExportResultsCommand { get; }

        /// <summary>
        /// Temizle Butonu için komutun tanımlanması
        /// </summary>
        public ICommand ClearResultsCommand { get; }

        public MainViewModel()
        {
            Results = new ObservableCollection<ResultItemViewModel>();
            //bu results'ı mainwindow'da Results.Count olarak bulunan öge sayımında da kullanıyoruz.

            ClearResultsCommand = new Command(Clear);
            SelectStartFolderPathCommand = new Command(SelectStartFolderPath);
            ExportResultsCommand = new Command(ExportResultsToFolder);
            Results.Add(new ResultItemViewModel()
            {
                FileName = "deneme dosyası.txt",
                FilePath = @"C:\Users\HACER\Universite\7.YARIYIL\Bitirme Projesi 1\deneme dosyası.txt",
                FileSizeBytes = 123456,
                Image = IconHelper.GetIconOfFile(@"C:\Users\HACER\Universite\7.YARIYIL\Bitirme Projesi 1\deneme dosyası", false, false),
                Selection = "test"
            });

            Results.Add(new ResultItemViewModel()
            {
                FileName = "deneme dosyası2.txt",
                FilePath = @"C:\Users\HACER\Universite\7.YARIYIL\Bitirme Projesi 1\deneme dosyası.txt",
                FileSizeBytes = 123456,
                Image = IconHelper.GetIconOfFile(@"C:\Users\HACER\Universite\7.YARIYIL\Bitirme Projesi 1\deneme dosyası", false, false),
                Selection = "test"
            });

        }
        public void ExportResultsToFolder()
        {
            if (SelectedResult != null)
            {
                // Seçili dosyanın yolunu kopyala
                Clipboard.SetText(SelectedResult.FilePath);
                MessageBox.Show("Dosya yolu kopyalandı!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Geçerli seçim yoksa uyarı göster
                MessageBox.Show("Lütfen geçerli bir seçim yapın!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        /// <summary>
        /// ...'ya tıklayarak seçilen klasörün dosya yolunun textboxta görünmesi ve atama işlemlerinin sağlanması gerçekleştirilir.
        /// </summary>
        public void SelectStartFolderPath()
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog(); // Klasör seçimi için bir dialog oluşturulur.
            // Dialog başlığı için açıklama ayarlanır.
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Aramayı başlatmak için bir klasör seçiniz.";
            if (fbd.ShowDialog() == true) //Eğer kullanıcı "klasör seç"'e tıklarsa true döner.
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

        /// <summary>
        /// Dosyanın/klasörün path'inden yararlanarak ResultItemViewModel nesnesi üretmek, sonuç 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="selectionText"></param>
        /// <returns></returns>
        public ResultItemViewModel CreateResultFromPath(string path, string selectionText)
        {
            if (path.IsFile()) //eğer dosyaysa
            {
                try
                {
                    FileInfo fInfo = new FileInfo(path);
                    ResultItemViewModel result = new ResultItemViewModel()
                    {
                        Image = IconHelper.GetIconOfFile(path, false, false),
                        FileName = fInfo.Name,
                        FilePath = fInfo.FullName,
                        Selection = selectionText,
                        FileSizeBytes = fInfo.Length,
                        Type = FileType.File
                    };

                    return result;
                }
                catch (Exception e) { MessageBox.Show($"{e.Message}"); return null; }
            }

            else if (path.IsDirectory()) //eğer klasörse
            {
                try
                {
                    DirectoryInfo dInfo = new DirectoryInfo(path);
                    ResultItemViewModel result = new ResultItemViewModel()
                    {
                        Image = IconHelper.GetIconOfFile(path, false, true),
                        FileName = dInfo.Name,
                        FilePath = dInfo.FullName,
                        Selection = selectionText,
                        FileSizeBytes = long.MaxValue,
                        Type = FileType.Folder
                    };

                    return result;
                }
                catch (Exception e) { MessageBox.Show($"{e.Message}"); return null; }
            }

            return null;
        }

        /// <summary>
        /// Sonucun listbox'a eklenmesi için gerekli metodu çağırır.
        /// </summary>
        /// <param name="result"></param>
        public void AddResultAsync(ResultItemViewModel result)
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                Results.Add(result);
            });

            ///Application.Current ile mevcut uygulama nesnesi alır.
            ///Dispatcher -> UI thread'ine güvenli bir şekilde işlem göndermek için kullanılır.             
            ///Doğrudan UI bileşenleri ile etkileşime geçmeye çalışırsa, "Cross-thread operation not valid" 
            ///gibi hatalar alınabileceğinden Dispatcher.Invoke() metodu kullanılır.
        }

        /// <summary>
        /// Bulunan sonuçlar bilgileriyle beraber Results'a yani ListBox'a eklenir. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="selection"></param>
        public void ResultFound(string path, string selection)
        {
            ResultItemViewModel result = CreateResultFromPath(path, selection);
            if (result != null)
                AddResultAsync(result);
        }

        /// <summary>
        /// Dosya adı içerisinde aranılan isim var mı diye kontrol eder.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public bool SearchFileName(string name, string searchText)
        {

            string fPath = CaseSensitive ? name : name.ToLower();
            if (GetFileName(fPath).Contains(searchText))
            {
                ResultFound(fPath, searchText);
                FileSearch++; //Aranan dosya sayısı 1 arttırılır.
                return true;
            }
            FileSearch++; //Aranan dosya sayısı 1 arttırılır.
            return false;
        }

        /// <summary>
        /// Duruma göre dosyanın uzantısını yoksayar.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public string GetFileName(string original)
        {
            if (IgnoreExtension)  // Eğer uzantıyı yok saymak istiyorsak
                return Path.GetFileNameWithoutExtension(original);  // Dosyanın uzantısız adını döndür.
            else  // Aksi halde
                return Path.GetFileName(original);  // Dosyanın adını ve uzantısını döndür.
        }



    }
}
