using Roulette.Models;

namespace Roulette.DataAccess.Interfaces
{
    public interface IGameTransactionRepository : IRepository<GameTransaction>
    {
        void Update(GameTransaction gameTransaction);
    }
}