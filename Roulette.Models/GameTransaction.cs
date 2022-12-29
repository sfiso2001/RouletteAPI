using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Models
{
    public class GameTransaction
    {
        public long Id { get; set; }
        public string TransactionType { get; set; }
        public string Reference { get; set; }
        public double StakeAmount { get; set; }
        public double OutcomeAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? OutcomeDate { get; set; }

        public GameTransaction(
            string transactionType, 
            string reference, 
            double stakeAmount,
            double outcomeAmount,
            DateTime createdDate
            )
        {
            TransactionType = transactionType;
            Reference = reference;
            StakeAmount = stakeAmount;
            OutcomeAmount = outcomeAmount;
            CreatedDate = createdDate;
        }

        public void SetGameTransactions(
            string transactionType,
            string reference,
            double stakeAmount,
            double outcomeAmount,
            DateTime createdDate,
            DateTime? outcomeDate
            )
        {
            TransactionType = transactionType;
            Reference = reference;
            StakeAmount = stakeAmount;
            OutcomeAmount = outcomeAmount;
            CreatedDate = createdDate;
            OutcomeDate = outcomeDate;
        }
    }
}
