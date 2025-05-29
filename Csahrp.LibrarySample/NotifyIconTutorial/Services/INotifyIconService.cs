namespace NotifyIconTutorial.Services
{
    /// <summary>
    /// 通知アイコン操作に関するインターフェース
    /// </summary>
    public interface INotifyIconService
    {
        /// <summary>
        /// 通知アイコン初期化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 通知アイコン表示
        /// </summary>
        void ShowNotifyIcon();

        /// <summary>
        /// 通知アイコン非表示
        /// </summary>
        void HideNotifyIcon();

        /// <summary>
        /// アイコンの点滅を開始
        /// </summary>
        void StartIconBlinking();

        /// <summary>
        /// アイコンの点滅を停止
        /// </summary>
        void StopIconBlinking();

        /// <summary>
        /// バルーン通知を表示
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="notificationType">通知の種類</param>
        void ShowBalloonNotification(string title, string message, NotificationType notificationType);
    }
}
