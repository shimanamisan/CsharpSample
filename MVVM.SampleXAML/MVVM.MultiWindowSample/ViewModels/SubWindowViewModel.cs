using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.MultiWindowSample.Entitirs;
using MVVM.MultiWindowSample.Services;
using System.Windows;

namespace MVVM.MultiWindowSample.ViewModels
{
    public partial class SubWindowViewModel : ObservableObject, IParameterReceiver, IResultProvider<Dictionary<int, UserEntity>>
    {
        // 保存ボタンが押されたかどうかを示すフラグ
        private bool _saveButtonPressed = false;

        private readonly IWindowService _windowService;

        private UserEntity _userInfo;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _age;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _phoneNumber;

        [ObservableProperty]
        private string _zipCode;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private string _companyName;

        [ObservableProperty]
        private List<GenderComboBox> _genderList;

        [ObservableProperty]
        private GenderComboBox _selectedGender;
        private int _key;

        partial void OnSelectedGenderChanged(GenderComboBox value)
        {
            if (value == null) return;

            if (value is GenderComboBox genderComboBox)
            {
                SelectedGender = genderComboBox;
            }
        }

        [RelayCommand]
        private void Save(object? sender)
        {
            if (sender is Window window)
            {
                _saveButtonPressed = true; // フラグを設定
                _windowService.CloseWindow(window);
            }
        }

        public SubWindowViewModel(IWindowService windowService)
        {
            GenderList = new List<GenderComboBox>
            {
                new GenderComboBox(1),
                new GenderComboBox(2),
                new GenderComboBox(3),
            };

            // 先頭を初期値として表示させる
            SelectedGender = GenderList[0];

            _windowService = windowService;
        }

        public Dictionary<int, UserEntity>? GetResult()
        {
            // 保存ボタンが押された場合のみデータを返す
            if (!_saveButtonPressed) return null;

            var newEntity = _userInfo.GenNewEntity(
                UserName,
                Age,
                SelectedGender,
                Email,
                PhoneNumber,
                ZipCode,
                Address,
                CompanyName);

            return new Dictionary<int, UserEntity>
            {
                {_key, newEntity }
            };
        }


        public void ReceiveParameter(object parameter)
        {
            if (parameter is Dictionary<int, UserEntity> dic)
            {
                var keys = dic.Keys;

                _key = keys.FirstOrDefault();

                var entity = dic[_key];

                if (entity == null) throw new ArgumentNullException("不正な値です");

                InitializedPropertySet(entity);
            }
        }

        private void InitializedPropertySet(UserEntity entity)
        {
            // キャッシュ用
            _userInfo = entity;

            UserName = entity.Name;
            Age = entity.Age.ToString();
            Email = entity.Email;
            PhoneNumber = entity.PhoneNumber;
            ZipCode = entity.PostalCode.Value;
            Address = entity.Address;
            CompanyName = entity.CompanyName;

            var genderId = entity.Gender switch
            {
                "男" => 1,
                "女" => 2,
                _ => 3
            };

            SelectedGender = GenderList.Where(e => e.Id == genderId).FirstOrDefault()
                ?? throw new NullReferenceException("nullの値を参照しています");
        }
    }
}
