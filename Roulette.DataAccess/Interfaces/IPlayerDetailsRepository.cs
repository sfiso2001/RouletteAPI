using Roulette.Models;

namespace Roulette.DataAccess.Interfaces
{
    public interface IPlayerDetailsRepository
    {
        void Update(PlayerDetail playerDetail);
    }
}