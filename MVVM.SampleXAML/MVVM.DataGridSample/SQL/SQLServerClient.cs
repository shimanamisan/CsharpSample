using DataGrid_Sample.Enitities;
using DataGrid_Sample.Repositories;
using System;
using System.Collections.ObjectModel;

namespace DataGrid_Sample.SQL
{
    public sealed class SQLServerClient : IDataGridRepository
    {
    
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SQLServerClient()
        {
            // サーバーに接続する処理など
        }

        public ObservableCollection<UserEnitity> Get()
        {
            // 未実装の例外はそのままにしておく
            throw new NotImplementedException();
        }
    }
}
