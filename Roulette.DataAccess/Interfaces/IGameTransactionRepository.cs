using Roulette.Models;

namespace Roulette.DataAccess.Interfaces
{
    public interface IGameTransactionRepository
    {
        void Update(GameTransaction gameTransaction);
    }
}