using MVVM.DataGridPagination.Entities;
using MVVM.DataGridPagination.Infrastructure.Implements;
using MVVM.DataGridPagination.Repositories;
using MVVM.DataGridPagination.Views;
using Prism.Ioc;
using System.Windows;

namespace MVVM.DataGridPagination
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
