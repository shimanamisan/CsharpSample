using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppShellNavigation.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email = "test@hoge.com"; // 便宜上初期値を入れておく

        [ObservableProperty]
        private string _password = "1234567890";

        [RelayCommand]
        private async Task Submit()
        {
            // パラメーターとして渡す値の型は key = string, value = object と指定する
            var parameters = new Dictionary<string, object> 
            {
                { "email", Email }
            };

            // プロフィールページに遷移する
            await Shell.Current.GoToAsync("//profile", parameters);
        }
    }
}
