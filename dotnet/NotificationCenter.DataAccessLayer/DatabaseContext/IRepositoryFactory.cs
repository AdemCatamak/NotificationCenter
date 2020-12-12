namespace NotificationCenter.DataAccessLayer.DatabaseContext
{
    public interface IRepositoryFactory
    {
        T Generate<T>() where T : IRepository;
    }
}