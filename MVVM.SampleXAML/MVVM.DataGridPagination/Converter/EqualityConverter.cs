using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVM.DataGridPagination.Converter
{
    /// <summary>
    /// 2つの値の等価性を比較するためのマルチバリューコンバーター<br/>
    /// 主にWPFのデータバインディングで使用される
    /// </summary>
    public class EqualityConverter : IMultiValueConverter
    {
        /// <summary>
        /// 2つの値を比較し等しいかどうかを判定する
        /// </summary>
        /// <param name="values">比較する値の配列。少なくとも2つの要素が必要</param>
        /// <param name="targetType">変換後の型。この実装では使用しない</param>
        /// <param name="parameter">追加のパラメータ。この実装では使用しない</param>
        /// <param name="culture">カルチャ情報。この実装では使用しない</param>
        /// <returns>
        /// 2つの値が等しい場合はtrue、そうでない場合はfalse<br/>
        /// 値の配列が無効な場合（要素数が2未満、または要素が未設定）も、falseを返す
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            /************************************************************************
             * values.Length
             * - MultiBindingから少なくとも2つの値が渡されることをチェック
             * 
             * values[0] == DependencyProperty.UnsetValue
             * - 最初の値（通常はページ番号）が設定されていない、または無効な場合をチェックする
             * - DependencyProperty.UnsetValueは依存関係プロパティが明示的に設定されていない場合のデフォルト値で、この値が渡された場合比較が出来ないので false の条件とする
             * 
             * values[1] == DependencyProperty.UnsetValue
             * - 2番目の値（通常は現在のページ番号）が設定されていない、または無効な場合をチェックする
             * - 動作は1番目の値のときと同じ
             */
            if (values.Length < 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                /************************************************************************
                 * 条件に合致した場合に false を返すことで以下の状況を防ぐ
                 * - バインディングエラーによる例外の発生
                 * - 無効な値による誤った比較結果
                 * - 予期しない動作やビジュアルの不整合
                 */
                return false;
            }

            // 2つの値が等しいかチェックする
            return Equals(values[0], values[1]);
        }

        /// <summary>
        /// 単一の値を複数の値に変換する。この実装では使用しない
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <param name="targetTypes">目標とする型の配列</param>
        /// <param name="parameter">追加のパラメータ</param>
        /// <param name="culture">カルチャ情報</param>
        /// <returns>変換された値の配列</returns>
        /// <exception cref="NotImplementedException">このメソッドは実装されていない</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
