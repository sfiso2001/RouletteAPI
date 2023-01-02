using Microsoft.Extensions.Logging;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.BusinessLogic.DTO.Responses;
using Roulette.Common.Enums;
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

        //Place Bet
        public async Task<PlaceBetResponse> DebitTransactionAsync(PlaceBetRequest placeBetRequest)
        {
            placeBetRequest.AssertIsNotNull(nameof(placeBetRequest));

            var playerDetail = await _unitOfWork.PlayerDetailRepository.GetAsync(placeBetRequest.PlayerId);

            if(playerDetail == null)
            {
                return new PlaceBetResponse()
                {
                    GameId = placeBetRequest.GameId,
                    PlayerId = placeBetRequest.PlayerId,
                    StakeAmount = placeBetRequest.StakeAmount,
                    Success = false,
                    Message = "Player not Found",
                    ErrorCode = ErrorType.PlayerIdNotFound.ToString()                   
                };
            }
            else
            {
                if(playerDetail.Balance < placeBetRequest.StakeAmount)
                {
                    return new PlaceBetResponse()
                    {
                        GameId = placeBetRequest.GameId,
                        PlayerId = placeBetRequest.PlayerId,
                        StakeAmount = placeBetRequest.StakeAmount,
                        Success = false,
                        Message = "Player Balance Low",
                        ErrorCode = ErrorType.PlayerBalanceLow.ToString()
                    };
                }

                var newTransaction = new GameTransaction(
                    transactionType: TransactionType.Bet.ToString(),
                    gameId: placeBetRequest.GameId,
                    reference: placeBetRequest.Reference,
                    spinReference: placeBetRequest.Reference,
                    playerId: placeBetRequest.PlayerId,
                    stakeAmount: placeBetRequest.StakeAmount,
                    outcomeAmount: 0,
                    createdDate: DateTime.Now
                    );

                var existingBets = await _unitOfWork.GameTransactionRepository.GetAllAsync(
                    filter: x => (x.Reference == placeBetRequest.Reference)
                && (x.TransactionType == TransactionType.Bet.ToString()));

                if(existingBets.Any())
                {
                    return new PlaceBetResponse()
                    {
                        GameId = placeBetRequest.GameId,
                        PlayerId = placeBetRequest.PlayerId,
                        StakeAmount = placeBetRequest.StakeAmount,
                        Success = false,
                        Message = "Bet Reference Already Exists",
                        ErrorCode = ErrorType.GameReferenceDuplicated.ToString()
                    };
                }

                _unitOfWork.GameTransactionRepository.Add(newTransaction);
                playerDetail.Balance = playerDetail.Balance - placeBetRequest.StakeAmount;
                _unitOfWork.PlayerDetailRepository.Update(playerDetail);
                _unitOfWork.Save();

                return new PlaceBetResponse()
                {
                    GameId = placeBetRequest.GameId,
                    PlayerId = placeBetRequest.PlayerId,
                    StakeAmount = placeBetRequest.StakeAmount,
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

            var existingBet = await _unitOfWork.GameTransactionRepository.GameTransactionBetsByReference(spinRequest.Reference);

            if (!existingBet.Any())
            {
                return new SpinResponse()
                {
                    GameId = spinRequest.GameId,
                    PlayerId = spinRequest.PlayerId,
                    WinAmount = spinRequest.WinAmount,
                    Success = false,
                    ErrorCode = ErrorType.GameReferenceNotFound.ToString(),
                    Message = "Initial Bet Not Found"
                };
            }

            var gameTransactionFromDb = await _unitOfWork.GameTransactionRepository.GetAsync(existingBet.FirstOrDefault().Id);

            var spinGameTransaction = new GameTransaction(
                transactionType: TransactionType.Spin.ToString(),
                gameId: gameTransactionFromDb.GameId,
                reference: gameTransactionFromDb.Reference,
                spinReference: await NextSpinReferenceAsync(gameTransactionFromDb.Reference),
                playerId: spinRequest.PlayerId,
                stakeAmount: gameTransactionFromDb.StakeAmount,
                outcomeAmount: spinRequest.WinAmount,
                createdDate: gameTransactionFromDb.CreatedDate
                );

            _unitOfWork.GameTransactionRepository.Add(spinGameTransaction);
            _unitOfWork.Save();

            return new SpinResponse()
            {
                GameId = spinRequest.GameId,
                PlayerId = spinRequest.PlayerId,
                WinAmount = spinRequest.WinAmount,
                Success = true,
                ErrorCode = ErrorType.None.ToString(),
                Message = "Spin updated successfully"
            };
        }

        //Payout
        public async Task<PayoutResponse> CreditPlayerAsync(PayoutRequest payoutRequest)
        {
            payoutRequest.AssertIsNotNull(nameof(payoutRequest));

            var playerDetail = await _unitOfWork.PlayerDetailRepository.GetAsync(payoutRequest.PlayerId);

            if (playerDetail == null)
            {
                return new PayoutResponse()
                {
                    GameId = payoutRequest.GameId,
                    PlayerId = payoutRequest.PlayerId,
                    Reference = payoutRequest.Reference,
                    Success = false,
                    Message = "Player Detail not Found",
                    ErrorCode = ErrorType.PlayerIdNotFound.ToString()
                };
            }

            var existingBet = await _unitOfWork.GameTransactionRepository.GameTransactionBetsByReference(payoutRequest.Reference);

            if (!existingBet.Any())
            {
                return new PayoutResponse()
                {
                    GameId = payoutRequest.GameId,
                    PlayerId = payoutRequest.PlayerId,
                    Reference = payoutRequest.Reference,
                    Success = false,
                    ErrorCode = ErrorType.GameReferenceNotFound.ToString(),
                    Message = "Initial Bet Not Found"
                };
            }

            var gamePayout = await _unitOfWork.GameTransactionRepository.GetGameTransactionPayoutAsync(payoutRequest.Reference);

            var gameTransactionFromDb = await _unitOfWork.GameTransactionRepository.GetAsync(existingBet.FirstOrDefault().Id);

            gameTransactionFromDb.OutcomeAmount = gamePayout;
            gameTransactionFromDb.OutcomeDate = DateTime.Now;

            _unitOfWork.GameTransactionRepository.Update(gameTransactionFromDb);

            playerDetail.Balance = playerDetail.Balance + gamePayout;
            _unitOfWork.PlayerDetailRepository.Update(playerDetail);
            _unitOfWork.Save();

            return new PayoutResponse()
            {
                GameId = payoutRequest.GameId,
                Reference = payoutRequest.Reference,
                PlayerId = payoutRequest.PlayerId,
                Success = true,
                ErrorCode = ErrorType.None.ToString()
            };
        }

        //ShowPreviousSpins
        public async Task<List<BetTransactionsResponse>> ShowPreviousSpinsAsync(string betReference)
        {
            var betTransactions = await _unitOfWork.GameTransactionRepository.GameTransactionSpinsByReference(betReference);
    
            return MapBetTransactions(betTransactions);
        }

        private async Task<string> NextSpinReferenceAsync(string betReference)
        {
            var currentSpinGameTransactions = await _unitOfWork.GameTransactionRepository.GameTransactionSpinsByReference(betReference); ;

            int nextReferentNumber = 1;
            
            if(currentSpinGameTransactions != null)
            {
                nextReferentNumber = currentSpinGameTransactions.Count() + 1;
            }

            return betReference + "-" +nextReferentNumber.ToString();
        }
      
        private List<BetTransactionsResponse> MapBetTransactions(IEnumerable<GameTransaction> gameList)
        {
            return new List<BetTransactionsResponse>(gameList
                .Select(x => new BetTransactionsResponse()
                {
                    Id = x.Id,
                    GameId = x.GameId,
                    PlayerId = x.PlayerId,
                    PlayerName = "",
                    TransactionType = x.TransactionType,
                    Reference = x.Reference,
                    SpinReference = x.SpinReference,
                    StakeAmount = x.StakeAmount,
                    OutcomeAmount = x.OutcomeAmount,
                    OutcomeDate = x.OutcomeDate,
                    CreatedDate = x.CreatedDate,
                    ErrorCode = ErrorType.None.ToString(),
                    Success = true
                }                 
                ).ToList().AsReadOnly());
        }        
    }
}
