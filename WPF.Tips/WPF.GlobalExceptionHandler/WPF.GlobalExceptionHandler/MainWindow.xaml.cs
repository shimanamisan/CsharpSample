using WPF.GlobalExceptionHandler;
using System;
using System.Windows;

namespace Global_Exception_Handler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Faital_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zero = 0;

                var cal = 10 / zero;
            }
            catch (Exception ex)
            {
                throw new ErrorException($"Faitalが発生しました。", ex);
            }

        }

        private void Warning_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zero = 0;

                var cal = 10 / zero;
            }
            catch (Exception ex)
            {
                throw new WarningException($"FaitalWarningが発生しました。", ex);
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zero = 0;

                var cal = 10 / zero;
            }
            catch (Exception)
            {
                throw new InfoException();
            }
        }

        private void Other_Click(object sender, RoutedEventArgs e)
        {
            var zero = 0;

            // try catch で例外を補足しない
            var cal = 10 / zero;
        }
    }
}
