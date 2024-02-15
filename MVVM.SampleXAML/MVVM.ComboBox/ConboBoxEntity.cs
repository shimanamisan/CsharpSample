namespace MVVM.ComboBox
{
    public sealed class ConboBoxEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">名前</param>
        public ConboBoxEntity(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}