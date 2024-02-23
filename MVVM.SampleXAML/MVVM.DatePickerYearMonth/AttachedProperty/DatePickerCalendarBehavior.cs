using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace MVVM.DatePickerYearMonth.AttachedProperty
{
    /// <summary>
    /// DatePicker コントロールを拡張して、年と月のみを表示させる機能を提供する。
    /// このクラスを使用することで、標準の DatePicker から日付の選択部分を除外し、
    /// ユーザーが年と月のみを選択できるようにカスタマイズする。
    /// </summary>
    public class DatePickerCalendarBehavior
    {
        /// <summary>
        /// 依存関係プロパティの定義
        /// DatePicker コントロールが月と年の選択のみを許可するかどうかを指定するためのもの
        /// </summary>  
        public static readonly DependencyProperty IsMonthYearProperty =
            DependencyProperty.RegisterAttached(
                   "IsMonthYear",
                    typeof(bool),
                    typeof(DatePickerCalendarBehavior),
                    new PropertyMetadata(OnIsMonthYearChanged));

        public static bool GetIsMonthYear(DependencyObject o)
            => (bool)o.GetValue(IsMonthYearProperty);

        public static void SetIsMonthYear(DependencyObject o, bool value)
            => o.SetValue(IsMonthYearProperty, value);

        private static void OnIsMonthYearChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            // イベントが発生したオブジェクトをDatePickerにキャスト
            var datePicker = (DatePicker)o;

            // メインスレッドで実行するための処理
            Application.Current.Dispatcher
                // BeginInvokeは非同期で実行される
                .BeginInvoke(
                    // アプリケーションがロードされ、レンダリングが完了したあとに実行する優先度
                    DispatcherPriority.Loaded,
                    // 実行されるActionデリゲートを定義
                    new Action<DatePicker, DependencyPropertyChangedEventArgs>(SetCalendarEventHandlers), datePicker, e);
        }

        /// <summary>
        /// OnIsMonthYearChangedイベント発火時に実行される処理
        /// </summary>
        /// <param name="datePicker">DatePicker</param>
        /// <param name="e">イベントに関する詳細情報</param>
        private static void SetCalendarEventHandlers(DatePicker datePicker, DependencyPropertyChangedEventArgs e)
        {
            // プロパティの値に変更があったか確認
            if (e.NewValue == e.OldValue) return;

            if ((bool)e.NewValue)
            {
                datePicker.CalendarOpened += DatePickerOnCalendarOpened;
                datePicker.CalendarClosed += DatePickerOnCalendarClosed;
            }
            else
            {
                datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
                datePicker.CalendarClosed -= DatePickerOnCalendarClosed;
            }
        }

        /// <summary>
        /// カレンダーがー表示される直前に実行されるイベントハンドラに登録する処理
        /// </summary>
        /// <param name="sender">イベントの発生源</param>
        /// <param name="routedEventArgs">イベントに関する詳細情報</param>
        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            var calendar = GetDatePickerCalendar(sender);
            // カレンダーが開かれた時に年月を選択するビューを表示する（1年分のカレンダー）
            // デフォルトのモードはMonth
            calendar.DisplayMode = CalendarMode.Year;

            // カレンダーの表示モードが変更された時に実行されるイベントハンドラに処理を登録
            calendar.DisplayModeChanged += CalendarOnDisplayModeChanged;
        }

        /// <summary>
        /// DatePickerのカレンダーが閉じられたとき実行されるイベントハンドラに登録する処理
        /// </summary>
        /// <param name="sender">イベントの発生源</param>
        /// <param name="routedEventArgs">イベントに関する詳細情報</param>
        private static void DatePickerOnCalendarClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            var datePicker = (DatePicker)sender;
            var calendar = GetDatePickerCalendar(sender);
            // カレンダーの選択されれた値を登録
            datePicker.SelectedDate = calendar.SelectedDate;

            // 表示モードの変更が不要になるので、カレンダーが閉じられた時にイベントハンドラの購読を解除
            calendar.DisplayModeChanged -= CalendarOnDisplayModeChanged;
        }

        /// <summary>
        /// カレンダーの表示モードが変更されたときに呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender">イベントの発生源</param>
        /// <param name="e">イベントに関する詳細情報</param>
        private static void CalendarOnDisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var calendar = (Calendar)sender;

            // カレンダーの表示モードが月表示モードであるか判定
            if (calendar.DisplayMode != CalendarMode.Month) return;

            // 選択された日付を取得する（選択した月の1日が渡ってくる）
            calendar.SelectedDate = GetSelectedCalendarDate(calendar.DisplayDate);

            // 選択されたCalendarから親要素のDatePickerを検索する
            var datePicker = GetCalendarsDatePicker(calendar);

            // 該当のDatePickerを閉じる
            datePicker.IsDropDownOpen = false;
        }

        /// <summary>
        /// DatePickerのカレンダーを取得する
        /// </summary>
        /// <param name="sender">カレンダーを取得する元となるオブジェクト</param>
        /// <returns>DatePicker のカレンダー</returns>
        private static Calendar GetDatePickerCalendar(object sender)
        {
            var datePicker = (DatePicker)sender;
            // DatePickerコントロールのテンプレート内で定義されているポップアップ部分を取得
            var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
            // ポップアップのカレンダー部分を取得
            return ((Calendar)popup.Child);
        }

        /// <summary>
        /// DatePickerのポップアップ部分を取得する再帰処理
        /// </summary>
        /// <param name="child">DatePicker を取得する元となる FrameworkElement</param>
        /// <returns>関連する DatePicker</returns>
        private static DatePicker GetCalendarsDatePicker(FrameworkElement child)
        {
            var parent = (FrameworkElement)child.Parent;
            // ブレークポイント
            if (parent.Name == "PART_Root")
            {
                return (DatePicker)parent.TemplatedParent;
            }
                
            return GetCalendarsDatePicker(parent);
        }

        /// <summary>
        /// 日付から年月をする
        /// </summary>
        /// <param name="selectedDate">基準となる日付</param>
        /// <returns>年月のみを考慮した DateTime?</returns>
        private static DateTime? GetSelectedCalendarDate(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue) return null;

            // 日付から年月を取得し、その月の1日を返す
            return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, 1);
        }
    }
}