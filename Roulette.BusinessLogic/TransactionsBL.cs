using Microsoft.Extensions.Logging;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.BusinessLogic.DTO.Responses;
using Roulette.BusinessLogic.Interfaces;
using Roulette.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //TODO: Check player balance

            //TODO: Place bet in GameTransactions

            //TODO: Debit Player

            //TODO: Return outcome

            return new PlaceBetResponse();
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
            //TODO: Chekc player Balance

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
