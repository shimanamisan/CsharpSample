namespace MVVM.OverlayOnProgressBar.Object
{
    public sealed class ProgressInfo
    {
        /// <summary>
        /// 進捗値
        /// </summary>
        public float ProgressValue { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">進捗値</param>
        public ProgressInfo(float value)
        {
            ProgressValue = value;
        }
    }
}
