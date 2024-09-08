using System;

namespace WPF.CatchingUnhandledExceptions
{
    /// <summary>
    /// ゼロの除算に関する例外
    /// </summary>
    public sealed class ToDivisionByZeroException : BaseException
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        /// <param name="exception">インナーエクセプション</param>
        public ToDivisionByZeroException(string message, Exception exception) : base(message, exception)
        { }

        /// <summary>
        /// 例外区分
        /// </summary>
        public override ExceptonKind Kind => ExceptonKind.Error;
    }
}
