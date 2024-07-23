namespace AppShellNavigation
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        /// <summary>
        /// 起動時の処理をオーバーライド
        /// </summary>
        //protected override async void OnStart()
        //{
        //    // AppShell.xaml では一番上に記述したページが起動時に表示されるが
        //    // 任意の位置（例えば最終行に記述したShellContent）を表示する場合
        //    await Shell.Current.GoToAsync("//profile");

        //    base.OnStart();
        //}
    }
}
