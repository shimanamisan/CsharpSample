using System.Windows;
using System.Windows.Controls;

namespace MVVM.ButtonContextMenu
{
    /// <summary>
    /// MainWindowのViewModel。ウィンドウのコンポーネントのコマンドとインタラクションを処理します。
    /// </summary>
    public sealed class MainWindowViewModel : BindableBase
    {

        #region コマンド

        /// <summary>
        /// 一番目メニューアイテムの実行に関連するコマンド。
        /// </summary>
        public DelegateCommand<object> FirstMenuItemCommand
            => _firstMenuItemCommand ?? (_firstMenuItemCommand = new DelegateCommand<object>(OnFirstMenuItemFirst));
        private DelegateCommand<object> _firstMenuItemCommand;

        /// <summary>
        /// 二番目のメニューアイテムの実行に関連するコマンド。
        /// </summary>
        public DelegateCommand<object> SecondMenuItemCommand
            => _secondMenuItemCommand ?? (_secondMenuItemCommand = new DelegateCommand<object>(OnSecondMenuItem));
        private DelegateCommand<object> _secondMenuItemCommand;

        /// <summary>
        /// ボタンをクリックした際のメニューアイテムの実行に関連するコマンド。
        /// </summary>
        public DelegateCommand<object> OpenContextMenuCommand
            => _openContextMenuCommand ?? (_openContextMenuCommand = new DelegateCommand<object>(OnOpenContextMenu));
        private DelegateCommand<object> _openContextMenuCommand;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        { }

        /// <summary>
        /// メインのボタンを押したときにContextMenuを開く方法
        /// </summary>
        /// <param name="o"></param>
        private void OnOpenContextMenu(object o)
        {
            var button = o as Button;

            if (button != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.IsOpen = true;
            }
        }

        /// <summary>
        /// 最初のメニューアイテムのコマンド実行メソッド。そのチェック状態をトグルし、メッセージボックスを表示します。
        /// </summary>
        /// <param name="o">コマンドを呼び出したメニューアイテムオブジェクト。</param>
        private void OnFirstMenuItemFirst(object o)
        {

            var meuItem = o as MenuItem;

            if (meuItem != null)
            {
                if (!Status.IsMenuItemFirst)
                {
                    Status.IsMenuItemFirst = !Status.IsMenuItemFirst;
                    meuItem.IsChecked = Status.IsMenuItemFirst;

                    MessageBox.Show("メニュー1");

                    return;
                }

                Status.IsMenuItemFirst = !Status.IsMenuItemFirst;
                meuItem.IsChecked = Status.IsMenuItemFirst;

                MessageBox.Show("メニュー1");

            }

        }

        /// <summary>
        /// 二番目のメニューアイテムのコマンド実行メソッド。そのチェック状態をトグルし、メッセージボックスを表示します。
        /// </summary>
        /// <param name="o">コマンドを呼び出したメニューアイテムオブジェクト。</param>
        private void OnSecondMenuItem(object o)
        {
            var meuItem = o as MenuItem;

            if (meuItem != null)
            {

                if (!Status.IsMenuItemSecond)
                {
                    Status.IsMenuItemSecond = !Status.IsMenuItemSecond;
                    meuItem.IsChecked = Status.IsMenuItemSecond;

                    MessageBox.Show("メニュー２");

                    return;
                }

                Status.IsMenuItemSecond = !Status.IsMenuItemSecond;
                meuItem.IsChecked = Status.IsMenuItemSecond;

                MessageBox.Show("メニュー２");

            }

        }

    }

}