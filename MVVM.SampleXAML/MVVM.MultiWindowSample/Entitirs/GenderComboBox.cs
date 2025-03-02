namespace MVVM.MultiWindowSample.Entitirs
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GenderComboBox
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayGender => Id switch
        {
            1 => "男",
            2 => "女",
            _ => "その他・不明"
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// Id:1 => 男性<br/>
        /// Id:2 => 女性<br/>
        /// Id:3 => その他・不明
        /// </remarks>
        public GenderComboBox(int id)
        {
            Id = id;
        }
    }
}
