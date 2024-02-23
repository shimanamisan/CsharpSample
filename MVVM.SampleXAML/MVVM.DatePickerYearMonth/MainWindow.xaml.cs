using MVVM.DatePickerYearMonth.ViewModel;
using System.Windows;

namespace MVVM.DatePickerYearMonth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWIndowViewModel();

        }
    }
}