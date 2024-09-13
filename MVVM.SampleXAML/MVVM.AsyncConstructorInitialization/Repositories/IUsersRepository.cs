using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM.AsyncConstructorInitialization.Repositories
{
    public interface IUsersRepository<T>
    {
        Task<IEnumerable<T>> GetUsersAsync();
    }
}
