namespace MVVM.MultiWindowSample.ValueObjects
{
    /// <summary>
    /// ValueObjectの基底クラス（値比較用）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueObject<T> where T : ValueObject<T> // ValueObjectを継承しているクラスを制約する
    {
        /// <summary>
        /// ValueObjectを継承しているクラスの値を比較する
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // 引数で渡ってきたオブジェクトが変換出来るか確認する（ジェネリックで指定されたクラス）
            // 変換できれば同じクラス（AreaIdなど）
            // 変換できなければ null が返却され
            var vo = obj as T;

            // 変換できなければ違うクラスと判定
            if (vo == null)
            {
                return false;
            }

            // 基底クラスにはValueプロパティは無いので継承側のクラスで実装してもらう
            return EqualsCore(vo);
        }

        /// <summary>
        /// EqualsCore
        /// </summary>
        /// <param name="other">other</param>
        /// <returns></returns>
        protected abstract bool EqualsCore(T other);

        /// <summary>
        /// イコールの演算子をクラス比較用にオーバーライド
        /// ※ == をオーバーライドすると != もオーバーライドしないとコンパイルエラーになる
        /// </summary>
        /// <param name="vo1"></param>
        /// <param name="vo2"></param>
        /// <returns></returns>
        public static bool operator ==(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            return Equals(vo1, vo2);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="vo1"></param>
        /// <param name="vo2"></param>
        /// <returns></returns>
        public static bool operator !=(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            return !Equals(vo1, vo2);
        }

        /// <summary>
        /// ToString
        /// おまじない的実装
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // 演算子をオーバーライドした際にこのメソッドもオーバーライドしないと警告が出る
            return base.ToString();
        }

        /// <summary>
        /// GetHashCode
        /// おまじない的実装
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // 演算子をオーバーライドした際にこのメソッドもオーバーライドしないと警告が出る
            return base.GetHashCode();
        }
    }
}
