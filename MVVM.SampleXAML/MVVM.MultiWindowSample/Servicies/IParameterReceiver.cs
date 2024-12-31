namespace MVVM.MultiWindowSample.Servicies
{
    /// <summary>
    /// 画面の呼び出し時、パラメーターを受け取るためのインターフェース
    /// </summary>
    public interface IParameterReceiver
    {
        /// <summary>
        /// パラメーターを受け取る
        /// </summary>
        /// <param name="parameter">オブジェクト</param>
        void ReceiveParameter(object parameter);
    }
}
