using MVVM.DataGridPagination.Entities;
using MVVM.DataGridPagination.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.DataGridPagination.ViewModels
{
    /// <summary>
    /// メインウィンドウのViewModelクラス
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private readonly IUsersRepository<UserEntity> _usersRepository;

        #region プロパティ

        /// <summary>
        /// PaginationViewModelを取得または設定する
        /// </summary>
        public PaginationViewModel PaginationVM
        {
            get => _paginationVM;
            set => SetProperty(ref _paginationVM, value);
        }
        private PaginationViewModel _paginationVM;

        /// <summary>
        /// 検索テキストが空かどうかを取得する
        /// </summary>
        public bool IsTextEmpty => string.IsNullOrWhiteSpace(SearchText);

        /// <summary>
        /// 検索テキストを取得または設定する
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    RaisePropertyChanged(nameof(IsTextEmpty));
                    FilterItems();
                }
            }
        }
        private string _searchText;

        private ObservableCollection<UserEntity> _originalMemberData;

        /// <summary>
        /// フィルタリングされたメンバーデータを取得または設定する
        /// </summary>
        public ObservableCollection<UserEntity> FilterMemberData
        {
            get => _filterMemberData;
            set => SetProperty(ref _filterMemberData, value);
        }
        private ObservableCollection<UserEntity> _filterMemberData;

        /// <summary>
        /// タイトルを取得または設定する
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private string _title = "Prism Application";

        #endregion

        #region コマンド

        /// <summary>
        /// 初期化を非同期で実行するコマンド
        /// </summary>
        public DelegateCommand InitializeAsyncCommand =>
           _initializeAsyncCommand ?? (new DelegateCommand(async () => await InitializeAsync()));
        private DelegateCommand _initializeAsyncCommand;

        /// <summary>
        /// アプリケーションを終了するコマンド
        /// </summary>
        public DelegateCommand AppCloseCommand =>
            _appCloseCommand ?? (new DelegateCommand(() => Application.Current.Shutdown()));
        private DelegateCommand _appCloseCommand;

        /// <summary>
        /// 検索を実行するコマンド
        /// </summary>
        public DelegateCommand SearchCommand =>
           _searchCommand ?? (new DelegateCommand(OnSearch));
        private DelegateCommand _searchCommand;

        #endregion

        /// <summary>
        /// 1ページで表示するデータの数
        /// </summary>
        private const int PageSize = 500;

        /// <summary>
        /// MainWindowViewModelクラスの新しいインスタンスを初期化する
        /// </summary>
        /// <param name="usersRepository">Userリポジトリインターフェース</param>
        public MainWindowViewModel(IUsersRepository<UserEntity> usersRepository)
        {
            _usersRepository = usersRepository;

            PaginationVM = new PaginationViewModel();
            // PaginationViewModel側の Action で通知されたときに実行されるメソッドを登録
            PaginationVM.PageChanged += OnPageChange;
        }

        /// <summary>
        /// ページが変更されたときに呼び出されるメソッド
        /// </summary>
        /// <param name="page">新しいページ番号</param>
        private async void OnPageChange(int page)
        {
            await LoadPage(page);
        }

        /// <summary>
        /// 指定されたページのデータを読み込む
        /// </summary>
        /// <param name="page">読み込むページ番号</param>
        private async Task LoadPage(int page)
        {
            // 総レコード数を取得する
            var totalCount = await _usersRepository.GetUsersAsync();

            // ページネーション用の設定を行う
            PaginationVM.TotalPages = (int)Math.Ceiling((double)totalCount.Count() / PageSize);
            PaginationVM.CurrentPage = page;

            // 指定されたページのデータを取得する
            var enumerableData = await _usersRepository.GetPaginateAsync(page, PageSize);

            FilterMemberData = new ObservableCollection<UserEntity>(enumerableData);
        }

        /// <summary>
        /// 初期化を非同期で実行する
        /// </summary>
        /// <param name="pageNumber">初期ページ番号 デフォルトは1</param>
        private async Task InitializeAsync(int pageNumber = 1)
        {
            try
            {
                // 総レコード数を取得する
                var totalCount = await _usersRepository.GetUsersAsync();

                // ページネーション用の設定を行う
                PaginationVM.TotalPages = (int)Math.Ceiling((double)totalCount.Count() / PageSize);
                PaginationVM.CurrentPage = 1;

                // 指定されたページのデータを取得する
                var enumerableData = await _usersRepository.GetPaginateAsync(pageNumber, PageSize);

                FilterMemberData = new ObservableCollection<UserEntity>(enumerableData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初期化中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// アイテムをフィルタリングする
        /// </summary>
        public void FilterItems()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) // 検索文字列が空の場合はTrueを返す
            {
                FilterMemberData = _originalMemberData;
            }
            else
            {
                // 検索結果をTextBoxの文字列でフィルタリングする
                var filterDate = _originalMemberData.Where(x => x.Name?.Contains(SearchText) == true);

                // フィルタリング結果をObservableCollectionに変換する
                FilterMemberData = filterDate == null ? new ObservableCollection<UserEntity>()
                                                      : new ObservableCollection<UserEntity>(filterDate);
            }
        }

        /// <summary>
        /// 検索を実行する
        /// </summary>
        private async void OnSearch()
        {
            try
            {
                var enumerableData = await _usersRepository.GetUsersAsync();

                FilterMemberData = new ObservableCollection<UserEntity>(enumerableData);

                // 絞り込み検索前のデータを保持する
                _originalMemberData = FilterMemberData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
