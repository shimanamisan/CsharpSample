using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.MultiWindowSample.Entitirs;
using MVVM.MultiWindowSample.Servicies;

namespace MVVM.MultiWindowSample.ViewModels
{
    public partial class SubWindowViewModel : ObservableObject, IParameterReceiver
    {
        [ObservableProperty]
        private string _userName = null!;

        public void ReceiveParameter(object parameter)
        {
            if (parameter is UserEntity entity)
            {
                UserName = entity.Name;
            }
        }
    }
}
