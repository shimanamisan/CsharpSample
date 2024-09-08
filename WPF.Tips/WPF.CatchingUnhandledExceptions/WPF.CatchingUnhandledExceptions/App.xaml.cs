using WPF.CatchingUnhandledExceptions;
using NLog;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Catching.Unhandled.Exceptions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// ロガー
        /// </summary>
        static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            // UIスレッドで処理されていない例外をここでまとめてキャッチする
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // 非UIスレッドで処理されていない例外を処理する
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // 例外が処理されずにアプリケーションがクラッシュする直前に呼び出される
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// UIスレッド未処理
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // デフォルトのアイコンはエラーに設定
            MessageBoxImage iconImage = MessageBoxImage.Error;
            // キャプション
            string caption = "エラー";

            // 型が変換できなかったらnullが返却される（意図していない例外であればnullになる）
            var exceptionBase = e.Exception as BaseException;

            if (exceptionBase != null)
            {
                if (exceptionBase.Kind == BaseException.ExceptonKind.Info)
                {
                    iconImage = MessageBoxImage.Information;
                    caption = "意図した情報";

                    _logger.Info(e.Exception.Message, e.Exception);

                }

                if (exceptionBase.Kind == BaseException.ExceptonKind.Warning)
                {
                    iconImage = MessageBoxImage.Warning;
                    caption = "意図した警告";

                    _logger.Warn(e.Exception.Message, e.Exception);

                }

                if (exceptionBase.Kind == BaseException.ExceptonKind.Error)
                {
                    iconImage = MessageBoxImage.Error;
                    caption = "意図したエラー";

                    _logger.Error(e.Exception.Message, e.Exception);
                }

                // ExceptionBaseに変換できる例外は意図したエラーとしてアプリケーションを実行するものとする
                e.Handled = true;

                MessageBox.Show(e.Exception.Message, caption, MessageBoxButton.OK, iconImage);

            }
        }

        /// <summary>
        /// 非UIスレッド未処理
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            // デフォルトのアイコンはエラーに設定
            MessageBoxImage iconImage = MessageBoxImage.Error;
            // キャプション
            string caption = "バックグラウンドタスクでエラー";

            // タイマーを停止
            BackgroundWorker.Stop();
            TimeSpan elapsedTime = BackgroundWorker._stopwatch.Elapsed;

            _logger.Error(e.Exception, $"{e.Exception.Message} - [実行から例外補足まで掛かった時間: {elapsedTime.TotalSeconds}]");

            var result = MessageBox.Show("バックグラウンドタスクで\nアプリケーションを終了しますか？",
                                        caption,
                                        MessageBoxButton.YesNo,
                                        iconImage);

            if(result == MessageBoxResult.Yes)
            {
                Environment.Exit(1);
            }

            // 例外を処理済みとしてアプリケーションを継続する
            e.SetObserved();
        }

        /// <summary>
        /// クラッシュする直前に呼び出される
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // 例外オブジェクトを取得
            Exception exception = e.ExceptionObject as Exception;

            // デフォルトのアイコンはエラーに設定
            MessageBoxImage iconImage = MessageBoxImage.Error;
            // キャプション
            string caption = "致命的なエラー";

            _logger.Error(exception, exception.Message);

            MessageBox.Show("エラーが発生しました\nアプリケーションを終了します。", caption, MessageBoxButton.OK, iconImage);
        }
    }
}
