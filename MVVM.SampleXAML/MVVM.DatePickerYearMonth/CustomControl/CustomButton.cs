using System.Windows;
using System.Windows.Controls;

namespace MVVM.DatePickerYearMonth.CustomControl
{
    public class CustomButton : Button
    {
        /// <summary>
        /// 依存関係プロパティの定義
        /// コードビハインドからアクセスする場合はこの名前を使用する
        /// </summary>
        public static readonly DependencyProperty IsShowMessageProperty =
            DependencyProperty.Register(
                // 登録するプロパティの名前
                "IsShowMessageDP",
                // プロパティに指定する値の型
                typeof(bool),
                // この依存関係プロパティの所有者
                typeof(CustomButton),
                // プロパティのメタデータ
                // 初期値とプロパティの値が変更された際に実行される処理を記述する（コールバック関数はnullでも可）
                new PropertyMetadata(false, OnIsShowMessageChanged));

        /// <summary>
        /// XAML側からアクセスするためのプロパティ
        /// </summary>
        public bool IsShowMessageDP
        {
            get => (bool)GetValue(IsShowMessageProperty);
            set => SetValue(IsShowMessageProperty, value);
        }

        /// <summary>
        /// 値変更時に実行される振る舞い
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIsShowMessageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is Button button)
            {

                if ((bool)e.NewValue)
                {
                    button.Click += OnButtonClick;
                }
                else
                {
                    button.Click -= OnButtonClick;
                }
            }
        }

        /// <summary>
        /// ボタンクリック時に実行されるメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button clicked!");
        }
    }
}
