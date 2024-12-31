namespace MVVM.MultiWindowSample.Repositories
{
    public interface IUsersRepository<T>
    {
        Task<IEnumerable<T>> GetUsersAsync();
    }
}
