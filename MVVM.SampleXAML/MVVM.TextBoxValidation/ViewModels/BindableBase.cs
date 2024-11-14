using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM.TextBoxValidation.ViewModels
{

    /// <summary>
    /// プロパティ変更通知基底クラス
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region 継承先で使用する実装

        /// <summary>
        /// エラーメッセージを格納するディクショナリ
        /// </summary>
        protected Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        /// <summary>
        /// プロパティごとのバリデーションロジックを実装する
        /// </summary>
        /// <param name="propertyName"></param>
        protected abstract void ValidateProperty(string propertyName);

        /// <summary>
        /// 指定されたプロパティ名とエラーメッセージをエラーリストに追加する
        /// </summary>
        /// <param name="propertyName">エラーを追加するプロパティの名前</param>
        /// <param name="error">追加するエラーメッセージ</param>
        protected void AddError(string propertyName, string error)
        {
            // 指定したキー（propertyName）に対応する値が存在するかチェック
            // キーが存在すれば true を返し対応する値を out パラメータに割り当てる
            // キーが無ければ false
            // propertyName に対応するキーが無ければ新しくエラーリストを作成
            if (!_errors.TryGetValue(propertyName, out var errorList))
            {
                errorList = new List<string>();
                _errors[propertyName] = errorList;
            }

            // リストにエラーメッセージが含まれていなかったら場合はメッセージを追加しエラーイベントを通知
            if (!errorList.Contains(error))
            {
                errorList.Add(error);

                // エラーが追加されたことを通知する
                OnErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// 指定したプロパティ名のエラーをクリアする
        /// </summary>
        /// <param name="propertyName">エラーをクリアするプロパティ名</param>
        protected void ClearErrors(string propertyName)
        {
            if(_errors.TryGetValue(propertyName, out List<string>? errorList) && errorList != null)
            {
                errorList.Clear();

                // エラーがクリアされたことを通知する
                OnErrorsChanged(propertyName);
            }
        }

        #endregion

        /// <summary>
        /// 指定したプロパティ名に関連するエラーが変更されたときに発生するイベントを発行する
        /// </summary>
        /// <param name="propertyName">エラーが変更されたプロパティの名前</param>
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #region INotifyDataErrorInfoインターフェース実装

        /// <summary>
        /// エラーが存在するかどうかを取得する
        /// </summary>
        /// <returns>エラーが存在する場合はtrue、存在しない場合はfalseを返す</returns>
        public bool HasErrors => _errors.Any();

        /// <summary>
        /// イベントハンドラー
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        /// 指定したプロパティ名に関するエラーを取得する
        /// </summary>
        /// <param name="propertyName">エラーを取得したいプロパティ名</param>
        /// <returns>プロパティ名に関連するエラーのリスト</returns>
        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return Enumerable.Empty<string>();

            if (_errors.TryGetValue(propertyName, out List<string>? errorList))
            {
                return errorList ?? Enumerable.Empty<string>();
            }

            return Enumerable.Empty<string>();
        }

        #endregion

        #region データバインディングに必要な処理

        /// <summary>
        /// プロパティ変更通知イベント
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        
        /// <summary>
        /// 値が変更されていた場合にのみOnPropertyChangedを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">変更前のプロパティの値</param>
        /// <param name="value">変更後のプロパティの値</param>
        /// <param name="propertyName">変更されたプロパティ名</param>
        /// <returns>真偽値</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, bool validate = false, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            if(validate && propertyName != null)
            {
                ValidateProperty(propertyName);
            }

            return true;
        }

        /// <summary>
        /// プロパティの変更を検知するメソッド
        /// </summary>
        /// <param name="property"></param>
        protected void OnPropertyChanged([CallerMemberName] string? property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
