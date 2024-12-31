using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MVVM.MultiWindowSample.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isMaximized = false;

        public MainWindow()
        {
            InitializeComponent();

            var converter = new BrushConverter();  
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (_isMaximized)
                {
                    WindowState = WindowState.Normal;
                    Width = 1080;
                    Height = 720;

                    _isMaximized = false;
                }
                else
                {
                    WindowState = WindowState.Maximized;

                    _isMaximized = true;
                }
            }
        }
    }
}
