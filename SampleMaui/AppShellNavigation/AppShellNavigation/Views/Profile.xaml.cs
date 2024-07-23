using AppShellNavigation.ViewModels;

namespace AppShellNavigation.Views;

public partial class Profile : ContentPage
{
	public Profile()
	{
		InitializeComponent();

		BindingContext = new ProfileViewModel();
    }
}