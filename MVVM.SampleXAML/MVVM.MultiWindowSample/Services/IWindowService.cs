using System.Windows;

namespace MVVM.MultiWindowSample.Services
{
    /// <summary>
    /// Windowサービスインターフェース
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// サブ画面を表示する
        /// </summary>
        /// <typeparam name="TWindow">Window</typeparam>
        /// <typeparam name="TViewModel">Windowと関連したViewModel</typeparam>
        /// <param name="parameter">パラメータ</param>
        /// <param name="owner">親ウィンドウ</param>
        void ShowWindow<TWindow, TViewModel>(object? parameter = null, Window? owner = null)
            // TWindowの制約
            where TWindow : Window, // Windowクラスを継承していること
                            new() // 指定した型が空のコンストラクタを持っていること
            // TViewModelの制約
            where TViewModel : class; // 参照型（クラス）であること

        /// <summary>
        /// サブ画面を表示し、結果をコールバックで返す
        /// </summary>
        /// <typeparam name="TWindow">Window</typeparam>
        /// <typeparam name="TViewModel">Windowと関連したViewModel</typeparam>
        /// <typeparam name="TResult">戻り値の型</typeparam>
        /// <param name="parameter">パラメータ</param>
        /// <param name="resultCallback">結果を受け取るコールバック</param>
        /// <param name="owner">親ウィンドウ</param>
        void ShowWindowWithCallback<TWindow, TViewModel, TResult>(
            object? parameter = null,
            Window? owner = null,
            Action<TResult?>? resultCallback = null) where TWindow : Window, new()
                                                     where TViewModel : class;

        /// <summary>
        /// 指定されたウィンドウを閉じる
        /// </summary>
        /// <param name="window">閉じるウィンドウ</param>
        void CloseWindow(Window window);
    }
}
