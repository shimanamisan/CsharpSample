using System.Windows;
using System.Windows.Controls;

namespace MVVM.DataGridPagination.Controls
{
    /// <summary>
    /// ページネーションアイテムのテンプレートを選択するためのセレクタークラス
    /// </summary>
    public sealed class PaginationItemTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// 数字を表示するためのDataTemplateを取得または設定する
        /// </summary>
        public DataTemplate NumberTemplate { get; set; }

        /// <summary>
        /// 省略記号（...）を表示するためのDataTemplateを取得または設定する
        /// </summary>
        public DataTemplate EllipsisTemplate { get; set; }

        /// <summary>
        /// 指定されたアイテムに対して適切なDataTemplateを選択する
        /// </summary>
        /// <param name="item">テンプレートを選択するためのアイテム</param>
        /// <param name="container">ContentPresenter</param>
        /// <returns>選択されたDataTemplate</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string && (string)item == "...")
            {
                return EllipsisTemplate;
            }

            return NumberTemplate;
        }
    }
}
