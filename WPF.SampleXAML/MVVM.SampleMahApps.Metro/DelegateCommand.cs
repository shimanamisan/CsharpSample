using System;
using System.Windows.Input;

namespace MVVM.SampleMahApps.Metro
{
    /// <summary>
    /// 任意の方を受け取るパターンのDelegateクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : ICommand
    {

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private readonly Action<T?> _execute;

        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action<T?> execute) : this(execute, () => true)
        { }

        public DelegateCommand(Action<T?> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute((T?)parameter);
        }
    }

    /// <summary>
    /// パラメータを渡さない場合のクラス
    /// </summary>
    public class DelegateCommand : ICommand
    {

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private readonly Action _execute;

        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute) : this(execute, () => true)
        { }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute();
        }
    }

}
