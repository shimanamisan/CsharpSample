using MVVM.OverlayOnProgressBar.Command;
using MVVM.OverlayOnProgressBar.Model;
using MVVM.OverlayOnProgressBar.Object;
using System;
using System.Threading; // CancellationTokenSource を使用するのに追加
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM.OverlayOnProgressBar.ViewModel
{
  internal class MainWIndowViewModel : ViewModelBase
  {
    // 取り消す必要があることを CancellationToken に通知する
    private CancellationTokenSource? _cancelToken;

    // Button Commnadプロパティにバインドしているプロパティ
    public DelegateCommand ExecuteShowOverlayButton { get; set; }
    public DelegateCommand ExecuteProgressCancelButton { get; set; }

    #region データバインディングさせているプロパティ
    // 「％」を表示させる
    private string? _progressPercentage;
    public string? ProgressPercentage
    {
      get => _progressPercentage;

      set => SetProperty(ref _progressPercentage, value);
    }

    // オーバーレイの表示・非表示を切り替える
    private string _isOverlay = "Hidden";
    public string IsOverlay
    {
      get => _isOverlay;
      set => SetProperty(ref _isOverlay, value);
    }

    // プログレスバーのカウンターを表示する
    private float _progressValue = 0;
    public float ProgressValue
    {
      get => _progressValue;
      set => SetProperty(ref _progressValue, value);
    }
    #endregion

    public MainWIndowViewModel()
    {
      ExecuteShowOverlayButton = new DelegateCommand(ExecuteShowOverlay);
      ExecuteProgressCancelButton = new DelegateCommand(ExecuteProgressCancel);
    }

    /// <summary>
    /// オーバーレイを表示
    /// </summary>
    private void ExecuteShowOverlay()
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
            MessageBox.Show($"処理がキャンセルされました: {ex.Message}", "キャンセル通知", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            IsOverlay = "Hidden";
            ProgressValue = 0;

            return;
          }
        }

      });

    }

    /// <summary>
    /// キャンセル処理
    /// </summary>
    private void ExecuteProgressCancel()
    {
      // キャンセル処理を実行して、OperationCanceledException に例外を投げる
      _cancelToken.Cancel();
    }
  }
}
