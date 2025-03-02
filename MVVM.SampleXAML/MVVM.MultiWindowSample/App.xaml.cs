using MVVM.MultiWindowSample.Entitirs;
using MVVM.MultiWindowSample.Models;
using MVVM.MultiWindowSample.Repositories;
using MVVM.MultiWindowSample.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using MVVM.MultiWindowSample.Services;
using MVVM.MultiWindowSample.ViewModels;

namespace MVVM.MultiWindowSample
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

            // Repositoryインターフェースと実装クラスを登録
            // ViewModelのコンストラクタの引数に IUsersRepository<UserEntity> を指定することでDIすることが可能になる
            services.AddTransient<IUsersRepository<UserEntity>, Users>();

            // 起動時の画面と対応したViewModelを登録
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<SubWindowViewModel>();

            // サブ画面を表示するサービスクラスを登録
            services.AddTransient<IWindowService, WindowService>();

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
