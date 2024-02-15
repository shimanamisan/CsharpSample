using MVVM.OverlayOnProgressBar.Command;
using MVVM.OverlayOnProgressBar.Model;
using MVVM.OverlayOnProgressBar.Object;
using System;
using System.Threading; // CancellationTokenSource を使用するのに追加
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.OverlayOnProgressBar.ViewModel
{
    public class MainWIndowViewModel : ViewModelBase
    {
        /// <summary>
        /// 取り消す必要があることを CancellationToken に通知する
        /// </summary>
        private CancellationTokenSource? _cancelToken;

        #region コマンド

        public DelegateCommand ShowOverlayButtonCommand
            => _showOverlayButtonCommand ?? (_showOverlayButtonCommand = new DelegateCommand(OnShowOverlay));

        private DelegateCommand _showOverlayButtonCommand;

        public DelegateCommand ProgressCancelCommand
            => _progressCancelCommand ?? (_progressCancelCommand = new DelegateCommand(OnProgressCancel));

        private DelegateCommand _progressCancelCommand;

        #endregion

        #region データバインディングさせているプロパティ

        /// <summary>
        /// 「％」を表示させる
        /// </summary>
        public string? ProgressPercentage
        {
            get => _progressPercentage;

            set => SetProperty(ref _progressPercentage, value);
        }
        private string? _progressPercentage;

        /// <summary>
        /// オーバーレイの表示・非表示を切り替えるプロパティ
        /// </summary>
        public string IsOverlay
        {
            get => _isOverlay;
            set => SetProperty(ref _isOverlay, value);
        }
        private string _isOverlay = "Hidden";

        /// <summary>
        /// プログレスバーのカウンターを表示する
        /// </summary>
        public float ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }
        private float _progressValue = 0;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWIndowViewModel()
        { }

        /// <summary>
        /// オーバーレイを表示
        /// </summary>
        private void OnShowOverlay()
        {
            Task.Run(async () =>
            {

                IsOverlay = "Visible";

                // 進捗状況を管理する Progressクラスに、自作した ProgressInfoクラスを渡す
                var p = new Progress<ProgressInfo>();

                // 進捗値が変更されるたびに呼ばれるイベントハンドラーにラムダ式で実行するメソッドを追加
                p.ProgressChanged += (o, e) =>
                {
                    // 変更後の値をプロパティに格納及び更新
                    ProgressValue = e.ProgressValue;
                    // TextBlockに表示させる文字列を更新する
                    ProgressPercentage = $"{e.ProgressValue} %";
                };

                using (_cancelToken = new CancellationTokenSource())
                {

                    try
                    {
                        // データを取得するモデルをインスタンス化
                        FetchApiModel apiModel = new FetchApiModel();

                        // データを取得する処理（非同期）を実行
                        // CancellationTokenSource インスタンスを引数に渡す
                        await apiModel.FetchApiEndPoint(p, _cancelToken);

                        IsOverlay = "Hidden";
                        ProgressValue = 0;

                    }
                    // スレッドが実行している操作のキャンセル時にスレッドにスローされる例外
                    catch (OperationCanceledException ex)
                    {
                        ShowMessageBox(ex.Message, "キャンセル通知");

                        IsOverlay = "Hidden";
                        ProgressValue = 0;

                        return;
                    }
                }

            });

        }

        /// <summary>
        /// メッセージボックス表示
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="caption">キャプション</param>
        private void ShowMessageBox(string message, string caption)
        {
            MessageBox.Show($"処理がキャンセルされました: {message}",
                            caption,
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// キャンセル処理
        /// </summary>
        private void OnProgressCancel()
        {
            // キャンセル処理を実行して、OperationCanceledException に例外を投げる
            _cancelToken.Cancel();
        }
    }
}
