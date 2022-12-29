using Roulette.DataAccess.Interfaces;
using Roulette.Models;

namespace Roulette.DataAccess.Repositories
{
    public class PlayerDetailsRepository : Repository<PlayerDetail>, IPlayerDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public PlayerDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PlayerDetail playerDetail)
        {
            //Check gameTransation in db and update if found.
            var playerDetailsFromDb = _db.PlayerDetails.FirstOrDefault(x => x.Id == playerDetail.Id);

            if (playerDetailsFromDb != null)
            {
                //TODO:- Update all properties, most likely player balance
            }
        }
    }
}
