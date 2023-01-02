using Roulette.Models;

namespace Roulette.DataAccess.Interfaces
{
    public interface IGameTransactionRepository : IRepository<GameTransaction>
    {
        void Update(GameTransaction gameTransaction);
        Task<IEnumerable<GameTransaction>> GameTransactionSpinsByReference(string betReference);
        Task<IEnumerable<GameTransaction>> GameTransactionBetsByReference(string betReference);
    }
}