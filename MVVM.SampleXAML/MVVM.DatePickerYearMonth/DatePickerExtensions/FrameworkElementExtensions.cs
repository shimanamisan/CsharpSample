using System.Windows;
using System.Windows.Media;

namespace MVVM.DatePickerYearMonth.DatePickerExtensions
{
    /// <summary>
    /// WPFアプリケーションにおけるビジュアルおよび論理ツリーのナビゲーションをサポートするユーティリティメソッドを提供する静的クラス
    /// 特定のUI要素の親要素を探すためのメソッドや、WPFの標準的なナビゲーションメソッドの機能を補完する
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// ビジュアルツリー上で指定されたアイテムの親を検索します。
        /// </summary>
        /// <typeparam name="T">検索対象のアイテムの型</typeparam>
        /// <param name="child">検索対象のアイテムの直接または間接的な子</param>
        /// <returns>指定された型に一致する最初の親アイテム。一致するアイテムが見つからない場合は null 参照が返される</returns>
        public static T TryFindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            // 親アイテムを取得
            var parentObject = GetParentObject(child);

            // ツリーの末端に到達した場合
            if (parentObject == null) return null;

            // 親が検索対象の型に一致するかチェック
            if (parentObject is T parent) return parent;

            // 再帰を使って次の階層へ遡っていく
            return TryFindParent<T>(parentObject);
        }

        /// <summary>
        /// WPFの <see cref="VisualTreeHelper.GetParent"/> メソッドの代替として、コンテンツ要素もサポートするメソッド
        /// コンテンツ要素については、このメソッドは要素の論理ツリーにフォールバックする
        /// </summary>
        /// <param name="child">処理対象のアイテム。</param>
        /// <returns>対象アイテムの親（存在する場合）。それ以外の場合は null。</returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            if (child is ContentElement contentElement)
            {
                var parent = ContentOperations.GetParent(contentElement) ??
                             (contentElement as FrameworkContentElement)?.Parent;

                if (parent != null) return parent;
            }

            // フレームワーク要素（DockPanelなど）の親も検索
            if (child is FrameworkElement frameworkElement)
            {
                var parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            // ContentElement / FrameworkElement でない場合はVisualTreeHelperに頼る
            return VisualTreeHelper.GetParent(child);
        }
    }
}