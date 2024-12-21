using CommunityToolkitMvvmTutorial.Entitirs;
using CommunityToolkitMvvmTutorial.Repositories;

namespace CommunityToolkitMvvmTutorial.Models
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
