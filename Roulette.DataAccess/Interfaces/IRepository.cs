using System.Linq.Expressions;

namespace Roulette.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        Task<T> GetAsync(int id);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
