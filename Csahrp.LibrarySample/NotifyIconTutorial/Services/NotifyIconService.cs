using H.NotifyIcon;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace NotifyIconTutorial.Services
{
    public sealed class NotifyIconService : INotifyIconService
    {
        /// <summary>
        /// NotifyIcon
        /// </summary>
        private TaskbarIcon? _notifyIcon;

        /// <summary>
        /// アイコン切り替え用のタイマー
        /// </summary>
        private DispatcherTimer? _iconBlinkTimer;

        /// <summary>
        /// 切り替え用のアイコンのパス（青色）
        /// </summary>
        private readonly string _normalIconPath = "pack://application:,,,/NotifyIconTutorial;component/Assets/Tasktray-Bule.ico";

        /// <summary>
        /// 切り替え用のアイコンのパス（赤色）
        /// </summary>
        private readonly string _alternateIconPath = "pack://application:,,,/NotifyIconTutorial;component/Assets/Tasktray-Red.ico";

        /// <summary>
        /// アイコンのImageSource（青色）
        /// </summary>
        private BitmapImage? _normalIcon;

        /// <summary>
        /// アイコンのImageSource（赤色）
        /// </summary>
        private BitmapImage? _alternateIcon;

        /// <summary>
        /// 現在のアイコン状態
        /// </summary>
        private bool _isAlternateIcon = false;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            // MainWindowからTaskbarIconを取得
            if (Application.Current.MainWindow != null)
            {

                var taskbarIcon = GetNotifyIcon();
                if (taskbarIcon != null)
                {
                    _notifyIcon = taskbarIcon;
                }
            }

            // アイコンの読み込み
            _normalIcon = new BitmapImage(new Uri(_normalIconPath));
            _alternateIcon = new BitmapImage(new Uri(_alternateIconPath));

            // タイマーの初期化
            _iconBlinkTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500) // 500ミリ秒ごとに切り替え
            };
            _iconBlinkTimer.Tick += IconBlinkTimer_Tick;
        }

        private void IconBlinkTimer_Tick(object? sender, EventArgs e)
        {
            if (_notifyIcon == null) return;

            // アイコンを交互に切り替え
            _isAlternateIcon = !_isAlternateIcon;
            _notifyIcon.IconSource = _isAlternateIcon ? _alternateIcon : _normalIcon;
        }

        public void StartIconBlinking()
        {
            _iconBlinkTimer?.Start();
        }

        public void HideNotifyIcon()
        {
            _notifyIcon?.SetCurrentValue(TaskbarIcon.VisibilityProperty, Visibility.Collapsed);
        }

        public void StopIconBlinking()
        {
            _iconBlinkTimer?.Stop();

            // 通常アイコンに戻す
            if (_notifyIcon != null && _normalIcon != null)
            {
                _notifyIcon.IconSource = _normalIcon;
                _isAlternateIcon = false;
            }
        }

        public void ShowNotifyIcon()
        {
            // _notifyIcon が null だったらNotifyIconを取得する
            _notifyIcon ??= GetNotifyIcon();

            // _notifyIcon が null でなけれは表示する
            _notifyIcon?.SetCurrentValue(TaskbarIcon.VisibilityProperty, Visibility.Visible);
        }

        /// <summary>
        /// NotifyIconを取得
        /// </summary>
        /// <returns></returns>
        private TaskbarIcon? GetNotifyIcon()
        {
            // MainWindowからNotifyIconを取得
            var mainWindow = Application.Current.MainWindow as Views.MainWindow;
            return mainWindow?.TrayIcon;
        }

        /// <summary>
        /// バルーン通知を表示
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="notificationType">通知の種類</param>
        public void ShowBalloonNotification(string title, string message, NotificationType notificationType)
        {
            try
            {
                // _notifyIcon が null だったらNotifyIconを取得する
                _notifyIcon ??= GetNotifyIcon();

                if (_notifyIcon == null) return;

                // 通知の種類に応じてアイコンを設定
                var iconType = notificationType switch
                {
                    NotificationType.Information => H.NotifyIcon.Core.NotificationIcon.Info,
                    NotificationType.Warning => H.NotifyIcon.Core.NotificationIcon.Warning,
                    NotificationType.Error => H.NotifyIcon.Core.NotificationIcon.Error,
                    _ => H.NotifyIcon.Core.NotificationIcon.None
                };

                // バルーン通知を表示
                _notifyIcon.ShowNotification(title, message, iconType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"通知の表示中にエラーが発生しました: {ex.Message}",
                    "エラー",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    /// <summary>
    /// 通知の種類
    /// </summary>
    public enum NotificationType
    {
        Information,
        Warning,
        Error
    }

}
