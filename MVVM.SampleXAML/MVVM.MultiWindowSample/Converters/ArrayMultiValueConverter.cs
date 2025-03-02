using System.Globalization;
using System.Windows.Data;

namespace MVVM.MultiWindowSample.Converters
{
    /// <summary>
    /// 複数のバインディングソースの値を1つのターゲット値に変換する
    /// </summary>
    public class ArrayMultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// 複数の値を配列に変換する
        /// </summary>
        /// <param name="values">変換する値の配列</param>
        /// <param name="targetType">変換先の型</param>
        /// <param name="parameter">コンバーターパラメーター</param>
        /// <param name="culture">カルチャー情報</param>
        /// <returns>変換された値の配列のクローン</returns>

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 元のデータが変更されないように値をコピーしてCommandParameterに渡す
            return values.Clone();

        }

        /// <summary>
        /// 今回は使用しない
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
