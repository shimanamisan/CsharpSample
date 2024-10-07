using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM.DataGridPagination.Repositories
{
    public interface IUsersRepository<T>
    {
        Task<IEnumerable<T>> GetUsersAsync();

        Task<IEnumerable<T>> GetPaginateAsync(int pageNumber, int pageSize); // 追加
    }
}
