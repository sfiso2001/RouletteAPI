using Roulette.Models;

namespace Roulette.DataAccess.Interfaces
{
    public interface IGameTransactionRepository : IRepository<GameTransaction>
    {
        void Update(GameTransaction gameTransaction);
        Task<IEnumerable<GameTransaction>> GameTransactionSpinsByReferenceAsync(string betReference);
        Task<IEnumerable<GameTransaction>> GameTransactionBetsByReferenceAsync(string betReference);
        Task<double> GetGameTransactionPayoutAsync(string betReference);
    }
}