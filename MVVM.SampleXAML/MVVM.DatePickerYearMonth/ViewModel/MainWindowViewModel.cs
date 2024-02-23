using MVVM.DatePickerYearMonth.Command;
using MVVM.DatePickerYearMonth.Helpers;
using System.Windows;

namespace MVVM.DatePickerYearMonth.ViewModel
{
    public class MainWIndowViewModel : ViewModelBase
    {
        #region コマンド

        public DelegateCommand<object> SelectedDateCommand
            => _selectedDateCommand ?? (_selectedDateCommand = new DelegateCommand<object>(OnSelectedDate));
        private DelegateCommand<object> _selectedDateCommand;

        #endregion

        #region データバインディングさせているプロパティ

        public DateTime? MonthYear
        {
            get => _monthYear;
            set
            {
                if(SetProperty(ref _monthYear, value))
                {
                    JapaneseEra = _monthYear.ConvertToJapaneseEraMidle();
                }
            }
        }
        private DateTime? _monthYear;

        public string? JapaneseEra
        {
            get => _japaneseEra;
            set => SetProperty(ref _japaneseEra, value);
        }
        private string? _japaneseEra = "和暦を表示する";

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWIndowViewModel()
        { }

        private void OnSelectedDate(object o)
        {
            MessageBox.Show($"{o}");
        }
    }
}
