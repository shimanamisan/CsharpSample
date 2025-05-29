using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotifyIconTutorial.Services;
using System.ComponentModel;
using System.Windows;

namespace NotifyIconTutorial.ViewModels
{

    public partial class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// INotifyIconService
        /// </summary>
        private readonly INotifyIconService _notifyIconService;

        /// <summary>
        /// 閉じるボタンでタスクトレイに格納する
        /// </summary>
        [ObservableProperty]
        private bool _isClosedTaskTray;

        /// <summary>
        /// 最小化時にタスクトレイに格納する
        /// </summary>
        [ObservableProperty]
        private bool _isMinimization;

        /// <summary>
        /// バックグラウンド処理実行中かどうか
        /// </summary>
        [ObservableProperty]
        private bool _isBackgroundTaskRunning;

        /// <summary>
        /// 閉じるボタンを押したときのコマンド
        /// </summary>
        /// <param name="e"></param>
        [RelayCommand]
        private void WindowClosing(CancelEventArgs e)
        {
            // IsTaskTrayがtrueの場合、タスクトレイに格納して閉じるのをキャンセル
            if (IsClosedTaskTray)
            {
                e.Cancel = true; // 閉じるのをキャンセル

                var window = Application.Current.MainWindow;
                if (window != null)
                {
                    window.Hide(); // ウィンドウを非表示
                    // タスクトレイアイコンを表示
                    _notifyIconService.ShowNotifyIcon();
                }
            }
            // IsTaskTrayがfalseの場合は通常通り閉じる（e.Cancelはfalseのまま）
        }

        /// <summary>
        /// アプリケーションを終了するコマンド
        /// </summary>
        [RelayCommand]
        private void ExitApplication()
        {
            // Hide のままだと確認ダイアログが一瞬表示されて非表示になるので Activateメソッドを実行する
            Application.Current.MainWindow.Activate();

            var messege = "アプリケーションを終了しますか？";
            var caption = "確認";

            var result = MessageBox.Show(messege, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) return;

            _notifyIconService.HideNotifyIcon();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// メインウィンドウを表示するコマンド
        /// </summary>
        [RelayCommand]
        private void ShowWindow()
        {
            // ウィンドウを表示し、前面に持ってくる
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.Activate();
            Application.Current.MainWindow.Focus();

            // ウィンドウが表示されたらタスクトレイアイコンを非表示にする
            _notifyIconService.HideNotifyIcon();
        }

        /// <summary>
        /// バックグラウンドタスクを実行するコマンド
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task ExecuteBackgroundTask()
        {
            // すでに実行中なら何もしない
            if (IsBackgroundTaskRunning)
                return;

            try
            {
                // バックグラウンド処理開始
                IsBackgroundTaskRunning = true;

                // タスクトレイアイコンを表示して点滅開始
                _notifyIconService.StartIconBlinking();

                // バックグラウンド処理を模擬（実際の処理に置き換えてください）
                await Task.Delay(10000); // 10秒間の処理を模擬

                // 処理完了時の通知
                MessageBox.Show("バックグラウンド処理が完了しました", "完了", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // バックグラウンド処理終了
                IsBackgroundTaskRunning = false;

                // アイコン点滅を停止
                _notifyIconService.StopIconBlinking();
            }
        }

        /// <summary>
        /// メインウィンドウが最小化されたり状態が変化したときのイベント時に
        /// 実行されるコマンド
        /// </summary>
        [RelayCommand]
        private void WindowStateChanged()
        {
            var window = Application.Current.MainWindow;
            if (window == null) return;

            if (window.WindowState == WindowState.Minimized)
            {
                // 最小化時にタスクトレイに格納するか否か
                if (!IsMinimization) return;

                // ウィンドウを非表示
                Application.Current.MainWindow.Hide();
                // 通知アイコンを表示
                _notifyIconService.ShowNotifyIcon();
            }
        }

        /// <summary>
        /// バルーン表示（情報）
        /// </summary>
        /// <param name="message"></param>
        [RelayCommand]
        private void ShowInformation(string message)
        {
            _notifyIconService.ShowBalloonNotification(
                "情報",
                message,
                NotificationType.Information);
        }

        /// <summary>
        /// バルーン表示（警告）
        /// </summary>
        /// <param name="message"></param>
        [RelayCommand]
        private void ShowWarning(string message)
        {
            _notifyIconService.ShowBalloonNotification(
                "警告",
                message,
                NotificationType.Warning);
        }

        /// <summary>
        /// バルーン表示（エラー）
        /// </summary>
        /// <param name="message"></param>
        [RelayCommand]
        private void ShowError(string message)
        {
            _notifyIconService.ShowBalloonNotification(
                "エラー",
                message,
                NotificationType.Error);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="notifyIconService"></param>
        public MainWindowViewModel(INotifyIconService notifyIconService)
        {
            _notifyIconService = notifyIconService;
            _notifyIconService.Initialize();
        }
    }
}
