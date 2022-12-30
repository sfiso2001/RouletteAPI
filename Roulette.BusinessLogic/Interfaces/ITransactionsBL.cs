using Roulette.BusinessLogic.DTO.Requests;
using Roulette.BusinessLogic.DTO.Responses;

namespace Roulette.BusinessLogic.Interfaces
{
    public interface ITransactionsBL
    {
        Task<PayoutResponse> CreditPlayerAsync(PayoutRequest payoutRequest);
        Task<PlaceBetResponse> DebitTransactionAsync(PlaceBetRequest placeBetRequest);
        Task<SpinResponse> PlaySpinAsync(SpinRequest spinRequest);
        Task<List<BetTransactionsResponse>> ShowPreviousSpins(string betReference);
        Task<PlayerBalanceResponse> PlayerBalanceAsync(PlayerBalanceRequest playerBalanceRequest);
    }
}