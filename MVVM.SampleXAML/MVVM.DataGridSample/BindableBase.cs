using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataGrid_Sample
{
    /// <summary>
    /// プロパティ変更通知をする
    /// </summary>
    public class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティ変更通知イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ変更通知メソッド
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// プロパティを変更する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="storage">プロパティ名（参照渡し）</param>
        /// <param name="value">変更後の値</param>
        /// <param name="propertyName">呼び出し元のプロパティ名</param>
        /// <returns>bool</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
