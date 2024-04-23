using MVVM.DataGridSearch.Entities;
using MVVM.DataGridSearch.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM.DataGridSearch.Models
{
    public sealed class Users : IUsersRepository<UserEntity>
    {
        private const string EXECUTE_SQL = "SELECT * FROM Users";

        public Task<IEnumerable<UserEntity>> GetUsers()
        {
            return Task.Run(() => SqlHelper.Query<UserEntity>(EXECUTE_SQL));
        }
    }
}
