using Roulette.DataAccess.Interfaces;
using Roulette.Models;

namespace Roulette.DataAccess.Repositories
{
    public class PlayerDetailRepository : Repository<PlayerDetail>, IPlayerDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public PlayerDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PlayerDetail playerDetail)
        {
            //Check gameTransation in db and update if found.
            var playerDetailsFromDb = _db.PlayerDetails.FirstOrDefault(x => x.Id == playerDetail.Id);

            if (playerDetailsFromDb != null)
            {
                playerDetailsFromDb.PlayerName = playerDetail.PlayerName;
                playerDetailsFromDb.Balance = playerDetail.Balance;                
            }
        }
    }
}
