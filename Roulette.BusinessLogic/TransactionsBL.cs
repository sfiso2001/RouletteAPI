using Microsoft.Extensions.Logging;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.BusinessLogic.DTO.Responses;
using Roulette.BusinessLogic.Interfaces;
using Roulette.DataAccess.Interfaces;
using Roulette.Models;
using Roulette.Models.Common;

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
        public async Task<PlaceBetResponse> DebitTransactionAsync(PlaceBetRequest placeBetRequest)
        {
            placeBetRequest.AssertIsNotNull(nameof(placeBetRequest));

            var playerDetail = await _unitOfWork.PlayerDetailRepository.GetAsync(placeBetRequest.PlayerId);

            if(playerDetail == null)
            {
                return new PlaceBetResponse()
                {
                    Success = false,
                    Message = "Player Balance not Found",
                    ErrorCode = ErrorType.None.ToString()                   
                };
            }
            else
            {
                if(playerDetail.Balance < placeBetRequest.StakeAmount)
                {
                    return new PlaceBetResponse()
                    {
                        Success = false,
                        Message = "Player Balance Low",
                        ErrorCode = ErrorType.PlayerBalanceLow.ToString()
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
                    Message = "Bet Placed Successfully",
                    ErrorCode = ErrorType.None.ToString()
                };
            }
        }

        //Spin
        public async Task<SpinResponse> PlaySpinAsync(SpinRequest spinRequest)
        {
            spinRequest.AssertIsNotNull(nameof(spinRequest));
            //TODO: Update GameTransaction Status

            //TODO: Return game Outcome
            return new SpinResponse();
        }

        //Payout
        public async Task<PayoutResponse> CreditPlayerAsync(PayoutRequest payoutRequest)
        {
            payoutRequest.AssertIsNotNull(nameof(payoutRequest));
            //TODO: Check Reference for updating request

            //TODO: Update player Balance

            return new PayoutResponse();
        }

        //ShowPreviousSpins
        public void ShowTransactions()
        {
            //TODO: Show Spin for Reference
        }

        public async Task<PlayerBalanceResponse> PlayerBalanceAsync(PlayerBalanceRequest playerBalanceRequest)
        {
            playerBalanceRequest.AssertIsNotNull(nameof(playerBalanceRequest));

            var playerDetail = await _unitOfWork.PlayerDetailRepository.GetAsync(playerBalanceRequest.PlayerId);

            if (playerDetail == null)
            {
                return new PlayerBalanceResponse()
                {
                    PlayerId = playerBalanceRequest.PlayerId,
                    Balance = 0,
                    Success = false,
                    Message = "Player Balance not Found",
                    ErrorCode = ErrorType.PlayerBalanceLow.ToString()
                };
            }
            else
            {
                return new PlayerBalanceResponse()
                {
                    PlayerId = playerDetail.Id,
                    PlayerName = playerDetail.PlayerName,
                    Balance = playerDetail.Balance,
                    Success = true,
                    ErrorCode = ErrorType.None.ToString()
                };
            }
        }
    }
}
