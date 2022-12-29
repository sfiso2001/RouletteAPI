using Microsoft.EntityFrameworkCore;
using Roulette.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Roulette.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        
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
        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id) 
                ?? throw new Exception("Not record found");
        }

        //GetAll
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
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
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        //GetDefaultorFisrt
        public async Task<T> GetDefaultOrFirstAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
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


            return await query.FirstOrDefaultAsync() 
                ?? throw new Exception("No record found");
        }

        //Remove
        public void Remove(int id)
        {
            T entity = dbSet.Find(id)
                ?? throw new Exception("Cannot remove that does not exist.");

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
}
