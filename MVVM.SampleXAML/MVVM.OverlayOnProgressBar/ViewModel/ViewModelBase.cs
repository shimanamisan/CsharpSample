using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM.OverlayOnProgressBar.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティ変更通知イベント
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// プロパティの変更を検知するメソッド
        /// </summary>
        /// <param name="property"></param>
        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// 値が変更されていた場合にのみOnPropertyChangedを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">変更前のプロパティの値</param>
        /// <param name="value">変更後のプロパティの値</param>
        /// <param name="propertyName">変更されたプロパティ名</param>
        /// <returns>真偽値</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
