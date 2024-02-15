using System;
using System.Windows;
using System.Windows.Controls;

namespace MVVM.ButtonContextMenu
{
    /// <summary>
    /// MainWindowのViewModel。ウィンドウのコンポーネントのコマンドとインタラクションを処理します。
    /// </summary>
    public sealed class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// 最初のメニューアイテムの実行に関連するコマンド。
        /// </summary>
        public DelegateCommand<object> MenuItemExecute { get; }

        /// <summary>
        /// 二番目のメニューアイテムの実行に関連するコマンド。
        /// </summary>
        public DelegateCommand<object> MenuItemExecute2 { get; }

        /// <summary>
        /// ボタンをクリックした際のメニューアイテムの実行に関連するコマンド。
        /// </summary>
        public DelegateCommand<object> ExecuteOpenContextMenu { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            MenuItemExecute = new DelegateCommand<object>(MenuItemFirst);

            MenuItemExecute2 = new DelegateCommand<object>(MenuItemSecond);

            ExecuteOpenContextMenu = new DelegateCommand<object>(OpenContextMenu);

        }

        /// <summary>
        /// メインのボタンを押したときにContextMenuを開く方法
        /// </summary>
        /// <param name="o"></param>
        private void OpenContextMenu(object o)
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
        private void MenuItemFirst(object o)
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
        private void MenuItemSecond(object o)
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