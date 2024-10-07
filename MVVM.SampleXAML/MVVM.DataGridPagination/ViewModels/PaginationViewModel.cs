using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace MVVM.DataGridPagination.ViewModels
{
    /// <summary>
    /// ページネーション機能を提供するViewModelクラス
    /// </summary>
    public sealed class PaginationViewModel : BindableBase
    {
        #region アクション

        /// <summary>
        /// ページが変更されたときに実行するアクション
        /// </summary>
        public event Action<int> PageChanged;

        #endregion

        #region プロパティ

        /// <summary>
        /// 現在のページ番号を取得または設定する
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    UpdatePageNumbers();

                    // アクションを実行（登録されたメソッドが実行される）
                    PageChanged?.Invoke(value);
                }
            }
        }
        private int _currentPage = 1;

        /// <summary>
        /// 総ページ数を取得または設定する
        /// </summary>
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                if (SetProperty(ref _totalPages, value))
                {
                    UpdatePageNumbers();
                }
            }
        }
        private int _totalPages;

        /// <summary>
        /// 表示するページ数を取得または設定する
        /// </summary>
        public int DisplayedPageCount
        {
            get => _displayedPageCount;
            set => SetProperty(ref _displayedPageCount, value);
        }
        private int _displayedPageCount = 5;

        /// <summary>
        /// ページネーションデータを取得または設定する
        /// </summary>
        public ObservableCollection<object> PageNumbers
        {
            get => _pageNumbers;
            set => SetProperty(ref _pageNumbers, value);
        }
        private ObservableCollection<object> _pageNumbers = new ObservableCollection<object>();

        #endregion

        #region コマンド

        /// <summary>
        /// 前のページに移動するコマンド
        /// </summary>
        public DelegateCommand PreviousPageCommand
            => _previousPageCommand ?? (_previousPageCommand = new DelegateCommand(PreviousPage, CanPreviousPage)
                                                                                   .ObservesProperty(() => CurrentPage));
        private DelegateCommand _previousPageCommand;

        /// <summary>
        /// 次のページに移動するコマンド
        /// </summary>
        public DelegateCommand NextPageCommand
            => _nextPageCommand ?? (_nextPageCommand = new DelegateCommand(NextPage, CanNextPage)
                                                                          .ObservesProperty(() => CurrentPage));
        private DelegateCommand _nextPageCommand;

        /// <summary>
        /// 指定したページに移動するコマンド
        /// </summary>
        public DelegateCommand<object> GoToPageCommand
            => _goToPageCommand ?? (_goToPageCommand = new DelegateCommand<object>(ExecuteGoToPage));
        private DelegateCommand<object> _goToPageCommand;

        #endregion

        /// <summary>
        /// PaginationViewModelクラスの新しいインスタンスを初期化する
        /// </summary>
        public PaginationViewModel()
        {
            UpdatePageNumbers();
        }

        /// <summary>
        /// 指定したページに移動するコマンドを実行する
        /// </summary>
        /// <param name="parameter">移動先のページ番号</param>
        private void ExecuteGoToPage(object parameter)
        {
            if (parameter is int page)
            {
                GoToPage(page);
            }
        }

        /// <summary>
        /// 指定したページに移動する
        /// </summary>
        /// <param name="page">移動先のページ番号</param>
        private void GoToPage(int page) => CurrentPage = page;

        /// <summary>
        /// 次のページに移動可能かどうかを判定する
        /// </summary>
        /// <returns>次のページに移動可能な場合はtrue それ以外はfalse</returns>
        private bool CanNextPage() => CurrentPage < TotalPages;

        /// <summary>
        /// 次のページに移動する
        /// </summary>
        private void NextPage() => CurrentPage++;

        /// <summary>
        /// 前のページに移動する
        /// </summary>
        private void PreviousPage() => CurrentPage--;

        /// <summary>
        /// 前のページに移動可能かどうかを判定する
        /// </summary>
        /// <returns>前のページに移動可能な場合はtrue それ以外はfalse</returns>
        private bool CanPreviousPage() => CurrentPage > 1;

        /// <summary>
        /// ページ番号の表示を更新する
        /// </summary>
        private void UpdatePageNumbers()
        {
            PageNumbers.Clear(); // 既存の表示しているページ番号を消去する
            int middleItems = 5; // 中央に表示するページ番号の数

            // CurrentPage - middleItems / 2 は 現在のページを基準にページ番号を配置する
            // Math.Min と Math.Max を使用して 開始位置が1未満にならず かつ終了位置が総ページ数を超えないようにする
            int start = Math.Max(1, Math.Min(CurrentPage - middleItems / 2, TotalPages - middleItems + 1));

            // 開始位置から middleItems 分だけ進んだ位置を終了位置とする
            // Math.Min を使用して 終了位置が総ページ数を超えないようにする
            int end = Math.Min(start + middleItems - 1, TotalPages);

            // 開始部分の処理
            if (start > 1)
            {
                // 最初のページ（1）を常に表示する
                AddUniqueItem(1);

                // 開始位置が3以上の場合 省略記号（...）を追加する
                if (start > 2)
                {
                    AddUniqueItem("...");
                }
            }

            // 中央部分のページ番号を追加
            for (int i = start; i <= end; i++)
            {
                // 計算された start から end までのページ番号を順に追加する
                AddUniqueItem(i);
            }

            // 終了部分の処理
            if (end < TotalPages)
            {
                // 終了位置が最後から2ページ以上前の場合 省略記号（...）を追加する
                if (end < TotalPages - 1)
                {
                    AddUniqueItem("...");
                }

                // 最後のページを常に表示する
                AddUniqueItem(TotalPages);
            }
        }

        /// <summary>
        /// 重複したページ番号を追加しないようにする
        /// </summary>
        /// <param name="item">追加するページ番号または省略記号</param>
        private void AddUniqueItem(object item)
        {
            if (!PageNumbers.Contains(item))
            {
                PageNumbers.Add(item);
            }
        }
    }
}
