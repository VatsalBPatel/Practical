using Microsoft.EntityFrameworkCore;
using Practical.Context;
using System.Linq;

namespace Practical.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        private readonly ApplicationContext _context;
        private DbSet<T> _entities;

        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Delete(T entity)
        {
             _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void ExecuteSql(string query)
        {
        }

        public virtual IQueryable<T> Table => Entities;

        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }

                return _entities;
            }
        }
    }
}
