using System.Windows;

namespace MVVM.MultiWindowSample.Servicies
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

    }
}
