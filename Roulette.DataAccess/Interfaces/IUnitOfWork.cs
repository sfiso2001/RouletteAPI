namespace Roulette.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IPlayerDetailRepository PlayerDetailRepository { get; set; }
        IGameTransactionRepository GameTransactionRepository { get; set; }

        void Save();
    }
}