using MVVM.AsyncConstructorInitialization.Entities;
using MVVM.AsyncConstructorInitialization.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.AsyncConstructorInitialization.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IUsersRepository<UserEntity> _usersRepository;

        #region プロパティ

        public bool IsTextEmpty => string.IsNullOrWhiteSpace(SearchText);

        public string SearchText
        {
            get => _searchText;
            set
            {
                if(SetProperty(ref _searchText, value))
                {
                    RaisePropertyChanged(nameof(IsTextEmpty));
                    FilterItems();
                }
            }
        }   
        private string _searchText;

        private ObservableCollection<UserEntity> _originalMemberData;

        public ObservableCollection<UserEntity> FilterMemberData
        {
            get => _filterMemberData;
            set => SetProperty(ref _filterMemberData, value);
        }
        private ObservableCollection<UserEntity> _filterMemberData;

        public string Title
        {
            get => _title;
            set =>  SetProperty(ref _title, value);
        }
        private string _title = "Prism Application";

        #endregion

        #region コマンド

        public DelegateCommand InitializeAsyncCommand =>
           _initializeAsyncCommand ?? (new DelegateCommand(async() => await InitializeAsync()));
        private DelegateCommand _initializeAsyncCommand;

        public DelegateCommand AppCloseCommand =>
            _appCloseCommand ?? (new DelegateCommand(() => Application.Current.Shutdown()));
        private DelegateCommand _appCloseCommand;

        public DelegateCommand SearchCommand =>
           _searchCommand ?? (new DelegateCommand(OnSearch));
        private DelegateCommand _searchCommand;

#endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="usersRepository">Userリポジトリインターフェース</param>
        public MainWindowViewModel(IUsersRepository<UserEntity> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        private async Task InitializeAsync()
        {
            try
            {
                //throw new Exception("InitializeAsync メソッド内でエラー");

                var enumerableData = await _usersRepository.GetUsersAsync();

                FilterMemberData = new ObservableCollection<UserEntity>(enumerableData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初期化中にエラーが発生しました。: {ex.Message}");
            }
        }

        public void FilterItems()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) // 検索文字列が空の場合はTrueを返す
            {
                FilterMemberData = _originalMemberData;
            }
            else
            {
                // 検索結果をTextBoxの文字列でフィルタリング
                var filterDate = _originalMemberData.Where(x => x.Name?.Contains(SearchText) == true);

                // フィルタリング結果をObservableCollectionに変換
                FilterMemberData = filterDate == null ? new ObservableCollection<UserEntity>()
                                                      : new ObservableCollection<UserEntity>(filterDate);
            }
        }

        private async void OnSearch()
        {
            try
            {
                var enumerableData = await _usersRepository.GetUsersAsync();

                FilterMemberData = new ObservableCollection<UserEntity>(enumerableData);

                // 絞り込み検索前のデータを保持
                _originalMemberData = FilterMemberData;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
