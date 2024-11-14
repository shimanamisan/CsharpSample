namespace MVVM.TextBoxValidation.ViewModels
{
    public sealed class MainWindowViewModel : BindableBase
    {
        public string ValidationRuleTextBox
        {
            get => _validationRuleTextBox;
            set => SetProperty(ref _validationRuleTextBox, value);
        }
        private string _validationRuleTextBox;

        public string ValidatesOnExceptionsTextBox
        {
            get => _validatesOnExceptionsTextBox;
            set
            {
                if(SetProperty(ref _validatesOnExceptionsTextBox, value))
                {

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("入力必須です。");
                    }

                    if (int.TryParse(value, out int number))
                    {
                        // int型の値に対するバリデーションを行う
                        if (number < 0)
                            throw new ArgumentException("負の値は入力できません。");

                        if (number > 10)
                            throw new ArgumentException("最大値を超えています。");
                    }
                    else
                    {
                        throw new ArgumentException("数値を入力してください。");
                    }
                }
            }
        }
        private string _validatesOnExceptionsTextBox;

        public string INotifyDataErrorInfoTextBox
        {
            get => _iNotifyDataErrorInfoTextBox;
            set => SetProperty(ref _iNotifyDataErrorInfoTextBox, value, true);
        }
        private string _iNotifyDataErrorInfoTextBox;

        public MainWindowViewModel()
        { }

        protected override void ValidateProperty(string propertyName)
        {
            // 指定されたプロパティの既存のエラーを削除
            ClearErrors(propertyName);

            switch (propertyName)
            {
                case nameof(INotifyDataErrorInfoTextBox):
                    INotifyDataErrorInfoTextBoxValidate(INotifyDataErrorInfoTextBox, AddError);
                    break;
                default:
                    throw new ArgumentException("不正なプロパティ名です。");
            }
        }

        private void INotifyDataErrorInfoTextBoxValidate(string value, Action<string, string> addError)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                addError(nameof(INotifyDataErrorInfoTextBox), "入力必須です。");
                return;
            }

            if (int.TryParse(value, out int number))
            {
                if (number < 0)
                    addError(nameof(INotifyDataErrorInfoTextBox), "負の値は入力できません。");

                if (number > 10)
                    addError(nameof(INotifyDataErrorInfoTextBox), "最大値を超えています。");
            }
            else
            {
                addError(nameof(INotifyDataErrorInfoTextBox), "数値を入力してください。");
            }
        }
    }
}