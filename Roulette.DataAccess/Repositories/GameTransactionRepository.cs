using Microsoft.EntityFrameworkCore;
using Roulette.Common.Enums;
using Roulette.DataAccess.Interfaces;
using Roulette.Models;
using System.Net;

namespace Roulette.DataAccess.Repositories
{
    public class GameTransactionRepository : Repository<GameTransaction>, IGameTransactionRepository
    {
        private readonly ApplicationDbContext _db;

        public GameTransactionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(GameTransaction gameTransaction)
        {
            //Check gameTransation in db and update if found.
            var gameTransactionFromDb = _db.GameTransactions.FirstOrDefault(x => x.Id == gameTransaction.Id);

            if (gameTransactionFromDb != null)
            {
                gameTransactionFromDb.OutcomeAmount = gameTransaction.OutcomeAmount;
                gameTransactionFromDb.OutcomeDate = gameTransaction.OutcomeDate;
            }
        }

        public async Task<IEnumerable<GameTransaction>> GameTransactionBetsByReference(string betReference)
        {
            var gameTransactionsByReference = await _db.GameTransactions
                .Where(x => x.Reference == betReference
                        && x.TransactionType == TransactionType.Bet.ToString())
                .ToListAsync();

            return gameTransactionsByReference;
        }

        public async Task<IEnumerable<GameTransaction>> GameTransactionSpinsByReference(string betReference)
        {
            var gameTransactionsByReference = await _db.GameTransactions
                .Where(x => x.Reference == betReference 
                            && x.TransactionType == TransactionType.Spin.ToString())
                .ToListAsync();

            return gameTransactionsByReference;
        }


    }
}
