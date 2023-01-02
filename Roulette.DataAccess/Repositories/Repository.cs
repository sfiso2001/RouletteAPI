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
            return await dbSet.FindAsync(id);
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
