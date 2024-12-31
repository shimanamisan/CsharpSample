namespace MVVM.MultiWindowSample.Servicies
{
    /// <summary>
    /// 画面の結果を提供するためのインターフェース
    /// </summary>
    public interface IResultProvider<TResult>
    {
        /// <summary>
        /// 結果を取得する
        /// </summary>
        /// <returns>画面の結果</returns>
        TResult? GetResult();
    }
}
