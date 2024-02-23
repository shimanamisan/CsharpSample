using MVVM.DatePickerYearMonth.Converter;
using MVVM.DatePickerYearMonth.DatePickerExtensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVM.DatePickerYearMonth.CustomControl
{
    public sealed class CustomDatePicker : DatePicker
    {
        /// <summary>
        /// 依存関係プロパティの定義
        /// DatePicker コントロールが月と年の選択のみを許可するかどうかを指定するためのもの
        /// </summary>
        public static readonly DependencyProperty IsMonthYearProperty =
            DependencyProperty.Register("IsMonthYearDP", // XAML側で使用するプロパティ名前を指定
                                        typeof(bool), // プロパティの型を指定
                                        typeof(CustomDatePicker), // DatePickerCalendar クラスにアタッチするように指定
                                        new PropertyMetadata(OnIsMonthYearChanged)); // 追加データ（メタデータ）としてコールバック関数

        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.Register("DateFormatDP",
                                        typeof(string),
                                        typeof(CustomDatePicker),
                                        new PropertyMetadata(OnDateFormatChanged));

        #region IsMonthYearプロパティのCLRラッパー

        public bool IsMonthYearDP
        {
            get => (bool)GetValue(IsMonthYearProperty);
            set => SetValue(IsMonthYearProperty, value);
        }

        #endregion

        #region DateFormatDPプロパティのCLRラッパー

        public string DateFormatDP
        {
            get => (string)GetValue(DateFormatProperty);
            set => SetValue(DateFormatProperty, value);
        }

        #endregion

        #region DateFormat依存関係プロパティの定義, 取得と設定

        public static string GetDateFormat(DependencyObject o)
        {
            return (string)o.GetValue(DateFormatProperty);
        }

        public static void SetDateFormat(DependencyObject o, string value)
        {
            o.SetValue(DateFormatProperty, value);
        }

        #endregion

        #region IsMonthYearで実行するコールバック関数やイベントハンドラ

        private static void OnIsMonthYearChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomDatePicker datePicker)
            {
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
        }

        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs e)
        {
            if (sender is CustomDatePicker datePicker)
            {
                var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
                if (popup?.Child is Calendar calendar)
                {
                    calendar.DisplayMode = CalendarMode.Year;
                    calendar.DisplayModeChanged += CalendarOnDisplayModeChanged;
                }
            }
        }

        private static void DatePickerOnCalendarClosed(object sender, RoutedEventArgs e)
        {
            if (sender is CustomDatePicker datePicker)
            {
                var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
                if (popup?.Child is Calendar calendar)
                {
                    calendar.DisplayModeChanged -= CalendarOnDisplayModeChanged;
                }
            }
        }

        private static void CalendarOnDisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            var calendar = (Calendar)sender;
            if (calendar.DisplayMode != CalendarMode.Month)
                return;

            calendar.SelectedDate = GetSelectedCalendarDate(calendar.DisplayDate);

            var datePicker = GetCalendarsDatePicker(calendar);
            datePicker.IsDropDownOpen = false;
        }

        private static DatePicker GetCalendarsDatePicker(FrameworkElement child)
        {
            var parent = (FrameworkElement)child.Parent;
            if (parent.Name == "PART_Root")
                return (DatePicker)parent.TemplatedParent;
            return GetCalendarsDatePicker(parent);
        }

        private static DateTime? GetSelectedCalendarDate(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue) return null;

            return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, 1);
        }

        #endregion

        #region DateFormatで実行するコールバック関数やイベントハンドラ

        private static void OnDateFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)o;

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Loaded, new Action<DatePicker>(ApplyDateFormat), datePicker);
        }

        private static void ApplyDateFormat(DatePicker datePicker)
        {
            var binding = new Binding("SelectedDate")
            {
                RelativeSource = new RelativeSource { AncestorType = typeof(DatePicker) },
                Converter = new DatePickerDateTimeConverter(),
                ConverterParameter = new Tuple<DatePicker, string>(datePicker, GetDateFormat(datePicker)),
                StringFormat = GetDateFormat(datePicker)
            };

            var textBox = GetTemplateTextBox(datePicker);
            textBox.SetBinding(TextBox.TextProperty, binding);

            textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
            textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;

            var dropDownButton = GetTemplateButton(datePicker);

            datePicker.CalendarOpened -= DatePickerOnCalendarOpenedFormat;
            datePicker.CalendarOpened += DatePickerOnCalendarOpenedFormat;

            // テキストボックスがちらつく問題を防ぐために、ドロップダウンボタンのPreviewMouseUpを処理する
            dropDownButton.PreviewMouseUp -= DropDownButtonPreviewMouseUp;
            dropDownButton.PreviewMouseUp += DropDownButtonPreviewMouseUp;
        }

        private static ButtonBase GetTemplateButton(DatePicker datePicker)
        {
            return (ButtonBase)datePicker.Template.FindName("PART_Button", datePicker);
        }

        private static void DropDownButtonPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // パターンマッチングを使用して型チェックと変数への代入を行う
            if (!(sender is FrameworkElement fe)) return;
            // 親要素を遡ってDatePickerを検索
            var datePicker = fe.TryFindParent<DatePicker>();

            if (datePicker == null || datePicker.SelectedDate == null) return;

            // カレンダーを開閉するボタンを取得
            var dropDownButton = GetTemplateButton(datePicker);

            // イベントの発生がドロップダウンボタンであり、ドロップダウンカレンダーが閉じていた場合
            // WPFではイベントが伝播し、同じイベントに対して複数のオブジェクトが反応することがある
            if (e.OriginalSource == dropDownButton && datePicker.IsDropDownOpen == false)
            {
                // カレンダーを表示する
                datePicker.SetCurrentValue(DatePicker.IsDropDownOpenProperty, true);

                // 日付を表示する
                datePicker.SetCurrentValue(DatePicker.DisplayDateProperty, datePicker.SelectedDate.Value);

                // ドロップダウンボタンがマウスのキャプチャ（マウスイベントを独占的に受け取る状態）を解放する
                dropDownButton.ReleaseMouseCapture();

                // イベントの終了処理
                // DatePickerの標準的なイベント処理が実行されるのを防ぐ
                e.Handled = true;
            }
        }

        private static TextBox GetTemplateTextBox(Control control)
        {
            control.ApplyTemplate();
            return (TextBox)control?.Template?.FindName("PART_TextBox", control);
        }

        private static void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;

            e.Handled = true;

            var textBox = (TextBox)sender;
            var datePicker = (DatePicker)textBox.TemplatedParent;
            var dateStr = textBox.Text;
            var formatStr = GetDateFormat(datePicker);
            datePicker.SelectedDate = DatePickerDateTimeConverter.StringToDateTime(datePicker, formatStr, dateStr);
        }

        private static void DatePickerOnCalendarOpenedFormat(object sender, RoutedEventArgs e)
        {
            var datePicker = (DatePicker)sender;
            var textBox = GetTemplateTextBox(datePicker);
            var formatStr = GetDateFormat(datePicker);
            textBox.Text = DatePickerDateTimeConverter.DateTimeToString(formatStr, datePicker.SelectedDate);
        }

        #endregion

    }
}
