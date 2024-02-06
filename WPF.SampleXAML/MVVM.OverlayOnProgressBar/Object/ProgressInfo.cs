namespace MVVM.OverlayOnProgressBar.Object
{
  internal class ProgressInfo
  {
    /// <summary>
    /// 進捗値
    /// </summary>
    public float ProgressValue { get; }

    /// <summary>
    /// 完全コンストラクタパターン
    /// </summary>
    /// <param name="value">進捗値</param>
    public ProgressInfo(float value)
    {
      ProgressValue = value;
    }
  }
}
