using System.Globalization;

namespace MVVM.DatePickerYearMonth.Helpers
{
    /// <summary>
    /// 日本の和暦を扱うための静的ヘルパークラス
    /// </summary>
    public static class JapaneseCalendar
    {
        /// <summary>
        /// 「令和◯年」と表示する
        /// </summary>
        /// <param name="date">DatePickerで選択された値</param>
        /// <returns>和暦（年のみ）を返す</returns>
        public static string ConvertToJapaneseEraShort(this DateTime? date)
        {

            if (date == null) return "日付";

            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
            return date?.ToString("ggy年", culture);
        }

        /// <summary>
        /// 「令和◯年◯月」と表示する
        /// </summary>
        /// <param name="date">DatePickerで選択された値</param>
        /// <returns>和暦（年月）を返す</returns>
        public static string ConvertToJapaneseEraMidle(this DateTime? date)
        {

            if (date == null) return "日付";

            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
            return date?.ToString("ggy年M月", culture);
        }

        /// <summary>
        /// 「令和◯年xx月xx日」と表示
        /// </summary>
        /// <param name="date">DatePickerで選択された値</param>
        /// <returns>和暦（年月日）を返す</returns>
        public static string ConvertToJapaneseEraFull(this DateTime? date)
        {
            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();
            return date?.ToString("ggy年M月d日", culture);
        }

        /// <summary>
        /// 和暦の日付（例："平成●年●月●●日"）を西暦に変換
        /// </summary>
        /// <param name="japaneseDate">和暦の日付</param>
        /// <returns>西暦の日付の文字列（変換できない場合はnull）</returns>
        public static string ConvertJapaneseEraTodate(this string japaneseDate)
        {
            var culture = new CultureInfo("ja-JP");
            culture.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();

            // 和暦の日付フォーマット
            const string format = "ggy'年'M'月'd'日'";
            if (DateTime.TryParseExact(japaneseDate, format, culture, DateTimeStyles.None, out DateTime date))
            {
                return date.ToString("yyyy/MM/dd");  // 西暦のフォーマットに変換
            }

            return null;
        }
    }
}
