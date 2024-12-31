using MVVM.MultiWindowSample.Entitirs;
using MVVM.MultiWindowSample.Repositories;

namespace MVVM.MultiWindowSample.Models
{
    public sealed class Users : IUsersRepository<UserEntity>
    {
        public Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            var sql = @"SELECT * FROM Users";

            return Task.Run(() => SqlHelper.Query<UserEntity>(sql));
        }
    }
}
