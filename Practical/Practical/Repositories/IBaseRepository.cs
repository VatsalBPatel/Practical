using System.Linq;

namespace Practical.Repositories
{
    public interface IBaseRepository<T> where T : class
    {

        IQueryable<T> GetAll();

        T GetById(object id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Save();

        void ExecuteSql(string query);

        IQueryable<T> Table { get; }

        IQueryable<T> TableNoTracking { get; }
    }
}
