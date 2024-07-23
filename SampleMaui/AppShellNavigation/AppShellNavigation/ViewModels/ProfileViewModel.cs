using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppShellNavigation.ViewModels
{
    [QueryProperty(nameof(Email), "email")]
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _modifiedEmail;

        partial void OnEmailChanged(string value)
        {
            ModifiedEmail = $"{value} さん、ようこそ！";
        }

        [RelayCommand]
        private async Task ProfileDetgail()
        {
            await Shell.Current.GoToAsync($"profiledetail?email={_email}");
        }
    }
}
