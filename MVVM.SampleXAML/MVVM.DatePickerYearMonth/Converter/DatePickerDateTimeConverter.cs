using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MVVM.DatePickerYearMonth.Converter
{
    /// <summary>
    /// 日付フォーマットを変換するためのコンバーター
    /// </summary>
    public sealed class DatePickerDateTimeConverter : IValueConverter
    {
        /// <summary>
        /// バインディングされた値を変換する
        /// </summary>
        /// <param name="value">DatePickerで選択された日付</param>
        /// <param name="targetType">変換後の値が期待する型</param>
        /// <param name="parameter">コンバーターで使用するパラメータ</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatStr = ((Tuple<DatePicker, string>)parameter).Item2;
            var selectedDate = (DateTime?)value;
            return DateTimeToString(formatStr, selectedDate);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tupleParam = ((Tuple<DatePicker, string>)parameter);
            var dateStr = (string)value;
            return StringToDateTime(tupleParam.Item1, tupleParam.Item2, dateStr);
        }

        public static string DateTimeToString(string formatStr, DateTime? selectedDate)
        {
            return selectedDate.HasValue ? selectedDate.Value.ToString(formatStr) : null;
        }

        public static DateTime? StringToDateTime(DatePicker datePicker, string dateFormat, string dateString)
        {
            DateTime parsedDate;
            // 指定されたフォーマットで日付文字列を解析する
            bool isParsed = DateTime.TryParseExact(dateString,
                                                   dateFormat,
                                                   CultureInfo.CurrentCulture,
                                                   DateTimeStyles.None,
                                                   out parsedDate);

            if (!isParsed)
            {
                isParsed = DateTime.TryParse(dateString, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate);
            }

            // 解析に成功した場合は解析された日付を、失敗した場合はDatePickerの選択された日付を返す
            return isParsed ? parsedDate : datePicker.SelectedDate;
        }

    }
}
