using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input; // 追加
using System.IO;
using System; // 追加

namespace MVVM.SampleMahApps.Metro
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        // 追加
        public ICommand ExecuteFetchTextDataButton { get; private set; }

        // プロパティの変更時に発火する
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _textBoxValue;

        public string? TextBoxValue
        {
            get { return _textBoxValue; }
            set
            {
                // 現在の値と変更があった場合
                if (_textBoxValue != value)
                {
                    // 変更があった値をプロパティに代入
                    _textBoxValue = value;
                    // TextBoxValueプロパティに変更があったことを通知する
                    OnPropertyChanged();
                }
            }
        }

        // 追加
        public MainWindowViewModel()
        {
            ExecuteFetchTextDataButton = new DelegateCommand<object>(FetchTextData);
        }

        // 追加
        private void FetchTextData(object? obj)
        {
            FetchTextModel model = new FetchTextModel(Path.Combine(Directory.GetCurrentDirectory(), "TextData.txt"));

            string result = model.FetchTextData();

            if(result != string.Empty) TextBoxValue = result;
        }

        /// <summary>
        /// PropertyChangedEventArgsを発火させる
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
