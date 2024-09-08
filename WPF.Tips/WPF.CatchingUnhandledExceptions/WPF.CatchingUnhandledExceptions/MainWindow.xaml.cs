using WPF.CatchingUnhandledExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Catching.Unhandled.Exceptions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// UIスレッド
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void DispatcherException_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // 意図した例外だった場合
                var zero1 = 0;
                var cal1 = 10 / zero1;
            }
            catch (Exception ex)
            {
                throw new ToDivisionByZeroException("DispatcherExceptionが発生しました。", ex);
            }
        }

        /// <summary>
        /// 致命的なエラー
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void AppDomainException_Click(object sender, RoutedEventArgs e)
        {
            var zero = 0;
            var cal = 10 / zero;

            try
            {
                // 正常な処理
            }
            catch (Exception ex)
            {
                throw new Exception($"致命的なエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// 非UIスレッド1
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void TaskSchedulerException1_Click(object sender, RoutedEventArgs e)
        {
            // タイマーを開始
            BackgroundWorker.Start();

            try
            {
                // awaitしていない処理はtry catch で例外を補足できない
                // TaskScheduler.UnobservedTaskException で補足される
                Task.Run(() =>
                {
                    var zero = 0;
                    var cal = 10 / zero;
                });

            }
            catch (Exception ex)
            {
                throw new Exception($"TaskSchedulerException1が発生しました。", ex);
            }
        }

        /// <summary>
        /// 非UIスレッド2
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private async void TaskSchedulerException2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // await で非同期処理を待機すると try catch で例外を補足できる
                await ToDivisionByZero();
            }
            catch (Exception ex)
            {
                throw new ToDivisionByZeroException($"TaskSchedulerException2が発生しました。", ex);
            }
        }

        /// <summary>
        /// 通常の例外処理
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void NomalException_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var zero = 0;
                var cal = 10 / zero;
            }
            catch (Exception ex)
            {
                throw new Exception($"NomalExceptionが発生しました。", ex);
            }
        }

        /// <summary>
        /// ガベージコレクタを実行
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント</param>
        private void GarbageCollection_Click(object sender, RoutedEventArgs e)
        {
            // 明示的にガベージコレクションを実行
            GC.Collect();

            // 現在のスレッドを一時停止し、すべてのファイナライザが実行されるのを待機
            GC.WaitForPendingFinalizers();
        }


        /// <summary>
        /// 非同期処理（Zeroの除算）
        /// </summary>
        /// <returns></returns>
        private async Task<int> ToDivisionByZero()
        {
            return await Task.Run(() =>
                    {
                        var zero = 0;
                        return 10 / zero;
                    });
        }

    }
}
