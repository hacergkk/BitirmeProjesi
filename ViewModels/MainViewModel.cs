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

        private int _folderSearched;
        /// <summary>
        /// Arama Türü - Klasör
        /// </summary>
        public int FolderSearched
        {
            get => _folderSearched;
            set => RaisePropertyChanged(ref _folderSearched, value);
        }

        private int _fileSearched;
        /// <summary>
        /// Arama Türü - Dosya
        /// </summary>
        public int FileSearched
        {
            get => _fileSearched;
            set => RaisePropertyChanged(ref _fileSearched, value);
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
        /// Klasörü seç-başlangıç klasörü
        /// </summary>
        public string StartFolder
        {
            get => _startFolder;
            set => RaisePropertyChanged(ref _startFolder, value);
        }

        private SearchType _searchOption;
        public SearchType SearchOption
        {
            get => _searchOption;
            set => RaisePropertyChanged(ref _searchOption, value);
        }        

        //seçilen sonuç değerini tutar.
        private ResultItemViewModel _selectedResult;
        public ResultItemViewModel SelectedResult
        {
            get => _selectedResult;
            set => RaisePropertyChanged(ref _selectedResult, value);
        }

        /// <summary>
        /// İptal'e basıldığında aramanın durdurulması için tanımlanmıştır.
        /// Aramaya başlandığında bu değer InvertBoolConverter sayesinde true yapılır. 
        /// İptal'e tıklanıldığında false yapılır.
        /// </summary>
        private bool _isSearching;
        public bool IsSearching
        {
            get => _isSearching;
            set => RaisePropertyChanged(ref _isSearching, value);
        }
        private bool _searchRecursive;
        /// <summary>
        /// Kapsamlı arama seçilmiştir. Seçilmemişse sadece o klasör incelenilir. Alt klasörler incelenilmez.
        /// </summary>
        public bool SearchRecursive
        {
            get => _searchRecursive;
            set => RaisePropertyChanged(ref _searchRecursive, value);
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
        public ICommand CopyPathCommand { get; }

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
            CopyPathCommand = new Command(CopyPath);
            CancelSearchCommand = new Command(CancelSearch);
            SearchCommand = new Command(Find);
        }
        /// <summary>
        /// Dosyanın/klasörün yolunu kopyalamak için kullanılır.
        /// </summary>
        public void CopyPath()
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
        /// <summary>
        /// Sonuçları temizlemek için kullanılır.
        /// </summary>
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
                        FilePath =  dInfo.FullName,
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

            //string fPath = CaseSensitive ? name : name.ToLower();
            //if (GetFileName(fPath).Contains(searchText))
            //{
            //    ResultFound(fPath, searchText);
            //    FileSearched++; //Aranan dosya sayısı 1 arttırılır.
            //    return true;
            //}
            //FileSearched++; //Aranan dosya sayısı 1 arttırılır.
            //return false;

            if (!CaseSensitive)
            {
                string filename = name.GetFileName();
                if (filename.ToLower().Contains(searchText.ToLower()))
                {
                    ResultFound(name, searchText); //Sonuç bulunduğu için listbox'a ekleme metodu çağrılır.
                    FileSearched++; //Aranan klasör sayısı arttırılır.
                    return true;
                }
                FileSearched++; //Eğer bulamamışsa bile o klasörü aradığından; aranan klasör sayısı arttırılır.
                return false;
            }
            else
            {
                if (name.GetFileName().Contains(searchText))
                {
                    ResultFound(name, searchText); //Sonuç bulunduğu için listbox'a ekleme metodu çağrılır.
                    FileSearched++; //Aranan klasör sayısı arttırılır.
                    return true;
                }

                FileSearched++; //Eğer bulamamışsa bile o klasörü aradığından; aranan klasör sayısı arttırılır.
                return false;
            }

        }


        /// <summary>
        /// IsSearching değişkeninin değeri değiştirilir.
        /// </summary>
        /// <param name="isSearching"></param>
        public void SetSearchingStatus(bool isSearching)
        {
            IsSearching = isSearching;
        }

        /// <summary>
        /// Arama durdurulur. İptal butonuna basılmıştır.
        /// </summary>
        public void CancelSearch()
        {
            SetSearchingStatus(false);
        }

        /// <summary>
        /// Recursive arama yapar. Bir klasördeki tüm dosyaları ve alt klasörleri tarar.
        /// </summary>
        /// <param name="searchText"></param>
        public void StartSearchRecursively(string searchText)
        {
            string startFolder = StartFolder;
            switch (SearchOption)
            {
                case SearchType.File:
                    {
                        RecursiveFileSearch(startFolder, searchText);
                    }
                    break;

                case SearchType.Folder:
                    {
                        RecursiveFolderSearch(startFolder, searchText);
                    }
                    break;
                case SearchType.All:
                    {
                        RecursiveFileSearch(startFolder, searchText);
                        RecursiveFolderSearch(startFolder, searchText);
                    }
                    break;
            }
        }

        /// <summary>
        /// Klasör arama 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public bool SearchFolderName(string name, string searchText)
        {
            if (!CaseSensitive)
            {
                string foldername = name.GetDirectoryName();
                if (foldername.ToLower().Contains(searchText.ToLower()))
                {
                    ResultFound(name, searchText); //Sonuç bulunduğu için listbox'a ekleme metodu çağrılır.
                    FolderSearched++; //Aranan klasör sayısı arttırılır.
                    return true;
                }
                FolderSearched++; //Eğer bulamamışsa bile o klasörü aradığından; aranan klasör sayısı arttırılır.
                return false;
            }
            else
            {
                if (name.GetDirectoryName().Contains(searchText))
                {
                    ResultFound(name, searchText); //Sonuç bulunduğu için listbox'a ekleme metodu çağrılır.
                    FolderSearched++; //Aranan klasör sayısı arttırılır.
                    return true;
                }

                FolderSearched++; //Eğer bulamamışsa bile o klasörü aradığından; aranan klasör sayısı arttırılır.
                return false;
            }

            //if (!CaseSensitive) //Varsaıylan seçilmemiş değeri false'dur. Eğer seçilmemişse büyük/küçük harf duyarlı olmasın.
            //{
            //    if (name.GetDirectoryName().Contains(searchText))
            //    {
            //        // if (name.GetDirectoryName().ToLower == searchText.ToLower)
            //        // {
            //        //     ResultFound(name, searchText); //Sonuç bulunduğu için listbox'a ekleme metodu çağrılır.
            //        //     FolderSearched++; //Aranan klasör sayısı arttırılır.
            //        //     return true;
            //        // }
            //        ////return false;
            //    }

            //    FolderSearched++; //Eğer bulamamışsa bile o klasörü aradığından; aranan klasör sayısı arttırılır.
            //    return false;
            //}
            //else
            //{
                // string dPath = CaseSensitive ? name : name.ToLower(); //eğer büyük küçük harf duyarlı değilse hepsini küçük baz alır.            
               
            //}


        }
        public bool CaseSensitiveValue()
        {
            return CaseSensitive;
        }

        /// <summary>
        /// Aranan/bulunan sayaçlarını sıfırlama
        /// </summary>
        public void ClearSearchCounters()
        {
            FileSearched = 0;
            FolderSearched = 0;
            //Bulunan ögeler zaten listbox'taki eleman sayısı olduğundan o otomatik sıfırlanılır.
        }
        public void Find()
        {
            try
            {
                CancelSearch(); //O an başka bir arama varsa o durdurulur.
                ClearSearchCounters(); //Sayaçlar sıfırlanılır.                

                if (!string.IsNullOrEmpty(SearchFor)) //Aranılacak dosya/klasör
                {
                    if (StartFolder.IsDirectory())
                    {
                        Clear();
                        SetSearchingStatus(true);

                        Task.Run(() =>
                        {
                            string searchText = CaseSensitive ? SearchFor : SearchFor.ToLower();

                            if (SearchRecursive)
                            {
                                StartSearchRecursively(searchText);
                            }

                            else
                            {
                                //StartSearchNonRecursively(searchText);
                                //Recursive olmayan arama kodlanacak.
                            }

                            SetSearchingStatus(false);
                        });
                    }
                }
            }
            catch (Exception e) { MessageBox.Show($"{e.Message} -- Cancelling Search"); CancelSearch(); }
        }

        public void RecursiveFileSearch(string startFolder,string searchText)
        {
            void DirectorySearch(string toSearchDir)
            {
                foreach (string folder in Directory.GetDirectories(toSearchDir))
                {
                    if (!IsSearching) return; //Arama sırasında iptal'e basılmışsa arama sonlandırılır.

                    foreach (string file in Directory.GetFiles(folder))
                    {
                        if (!IsSearching) return;

                        SearchFileName(file, searchText);
                    }

                    FolderSearched++;

                    DirectorySearch(folder);
                }
            }

            DirectorySearch(startFolder);

            foreach (string file in Directory.GetFiles(startFolder))
            {
                if (!IsSearching) return;

                SearchFileName(file, searchText);
            }
        }
        public void RecursiveFolderSearch(string startFolder, string searchText)
        {
            void DirectorySearch(string toSearchDir)
            {
                foreach (string folder in Directory.GetDirectories(toSearchDir))
                {
                    if (!IsSearching) return; //Arama sırasında iptal'e basılmışsa arama sonlandırılır.

                    SearchFolderName(folder, searchText);

                    DirectorySearch(folder);//Alt klasör için arama devam eder. Bu yüzden recursive olarak metodu tekrar çağırır.
                }
            }

            DirectorySearch(startFolder);
        }
    }
}