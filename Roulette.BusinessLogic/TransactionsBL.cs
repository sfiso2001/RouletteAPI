using Microsoft.Extensions.Logging;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.BusinessLogic.DTO.Responses;
using Roulette.BusinessLogic.Interfaces;
using Roulette.DataAccess.Interfaces;
using Roulette.Models;

namespace Roulette.BusinessLogic
{
    public class TransactionsBL : ITransactionsBL
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionsBL(IUnitOfWork unitOfWork, ILogger<TransactionsBL> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        //Place Bet
        public PlaceBetResponse DebitTransaction(PlaceBetRequest placeBetRequest)
        {
            var playerDetail = _unitOfWork.PlayerDetailRepository.Get(placeBetRequest.PlayerId);

            if(playerDetail == null)
            {
                return new PlaceBetResponse()
                {
                    Success = false,
                    Message = "Player Balance not Found"
                };
            }
            else
            {
                if(playerDetail.Balance < placeBetRequest.StakeAmount)
                {
                    return new PlaceBetResponse()
                    {
                        Success = false,
                        Message = "Player Balance Low"
                    };
                }

                var newTransaction = new GameTransaction(
                    transactionType: "Bet",
                    gameId: placeBetRequest.GameId,
                    reference: placeBetRequest.Reference,
                    playerId: placeBetRequest.PlayerId,
                    stakeAmount: placeBetRequest.StakeAmount,
                    outcomeAmount: 0,
                    createdDate: DateTime.Now
                    );

                //DOTO: Check if reference not duplicated

                _unitOfWork.GameTransactionRepository.Add(newTransaction);
                _unitOfWork.Save();

                return new PlaceBetResponse()
                {
                    Success = true,
                    Message = "Bet Placed Successfully"
                };
            }
        }

        //Spin
        public SpinResponse PlaySpin(SpinRequest spinRequest)
        {
            //TODO: Update GameTransaction Status

            //TODO: Return game Outcome
            return new SpinResponse();
        }

        //Payout
        public PayoutResponse CreditPlayer(PayoutRequest payoutRequest)
        {
            //TODO: Check Reference for updating request

            //TODO: Update player Balance

            return new PayoutResponse();
        }

        //ShowPreviousSpins
        public void ShowTransactions()
        {
            //TODO: Show Spin for Reference
        }

        public PlayerBalanceResponse PlayerBalance(PlayerBalanceRequest playerBalanceRequest)
        {
            //TODO: Validate playerId


            //TODO: Return Player balance if Found
            var playerDetail = _unitOfWork.PlayerDetailRepository.Get(playerBalanceRequest.PlayerId);

            if (playerDetail == null)
            {
                return new PlayerBalanceResponse()
                {
                    Success = false,
                    Message = "Player Balance not Found"
                };
            }
            else
            {
                return new PlayerBalanceResponse()
                {
                    Balance = playerDetail.Balance,
                    Success = true
                };
            }
        }
    }
}
