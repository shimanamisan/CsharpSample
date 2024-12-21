namespace CommunityToolkitMvvmTutorial.Repositories
{
    public interface IUsersRepository<T>
    {
        Task<IEnumerable<T>> GetUsersAsync();
    }
}
