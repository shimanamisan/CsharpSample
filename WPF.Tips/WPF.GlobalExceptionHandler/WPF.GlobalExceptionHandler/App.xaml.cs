using WPF.GlobalExceptionHandler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Global_Exception_Handler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        public App()
        {
            // 処理されていない例外をここでまとめてキャッチする
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    caption = "情報";

                    Debug.WriteLine(e.Exception.Message);

                    // 警告などの場合はアプリケーションを落とさないようにする（デフォルトでは false なのでアプリケーションは落ちる）
                    e.Handled = true;
                }

                if (exceptionBase.Kind == BaseException.ExceptonKind.Warning)
                {
                    iconImage = MessageBoxImage.Warning;
                    caption = "警告";

                    Debug.WriteLine(e.Exception.Message);

                    // 警告などの場合はアプリケーションを落とさないようにする（デフォルトでは false なのでアプリケーションは落ちる）
                    e.Handled = true;
                }

                // ErrorException は BaseException を継承して意図している例外であるが、
                // アプリケーションは落ちる動きとする

            }

            MessageBox.Show(e.Exception.Message, caption, MessageBoxButton.OK, iconImage);
        }
    }
}
