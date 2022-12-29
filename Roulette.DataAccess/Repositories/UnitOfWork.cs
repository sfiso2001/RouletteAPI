using Roulette.DataAccess.Interfaces;

namespace Roulette.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IPlayerDetailRepository PlayerDetailRepository { get; set; }
        public IGameTransactionRepository GameTransactionRepository { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            PlayerDetailRepository = new PlayerDetailRepository(_db);
            GameTransactionRepository = new GameTransactionRepository(_db);
        }

        //Data update must be done by UnitOfWork
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
