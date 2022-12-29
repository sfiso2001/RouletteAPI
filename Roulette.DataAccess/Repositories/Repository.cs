using Microsoft.EntityFrameworkCore;
using Roulette.DataAccess.Interfaces;
using System.Linq.Expressions;

namespace Roulette.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        //
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = db.Set<T>();
        }

        //Add
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        //Get
        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        //GetAll
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {

            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        //GetDefaultorFisrt
        public T GetDefaultOrFirst(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            return query.FirstOrDefault();
        }

        //Remove
        public void Remove(int id)
        {
            T entity = dbSet.Find(id);

            Remove(entity);

        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }


        //RemoveRange
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
