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

        /// <summary>
        /// IsMonthYearプロパティのCLRラッパー
        /// </summary>
        public bool IsMonthYearDP
        {
            get => (bool)GetValue(IsMonthYearProperty);
            set => SetValue(IsMonthYearProperty, value);
        }

        #endregion

        #region DateFormatDPプロパティのCLRラッパー

        /// <summary>
        /// DateFormatDPプロパティのCLRラッパー
        /// </summary>
        public string DateFormatDP
        {
            get => (string)GetValue(DateFormatProperty);
            set => SetValue(DateFormatProperty, value);
        }

        #endregion

        #region DateFormat依存関係プロパティの定義, 取得と設定

        /// <summary>
        ///  DateFormatプロパティの値を取得
        /// </summary>
        /// <param name="o">値を取得する DependencyObject</param>
        /// <returns>IsMonthYearProperty の値 (bool)</returns>
        public static string GetDateFormat(DependencyObject o)
        {
            return (string)o.GetValue(DateFormatProperty);
        }

        /// <summary>
        /// DateFormatプロパティの値を設定
        /// </summary>
        /// <param name="o">プロパティ値を設定するDependencyObject</param>
        /// <param name="value">設定する文字列値</param>
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

        /// <summary>
        /// 指定された sender から DatePicker のカレンダーを取得する
        /// </summary>
        /// <param name="sender">カレンダーを取得する元となるオブジェクト</param>
        /// <returns>DatePicker のカレンダー</returns>
        private static Calendar GetDatePickerCalendar(object sender)
        {
            var datePicker = (DatePicker)sender;
            var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);
            return ((Calendar)popup.Child);
        }

        /// <summary>
        /// 指定された FrameworkElement から DatePicker を取得する
        /// </summary>
        /// <param name="child">DatePicker を取得する元となる FrameworkElement</param>
        /// <returns>関連する DatePicker</returns>
        private static DatePicker GetCalendarsDatePicker(FrameworkElement child)
        {
            var parent = (FrameworkElement)child.Parent;
            if (parent.Name == "PART_Root")
                return (DatePicker)parent.TemplatedParent;
            return GetCalendarsDatePicker(parent);
        }

        /// <summary>
        /// 指定された日付から年月のみを使用した DateTime? を取得する
        /// </summary>
        /// <param name="selectedDate">基準となる日付</param>
        /// <returns>年月のみを考慮した DateTime?</returns>
        private static DateTime? GetSelectedCalendarDate(DateTime? selectedDate)
        {
            if (!selectedDate.HasValue) return null;

            return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, 1);
        }

        #endregion

        #region DateFormatで実行するコールバック関数やイベントハンドラ

        /// <summary>
        /// DateFormatプロパティが変更されたときに呼び出されるメソッドです
        /// </summary>
        /// <param name="o">イベントが発生したDependencyObject</param>
        /// <param name="e">イベントの詳細情報</param>
        private static void OnDateFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)o;

            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Loaded, new Action<DatePicker>(ApplyDateFormat), datePicker);
        }

        /// <summary>
        /// 指定されたDatePickerにカスタム日付フォーマットを適用します
        /// </summary>
        /// <param name="datePicker">フォーマットを適用するDatePicker</param>
        private static void ApplyDateFormat(DatePicker datePicker)
        {
            var binding = new Binding("SelectedDate")
            {
                RelativeSource = new RelativeSource { AncestorType = typeof(DatePicker) },
                Converter = new DatePickerDateTimeConverter(),
                ConverterParameter = new Tuple<DatePicker, string>(datePicker, GetDateFormat(datePicker)),
                StringFormat = GetDateFormat(datePicker) // This is also new but didnt seem to help
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

        /// <summary>
        /// DatePickerのテンプレートからButtonBaseを取得します
        /// </summary>
        /// <param name="datePicker">ButtonBaseを検索するDatePicker</param>
        /// <returns>DatePickerのテンプレート内のButtonBase</returns>
        private static ButtonBase GetTemplateButton(DatePicker datePicker)
        {
            return (ButtonBase)datePicker.Template.FindName("PART_Button", datePicker);
        }


        /// <summary>
        /// DatePickerのドロップダウンボタンがクリックされたときのイベントハンドラです
        /// ドロップダウンボタンのクリックによるテキストボックスの点滅問題を防止します
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void DropDownButtonPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var datePicker = fe.TryFindParent<DatePicker>();
            if (datePicker == null || datePicker.SelectedDate == null) return;

            var dropDownButton = GetTemplateButton(datePicker);

            // Dropdown button was clicked
            if (e.OriginalSource == dropDownButton && datePicker.IsDropDownOpen == false)
            {
                // ドロップダウンを開く
                datePicker.SetCurrentValue(DatePicker.IsDropDownOpenProperty, true);

                // 標準の DatePicker ドロップダウンを開く *テキストボックスの値を設定することを除く *他のすべてを模倣する
                datePicker.SetCurrentValue(DatePicker.DisplayDateProperty, datePicker.SelectedDate.Value);

                // カレンダーが機能しない場合は重要
                dropDownButton.ReleaseMouseCapture();

                // datePicker.csがこのイベントを処理しないようにする 
                e.Handled = true;
            }
        }


        /// <summary>
        /// DatePickerのテンプレートからTextBoxを取得します
        /// </summary>
        /// <param name="control">TextBoxを検索するControl</param>
        /// <returns>DatePickerのテンプレート内のTextBox</returns>
        private static TextBox GetTemplateTextBox(Control control)
        {
            control.ApplyTemplate();
            return (TextBox)control?.Template?.FindName("PART_TextBox", control);
        }

        /// <summary>
        /// TextBoxのPreviewKeyDownイベントハンドラです
        /// Enterキーが押された際にDatePickerのSelectedDateを設定します
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;

            /* DatePickerは、Key.Returnが押された場合、SelectedDateを設定するためにTextBoxのKeyDownイベントを購読します。
             * この場合、フォーカスが外れるか、別の日付が選択されるまで、そのテキストは内部の日付解析の結果になります。
             * 回避策は、KeyDownイベントのバブリングを停止することです。DatePicker.SelectedDateの設定を処理することです。*/

            e.Handled = true;

            var textBox = (TextBox)sender;
            var datePicker = (DatePicker)textBox.TemplatedParent;
            var dateStr = textBox.Text;
            var formatStr = GetDateFormat(datePicker);
            datePicker.SelectedDate = DatePickerDateTimeConverter.StringToDateTime(datePicker, formatStr, dateStr);
        }

        /// <summary>
        /// DatePickerのカレンダーが開かれたときのイベントハンドラです
        /// カレンダーが開かれた際に、カスタムフォーマットの日付文字列を設定します
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void DatePickerOnCalendarOpenedFormat(object sender, RoutedEventArgs e)
        {
            /* When DatePicker's TextBox is not focused and its Calendar is opened by clicking its calendar button
             * its text will be the result of its internal date parsing until its TextBox is focused and another
             * date is selected. A workaround is to set this string when it is opened. */

            var datePicker = (DatePicker)sender;
            var textBox = GetTemplateTextBox(datePicker);
            var formatStr = GetDateFormat(datePicker);
            textBox.Text = DatePickerDateTimeConverter.DateTimeToString(formatStr, datePicker.SelectedDate);
        }

        #endregion

    }
}
