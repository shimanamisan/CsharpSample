using DataGrid_Sample.Enitities;
using System.Collections.ObjectModel;

namespace DataGrid_Sample.Repositories
{
    public interface IDataGridRepository
    {
        ObservableCollection<UserEnitity> Get();
    }
}
