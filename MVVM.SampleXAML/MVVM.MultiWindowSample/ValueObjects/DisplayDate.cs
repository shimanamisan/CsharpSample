using MVVM.MultiWindowSample.Extensions;

namespace MVVM.MultiWindowSample.ValueObjects
{
    public sealed class DisplayDate : ValueObject<DisplayDate>
    {
        /// <summary>
        /// 西暦表示（加工なし）
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 西暦の日付を和暦「"令和●年xx月xx日"」に変換
        /// </summary>
        public string DisplayJapaneseEraFullFromat => Value.ConvertJapaneseEraTodateFullFromat();

        /// <summary>
        /// 西暦の日付を和暦「"令和●年度"」に変換
        /// </summary>
        public string DisplayJapaneseEraFiscalYear => Value.ConvertJapaneseEraToFiscalYear();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DisplayDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("日付を入力してください。");
            }

            Value = value;
        }

        /// <summary>
        /// 値の比較用メソッド
        /// </summary>
        protected override bool EqualsCore(DisplayDate other)
        {
            return Value == other.Value;
        }
    }
}
