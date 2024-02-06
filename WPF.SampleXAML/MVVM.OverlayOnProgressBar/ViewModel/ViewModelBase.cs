using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.OverlayOnProgressBar.ViewModel
{
  internal class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// プロパティの変更を検知するメソッド
    /// </summary>
    /// <param name="property"></param>
    protected void OnPropertyChanged([CallerMemberName] string property = null)
    {
      // ここでのthisは継承先で実行したクラス（MainWindowViewModelで継承して実行したならそのクラスがthis）
      // Null演算子を使用
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
      if (object.Equals(storage, value)) return false;

      storage = value;
      this.OnPropertyChanged(propertyName);
      return true;
    }
  }
}
