using AppShellNavigation.ViewModels;

namespace AppShellNavigation.Views;

public partial class ProfileDetail : ContentPage
{
	public ProfileDetail()
	{
		InitializeComponent();

		BindingContext = new ProfileDetailViewModel();
	}
}