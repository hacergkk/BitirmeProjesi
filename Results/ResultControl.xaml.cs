using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SearchApplication.Converters;

namespace SearchApplication.Results
{
    /// <summary>
    /// Interaction logic for ResultControl.xaml
    /// </summary>
    public partial class ResultControl : UserControl
    {
        public ResultControl()
        {
            InitializeComponent();
            /// <Image Source="{Binding Icon, => Görüntü kaynağının icon olduğu belirtilir.
            /// UpdateSourceTrigger=PropertyChanged, => Icon propoertsi değiştiyse source otomatik güncellenir.
            /// Converter={StaticResource IconToImageSourceConverter}}" Converterı atandı.
            /// HorizontalAlignment="Left" Width="20" Height="20"/>

        }
    }
}
