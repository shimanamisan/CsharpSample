using System;

namespace WPF.GlobalExceptionHandler
{
    /// <summary>
    /// 例外の抽象クラスを作成
    /// </summary>
    public abstract class BaseException : Exception
    {
        /// <summary>
        /// サブクラスに実装を矯正させる
        /// </summary>
        public abstract ExceptonKind Kind { get; }

        /// <summary>
        /// エクセプションの区分
        /// </summary>
        public enum ExceptonKind
        {
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// コンストラクター（インナーエクセプションも受ける場合）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public BaseException(string message, Exception exception)
            // base コンストラクタ初期化子
            // 基底クラス（ここではException）のコンストラクタを明示的に呼び出す
            : base(message, exception)
        { }

        /// <summary>
        /// コンストラクター（メッセージのみの場合）
        /// </summary>
        /// <param name="message"></param>
        public BaseException(string message) : base(message)
        { }
    }
}
