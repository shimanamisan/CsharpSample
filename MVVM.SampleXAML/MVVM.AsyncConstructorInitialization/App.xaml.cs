using MVVM.AsyncConstructorInitialization.Entities;
using MVVM.AsyncConstructorInitialization.Models;
using MVVM.AsyncConstructorInitialization.Repositories;
using MVVM.AsyncConstructorInitialization.Views;
using Prism.Ioc;
using System.Windows;

namespace MVVM.AsyncConstructorInitialization
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
