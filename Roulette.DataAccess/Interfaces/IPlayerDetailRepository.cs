using Roulette.Models;

namespace Roulette.DataAccess.Interfaces
{
    public interface IPlayerDetailRepository : IRepository<PlayerDetail>
    {
        void Update(PlayerDetail playerDetail);
    }
}