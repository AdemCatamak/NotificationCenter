using System.Collections.Generic;
using System.Linq;
using NotificationCenter.DataAccessLayer.DatabaseContext.Exceptions;

namespace NotificationCenter.DataAccessLayer.DatabaseContext
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IEnumerable<IRepository> _repositories;

        public RepositoryFactory(IEnumerable<IRepository> repositories)
        {
            _repositories = repositories;
        }

        public T Generate<T>() where T : IRepository
        {
            IRepository? repository = _repositories.FirstOrDefault(x => x is T);
            if (repository == null)
            {
                throw new RepositoryNotFoundException(typeof(T));
            }

            return (T) repository;
        }
    }
}