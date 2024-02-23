using System.Windows;
using System.Windows.Controls;

namespace MVVM.DatePickerYearMonth.AttachedProperty
{
    public class ButtonExtends
    {
      
        public static readonly DependencyProperty IsShowMessagePropertyAT =
            // 添付プロパティを登録するにはRegisterAttachedメソッドを使用する
            DependencyProperty.RegisterAttached(
                // 登録するプロパティの名前
                "IsShowMessageAT",
                typeof(bool),
                typeof(ButtonExtends),
                new PropertyMetadata(false, OnIsShowMessageChanged));

        /// <summary>
        /// ゲッターには登録したプロパティ名にGetを命名する必要がある
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool GetIsShowMessageAT(DependencyObject o)
            => (bool)o.GetValue(IsShowMessagePropertyAT);

        /// <summary>
        /// ゲッターと同様の命名規則
        /// </summary>
        /// <param name="o"></param>
        /// <param name="value"></param>
        public static void SetIsShowMessageAT(DependencyObject o, bool value)
           => o.SetValue(IsShowMessagePropertyAT, value);

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

        private static void OnButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("AttachedProperty!");
        }
    }
}
