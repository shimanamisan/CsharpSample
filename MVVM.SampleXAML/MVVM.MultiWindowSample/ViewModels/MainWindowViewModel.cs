using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.MultiWindowSample.Entitirs;
using MVVM.MultiWindowSample.Repositories;
using MVVM.MultiWindowSample.Servicies;
using MVVM.MultiWindowSample.ViewModels;
using MVVM.MultiWindowSample.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace MVVM.MultiWindowSample
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IUsersRepository<UserEntity> _usersRepository;

        private readonly IWindowService _windowService;

        private ObservableCollection<UserEntity> _allCustomerList;

        #region プロパティ

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<UserEntity> _filteredCustomerList;

        [ObservableProperty]
        private bool _hasUserInput = true;

        partial void OnSearchTextChanging(string value)
        {
            HasUserInput = string.IsNullOrWhiteSpace(value) ? true : false;

            if (string.IsNullOrWhiteSpace(value))
            {
                FilteredCustomerList = new ObservableCollection<UserEntity>(_allCustomerList);
            }
            else
            {
                var filteredData = _allCustomerList.Where(x => x.Name?.Contains(value,
                                                                                StringComparison.OrdinalIgnoreCase) // 大文字小文字を区別しないオプション
                                                                                == true);
                FilteredCustomerList = new ObservableCollection<UserEntity>(filteredData);
            }
        }

        #endregion

        public MainWindowViewModel(IUsersRepository<UserEntity> usersRepository,
                                   IWindowService windowService)
        {
            _usersRepository = usersRepository;
            _windowService = windowService;
        }

        #region コマンド

        [RelayCommand]
        private void RowSelected(object? sender)
        {
            if (sender is UserEntity entity)
            {
                var owner = Application.Current.MainWindow;
                _windowService.ShowWindow<SubWindow, SubWindowViewModel>(entity, owner);
            }
        }

        /// <summary>
        /// アプリケーションを終了する
        /// </summary>
        [RelayCommand]
        private void Shutdown()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task InitializeAsync()
        {
            try
            {
                var enumerableData = await _usersRepository.GetUsersAsync();

                // 全ユーザーデータを保持する
                _allCustomerList = new ObservableCollection<UserEntity>(enumerableData);
                // DataGridにバインディングするフィルター済みのデータ
                FilteredCustomerList = new ObservableCollection<UserEntity>(_allCustomerList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初期化中にエラーが発生しました。\n[{ex.Message}]");
            }
        }

        #endregion

    }
}
