namespace DBRepository.Interfaces
{
    public interface IRepositoryContextFactory
    {
        RepositoryContext CreateDBContext(string connectionString);
    }
}
