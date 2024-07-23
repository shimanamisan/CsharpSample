using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppShellNavigation.ViewModels
{
    [QueryProperty(nameof(Email), "email")]
    public partial class ProfileDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

        [RelayCommand]
        private async Task Back()
        {
            // ナビゲーションページをPopする（前のページに戻る）
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
