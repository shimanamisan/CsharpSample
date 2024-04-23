using MVVM.DataGridSearch.Entities;
using MVVM.DataGridSearch.Models;
using MVVM.DataGridSearch.Repositories;
using MVVM.DataGridSearch.Views;
using Prism.Ioc;
using System.Windows;

namespace MVVM.DataGridSearch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IUsersRepository<UserEntity>, Users>();
        }
    }
}
