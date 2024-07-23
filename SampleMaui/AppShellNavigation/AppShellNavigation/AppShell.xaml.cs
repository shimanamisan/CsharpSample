using AppShellNavigation.Views;

namespace AppShellNavigation
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // ProfileDetailページをProfileページの下の階層として登録
            Routing.RegisterRoute("profile/profiledetail", typeof(ProfileDetail));
        }
    }
}
