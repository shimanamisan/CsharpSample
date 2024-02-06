using MahApps.Metro.Controls;
using MVVM.OverlayOnProgressBar.ViewModel;

namespace MVVM.OverlayOnProgressBar
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : MetroWindow
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = new MainWIndowViewModel();
    }
  }
}
