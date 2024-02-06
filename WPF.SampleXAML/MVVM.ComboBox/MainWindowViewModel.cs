using System.Collections.Generic;

namespace MVVM.ComboBox
{
    /// <summary>
    /// MainWindowのViewModel。ウィンドウのコンポーネントのコマンドとインタラクションを処理します。
    /// </summary>
    public sealed class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// ComboBoxに表示させるリスト
        /// </summary>
        public List<ConboBoxEntity> ComboBoxSource
        {
            get => _comboBoxSource;
            set => SetProperty(ref _comboBoxSource, value);
        }
        private List<ConboBoxEntity> _comboBoxSource = new List<ConboBoxEntity>();

        /// <summary>
        /// コンボボックスで選択された要素
        /// </summary>
        public ConboBoxEntity SelectedComboBoxItem
        {
            get => _selectedComboBoxItem;
            set => SetProperty(ref _selectedComboBoxItem, value);
        }
        private ConboBoxEntity _selectedComboBoxItem;

        /// <summary>
        /// コンボボックスに選択されたデータを表示する
        /// </summary>
        public string SelectedComboBoxItemText
        {
            get => _selectedComboBoxItemText;
            set => SetProperty(ref _selectedComboBoxItemText, value);
        }
        private string _selectedComboBoxItemText = "テスト";

        /// <summary>
        /// SelectedChangedイベントが実行されたときのコマンド
        /// </summary>
        public DelegateCommand ExecuteSelectedItem { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {

            ComboBoxSource = new List<ConboBoxEntity>
                                {
                                    new ConboBoxEntity(1, "山根 京子"),
                                    new ConboBoxEntity(2, "鵜飼 亮介"),
                                    new ConboBoxEntity(3, "住吉 知香"),
                                    new ConboBoxEntity(4, "岡本 徹"),
                                    new ConboBoxEntity(5, "岩澤 博志"),
                                    new ConboBoxEntity(6, "長谷部 美里"),
                                };

            SelectedComboBoxItem = ComboBoxSource[0];

            ExecuteSelectedItem = new DelegateCommand(SelectedItem);

        }

        /// <summary>
        /// 選択された要素をテキストボックスに格納する
        /// </summary>
        private void SelectedItem()
        {
            SelectedComboBoxItemText = SelectedComboBoxItem.Name;
        }
    }

}