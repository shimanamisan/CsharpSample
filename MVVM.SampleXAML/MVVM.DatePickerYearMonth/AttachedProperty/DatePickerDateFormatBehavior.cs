using MVVM.DatePickerYearMonth.Converter;
using MVVM.DatePickerYearMonth.DatePickerExtensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVM.DatePickerYearMonth.AttachedProperty
{
    /// <summary>
    /// DatePickerコントロールの日付表示形式をカスタマイズするためのクラス
    /// </summary>
    public class DatePickerDateFormatBehavior
    {

        /// <summary>
        /// DatePickerにカスタム日付フォーマットを適用するための依存関係プロパティを定義します
        /// </summary>
        public static readonly DependencyProperty DateFormatProperty =
            DependencyProperty.RegisterAttached(
                    "DateFormat",
                    typeof(string),
                    typeof(DatePickerDateFormatBehavior),
                    new PropertyMetadata(OnDateFormatChanged));

        public static string GetDateFormat(DependencyObject o)
        {
            return (string)o.GetValue(DateFormatProperty);
        }

        public static void SetDateFormat(DependencyObject o, string value)
        {
            o.SetValue(DateFormatProperty, value);
        }

        private static void OnDateFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
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
                    new Action<DatePicker>(ApplyDateFormat), datePicker);
        }

        /// <summary>
        /// 指定されたDatePickerにカスタム日付フォーマットを適用します
        /// </summary>
        /// <param name="datePicker">フォーマットを適用するDatePicker</param>
        private static void ApplyDateFormat(DatePicker datePicker)
        {
            // DatePickerコントロールのSelectedDateプロパティに対するバインディングを設定する
            var binding = new Binding("SelectedDate")
            {
                // バインディングのソースとしてDatePickerコントロールを指定する
                RelativeSource = new RelativeSource { AncestorType = typeof(DatePicker) },
                // 日付のフォーマットをカスタマイズする独自のコンバーターを指定
                Converter = new DatePickerDateTimeConverter(),
                // コンバーターに渡すパラメータを指定
                ConverterParameter = new Tuple<DatePicker, string>(datePicker, GetDateFormat(datePicker)),
                Mode = BindingMode.TwoWay // 双方向バインディングを設定
            };

            // DatePickerのコントロールテンプレートからテキストボックスを取得する
            var textBox = GetTemplateTextBox(datePicker);
            // テキストボックスのプロパティに生成した Binding("SelectedDate") を指定
            textBox.SetBinding(TextBox.TextProperty, binding);

            // Textbox選択時、エンターキーで発生するイベントに対しても処理を追加
            textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
            textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;

            // DatePickerのボタン要素を検索
            var dropDownButton = GetTemplateButton(datePicker);

            datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
            datePicker.CalendarOpened += DatePickerOnCalendarOpened;

            // テキストボックスがちらつく問題を防ぐために、ドロップダウンボタンのPreviewMouseUpを処理する。
            dropDownButton.PreviewMouseUp -= DropDownButtonPreviewMouseUp;
            dropDownButton.PreviewMouseUp += DropDownButtonPreviewMouseUp;
        }

        private static ButtonBase GetTemplateButton(DatePicker datePicker)
        {
            // DatePickerのコントロールテンプレートから PART_Button という名前の要素を検索
            // PART_ButtonはCalendarの開閉をするボタン
            return (ButtonBase)datePicker.Template.FindName("PART_Button", datePicker);
        }

        private static TextBox GetTemplateTextBox(Control control)
        {
            // コントロールテンプレートが遅延ロードされていた場合、ビジュアルツリーを強制的に生成
            //control.ApplyTemplate();

            // DatePickerのコントロールテンプレートから PART_TextBox という名前の要素を検索
            return (TextBox)control?.Template?.FindName("PART_TextBox", control);
        }

        /// <summary>
        /// DatePickerのドロップダウンボタンがクリックされたときのイベントハンドラ
        /// ドロップダウンボタンのクリックによるテキストボックスの点滅問題を防止する
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
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

        /// <summary>
        /// TextBoxのPreviewKeyDownイベントハンドラ
        /// Enterキーが押された際にDatePickerのSelectedDateを設定する
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;

            /* 
             * DatePickerは、Key.Returnが押された場合、テキストボックスのKeyDownイベントを購読してSelectedDateを設定します。
             * が押された場合、そのSelectedDateを設定する。この場合、フォーカスが外れるか、
             * 別の日付が選択されるまで、そのテキストは内部の日付解析の結果になります。
             * フォーカスを失うか、別の日付が選択されるまで、テキストは内部の日付解析の結果になります。
             * 回避策としては、KeyDownイベントがバブリングされるのを止めることです。
             * DatePicker.SelectedDateの設定を処理することです。
             */

            e.Handled = true;

            var textBox = (TextBox)sender;
            var datePicker = (DatePicker)textBox.TemplatedParent;
            var dateStr = textBox.Text;
            var formatStr = GetDateFormat(datePicker);
            datePicker.SelectedDate = DatePickerDateTimeConverter.StringToDateTime(datePicker, formatStr, dateStr);
        }

        /// <summary>
        /// DatePickerのカレンダーが開かれたときのイベントハンドラ
        /// カレンダーが開かれた際に、カスタムフォーマットの日付文字列を設定する
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs e)
        {
            /* DatePickerのTextBoxにフォーカスが当たっていない状態で、カレンダーボタンをクリックしてカレンダーを開くと、
             * TextBoxにフォーカスが当たって別の日付が選択されるまで、そのテキストは内部の日付解析の結果になります。
             * 回避策として、開くときにこの文字列を設定します。
             */

            var datePicker = (DatePicker)sender;
            var textBox = GetTemplateTextBox(datePicker);
            var formatStr = GetDateFormat(datePicker);
            textBox.Text = DatePickerDateTimeConverter.DateTimeToString(formatStr, datePicker.SelectedDate);
        }
    }
}