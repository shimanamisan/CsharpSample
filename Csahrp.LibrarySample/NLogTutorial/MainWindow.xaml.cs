using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace NLogTutorial
{

    public partial class MainWindow : Window
    {
        /// <summary>
        /// Loggerは各クラスでstatic変数で使用すること
        /// 新しいLoggerを作成するとオーバーヘッドが発生するので推奨されていない
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ループ処理を停止するフラグ
        /// </summary>
        private bool _isLogStop = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ログ出力のループ処理を開始する
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント引数</param>
        private async void LogOutput(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                _isLogStop = true;

                while (_isLogStop)
                {
                    // 以下はinfoフォルダに出力される
                    _logger.Trace("Traceを一回出力します");
                    _logger.Debug("Debugを一回出力します");
                    _logger.Info("Infoを一回出力します");

                    // 以下はerrorフォルダに出力される
                    _logger.Warn("Warnを一回出力します");
                    _logger.Error("Errorを一回出力します");
                    _logger.Fatal("Fatalを一回出力します");

                    await Task.Delay(100);
                }
            });
        }

        /// <summary>
        /// 意図的に例外を発生させる
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void LogExceptionOutput(object sender, RoutedEventArgs e)
        {
            try
            {
                var num = 0;
                // 0による除算で例外を発生させる
                var sum = 1 / num;
            }
            catch (Exception ex)
            {
                // 以下はerrorフォルダに出力される
                _logger.Warn("Warnを一回出力します: {0}", ex.Message);
                _logger.Error("Errorを一回出力します: {0}", ex.Message);
                _logger.Fatal("Fatalを一回出力します: {0}", ex.Message);

                MessageBox.Show("エラーが発生しました。");
            }
        }

        /// <summary>
        /// ループ処理を停止
        /// </summary>
        /// <param name="sender">コントロールオブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void LogRotationStop(object sender, RoutedEventArgs e)
        {
            _isLogStop = false;

            MessageBox.Show("ループ処理を停止しました。");
        }

    }
}
