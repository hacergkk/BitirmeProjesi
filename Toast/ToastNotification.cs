using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;

namespace SearchApplication.Toast
{
    public class ToastNotification
    {
        public static void Show(string message, string title, bool isSuccess)
        {
            var notification = new Window
            {
                Width = 300,
                Height = 100,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = isSuccess ? Brushes.Green : Brushes.Red,
                Content = new TextBlock
                {
                    Text = message,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16
                },
                BorderBrush = Brushes.Transparent,
                WindowStyle = WindowStyle.None,
                Topmost = true,
                ShowInTaskbar = false,
                Opacity = 0.8
            };

            notification.Show();

            // Otomatik kapanma için bir süre bekleyelim
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (s, e) =>
            {
                notification.Close();
                timer.Stop();
            };
            timer.Start();
        }
    }
}
