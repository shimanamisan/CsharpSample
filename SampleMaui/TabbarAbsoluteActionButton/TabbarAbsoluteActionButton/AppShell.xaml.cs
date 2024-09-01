using System.Windows.Input;

namespace TabbarAbsoluteActionButton
{
    public partial class AppShell : Shell
    {
        public ICommand CustomActionButtonCommand { get; }
            = new Command(async () => await Current.DisplayAlert("タイトル", "Hello World", "OK"));

        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
