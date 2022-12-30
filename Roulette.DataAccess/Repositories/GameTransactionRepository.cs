using Roulette.DataAccess.Interfaces;
using Roulette.Models;

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
    }
}
