using System.Windows;

namespace MVVM.MultiWindowSample.Services
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

        public void ShowWindowWithCallback<TWindow, TViewModel, TResult>(
            object? parameter = null,
            Window? owner = null,
            Action<TResult?>? resultCallback = null) where TWindow : Window, new()
                                                     where TViewModel : class
        {
            try
            {
                // 新しいウィンドウを作成
                var window = new TWindow();
                var viewModel = _serviceProvider.GetService(typeof(TViewModel));

                if (viewModel == null)
                {
                    throw new InvalidOperationException($"Failed to resolve {typeof(TViewModel).Name}");
                }

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

                // 画面を閉じた際の処理
                window.Closed += (sender, args) =>
                {
                    // ViewModelにIResultProvider<TResult>が実装されていた場合
                    if (viewModel is IResultProvider<TResult> resultProvider)
                    {
                        resultCallback?.Invoke(resultProvider.GetResult());
                    }
                    else
                    {
                        // TResult型のデフォルト値が resultCallback に渡される
                        // TResult型が参照型（クラスなど）だった場合は null になる
                        // TResult型がプリミティブな値だったらその方のデフォルト値になる（intなら0）
                        resultCallback?.Invoke(default);
                    }
                };

                window.ShowDialog();
            }
            catch (Exception)
            {
                // ログなどを記録

                throw;
            }
        }

        public void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
