using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.MultiWindowSample.Entitirs;
using MVVM.MultiWindowSample.Repositories;
using MVVM.MultiWindowSample.Services;
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
        private bool _isAllPrintChecked = true;

        partial void OnIsAllPrintCheckedChanged(bool value)
        {
            FilteredCustomerList.ToList().ForEach(user => user.IsPrint = value);
        }

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
        private void RowEdit(object? sender)
        {
            if (sender is object[] values && values.Length == 2)
            {
                var selectedIndex = (int)values[1];
                if (values[0] is UserEntity entity)
                {
                    var paramDic = new Dictionary<int, UserEntity>
                    {
                        { selectedIndex, entity }
                    };

                    var owner = Application.Current.MainWindow;

                    _windowService.ShowWindowWithCallback<SubWindow, SubWindowViewModel, Dictionary<int, UserEntity>?>(
                        parameter: paramDic,
                        owner: owner,
                        resultCallback: CallBackResult);

                    return;
                }

                //// 型変換に失敗した場合のカスタム例外（Warning）
                //throw new TypeConversionException(typeof(BackUpInfoItem), values[0]?.GetType() ?? typeof(object));
            }

        }

        [RelayCommand]
        private void RowDelete(object? sender)
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

        private void CallBackResult(Dictionary<int, UserEntity>? dic)
        {
            if (dic != null)
            {
                var keys = dic.Keys;

                var index = keys.FirstOrDefault();

                var entity = dic[index];

                if (entity == null) throw new ArgumentNullException("不正な値です");

                if (index >= 0 && index < _allCustomerList.Count && index < FilteredCustomerList.Count)
                {

                    UpdateCollectionItem(FilteredCustomerList, index, entity);
                    UpdateCollectionItem(_allCustomerList, index, entity);
                }
            }
        }

        private void UpdateCollectionItem<T>(ObservableCollection<T> collection, int index, T newEntity)
        {
            collection[index] = newEntity;
        }
    }
}
