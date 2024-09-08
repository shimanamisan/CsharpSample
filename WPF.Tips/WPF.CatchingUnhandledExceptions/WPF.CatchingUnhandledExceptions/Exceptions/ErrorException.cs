using System;

namespace WPF.CatchingUnhandledExceptions
{
    public sealed class ErrorException : BaseException
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="message">出力メッセージ</param>
        /// <param name="exception">インナーエクセプション</param>
        public ErrorException(string message, Exception exception) : base(message, exception)
        { }

        /// <summary>
        /// 例外の区分
        /// </summary>
        public override ExceptonKind Kind => ExceptonKind.Error;
    }
}
