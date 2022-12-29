using Roulette.BusinessLogic.DTO.Requests;
using Roulette.BusinessLogic.DTO.Responses;

namespace Roulette.BusinessLogic.Interfaces
{
    public interface ITransactionsBL
    {
        PayoutResponse CreditPlayer(PayoutRequest payoutRequest);
        PlaceBetResponse DebitTransaction(PlaceBetRequest placeBetRequest);
        SpinResponse PlaySpin(SpinRequest spinRequest);
        void ShowTransactions();
        PlayerBalanceResponse PlayerBalance(PlayerBalanceRequest playerBalanceRequest);
    }
}