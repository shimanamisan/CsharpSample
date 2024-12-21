using CommunityToolkitMvvmTutorial.Entitirs;
using CommunityToolkitMvvmTutorial.Models;
using CommunityToolkitMvvmTutorial.Repositories;
using CommunityToolkitMvvmTutorial.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CommunityToolkitMvvmTutorial
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
