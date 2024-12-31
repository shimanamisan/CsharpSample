using System.Windows;

namespace MVVM.MultiWindowSample.Servicies
{
    public class WindowService : IWindowService
    {
        /// <summary>
        /// 親ウィンドウの情報
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        /// サービスプロバイダ
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        public WindowService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void ShowWindow<TWindow, TViewModel>(object? parameter = null, Window? owner = null)
            where TWindow : Window, new()
            where TViewModel : class
        {
            try
            {
                // 新しいウィンドウを作成
                var window = new TWindow();
                var viewModel = _serviceProvider.GetService(typeof(TViewModel))
                    ?? throw new InvalidOperationException($"Failed to resolve {typeof(TViewModel).Name}");

                // ViewModel に IParameterReceiver が実装されていればパラメーターを渡す
                if (viewModel is IParameterReceiver parameterReceiver)
                {
                    parameterReceiver.ReceiveParameter(parameter);
                }

                window.DataContext = viewModel;

                if (owner != null)
                {
                    window.Owner = owner;
                }

                window.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
