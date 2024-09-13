using MVVM.AsyncConstructorInitialization.Entities;
using MVVM.AsyncConstructorInitialization.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM.AsyncConstructorInitialization.Models
{
    public sealed class Users : IUsersRepository<UserEntity>
    {
        private const string EXECUTE_SQL = "SELECT * FROM Users";

        public Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            return Task.Run(() => SqlHelper.Query<UserEntity>(EXECUTE_SQL));
        }
    }
}
