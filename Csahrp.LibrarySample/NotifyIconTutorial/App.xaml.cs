using Microsoft.Extensions.DependencyInjection;
using NotifyIconTutorial.Services;
using NotifyIconTutorial.ViewModels;
using NotifyIconTutorial.Views;
using System.Windows;

namespace NotifyIconTutorial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // サービスコレクションを作成
            var services = new ServiceCollection();

            // 起動時の画面と対応したViewModelを登録
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<INotifyIconService, NotifyIconService>();

            // サービスプロバイダーから登録したViewModelを呼び出しDataContextに代入する
            var provider = services.BuildServiceProvider();
            var mainWindow = new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            };

            // 画面を起動
            mainWindow.Show();
        }
    }
}
