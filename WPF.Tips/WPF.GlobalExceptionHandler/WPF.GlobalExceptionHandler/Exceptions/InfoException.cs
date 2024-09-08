namespace WPF.GlobalExceptionHandler
{
    public sealed class InfoException : BaseException
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        public InfoException() : base("情報です。")
        { }

        /// <summary>
        /// 例外の区分
        /// </summary>
        public override ExceptonKind Kind => ExceptonKind.Info;
    }
}
