using Microsoft.Extensions.Logging;
using Roulette.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.BusinessLogic
{
    public class TransactionsBL
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionsBL(IUnitOfWork unitOfWork, ILogger<TransactionsBL> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        //Place Bet
        public void DebitTransaction()
        {
            //TODO: Check player balance

            //TODO: Place bet in GameTransactions

            //TODO: Debit Player

            //TODO: Return outcome
        }

        //Spin
        public void PlaySpin()
        {
            //TODO: Update GameTransaction Status

            //TODO: Return game Outcome
        }

        //Payout
        public void CreditPlayer()
        {
            //TODO: Chekc player Balance

            //TODO: Update player Balance
        }

        //ShowPreviousSpins
        public void ShowTransactions()
        {
            //TODO: Show Spin for Reference
        }

        private double PlayerBalance(long playerId)
        {
            //TODO: Validate playerId

            //TODO: Return Player balance if Found
            return 0;
        }
    }
}
