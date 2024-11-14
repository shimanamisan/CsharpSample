using System.Globalization;
using System.Windows.Controls;

namespace MVVM.TextBoxValidation.ValidationRules
{
    public class TextBoxRule : ValidationRule
    {
        /// <summary>
        /// XAML側から指定するプロパティ
        /// </summary>
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "入力必須です。");
            }

            if (int.TryParse(value as string, out int number))
            {
                if (number < 0) return new ValidationResult(false, "負の値は入力できません。");

                if (number > Max) return new ValidationResult(false, "最大値を超えています。");

                // 第一引数(bool): チェックした値が有効な値か判定する（true:有効な値と判定、false: 無効な値と判定）
                // 第二引数(object): エラーだった場合のメッセージを指定
                // 以下は値が有効だった場合に返す値の例
                //return new ValidationResult(true, null);

                // 上記と同様の戻り値（値が有効だった場合に返す）
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "数値を入力してください。");
            }
        }
    }
}
