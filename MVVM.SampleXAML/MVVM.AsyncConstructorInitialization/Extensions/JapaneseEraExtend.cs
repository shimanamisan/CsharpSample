using System;
using System.Globalization;

namespace MVVM.AsyncConstructorInitialization.Extensions
{
    public static class JapaneseEraExtend
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
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();
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
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();
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
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();
            return date?.ToString("ggy年M月d日", culture);
        }

        /// <summary>
        /// 西暦の日付を和暦「"令和●年xx月xx日"」に変換
        /// </summary>
        /// <param name="christianEra"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ConvertJapaneseEraTodateFullFromat(this string christianEra)
        {
            var cultureInfo = new CultureInfo("ja-JP");
            cultureInfo.DateTimeFormat.Calendar = new JapaneseCalendar();

            // 日付フォーマット
            const string format = "yyyy/M/d H:mm";
            // 渡ってきた値がフォーマットの形式と一致するか確認
            if (DateTime.TryParseExact(christianEra, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                // 和暦のフォーマットに変換して返す
                return date.ToString("ggy年M月d日", cultureInfo);
            }

            // 解析できなかった場合の処理
            throw new ArgumentException("和変換に失敗しました。", nameof(christianEra));
        }

        /// <summary>
        /// 西暦の日付を和暦「"令和●年度"」に変換
        /// </summary>
        /// <param name="christianEra">和暦の日付</param>
        /// <returns>西暦の日付の文字列（変換できない場合はnull）</returns>
        public static string ConvertJapaneseEraToFiscalYear(this string christianEra)
        {
            var cultureInfo = new CultureInfo("ja-JP");
            cultureInfo.DateTimeFormat.Calendar = new JapaneseCalendar();

            const string format = "yyyy/M/d H:mm";
            if (DateTime.TryParseExact(christianEra, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date.ToString("ggy年度", cultureInfo);
            }

            throw new ArgumentException("和変換に失敗しました。", nameof(christianEra));
        }
    }
}
