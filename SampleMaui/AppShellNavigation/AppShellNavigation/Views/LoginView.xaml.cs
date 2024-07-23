using AppShellNavigation.ViewModels;

namespace AppShellNavigation.Views;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();

		BindingContext = new LoginViewModel();
	}
}