using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM.DataGridSearch.Repositories
{
    public interface IUsersRepository<T>
    {
        Task<IEnumerable<T>> GetUsers();
    }
}
