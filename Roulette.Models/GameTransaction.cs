using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Models
{
    public class GameTransaction
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string GameId { get; set; }
        public string Reference { get; set; }
        [Required]
        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public PlayerDetail PlayerDetail { get; set; }
        public double StakeAmount { get; set; }
        public double OutcomeAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? OutcomeDate { get; set; }

        public GameTransaction(
            string transactionType, 
            string gameId,
            string reference, 
            int playerId,
            double stakeAmount,
            double outcomeAmount,
            DateTime createdDate)
        {
            TransactionType = transactionType;
            GameId = gameId;
            Reference = reference;
            PlayerId = playerId;
            StakeAmount = stakeAmount;
            OutcomeAmount = outcomeAmount;
            CreatedDate = createdDate;
        }

        public void SetGameTransactions(
            string transactionType,
            string gameId,
            string reference,
            int playerId,
            double stakeAmount,
            double outcomeAmount,
            DateTime createdDate,
            DateTime? outcomeDate
            )
        {
            TransactionType = transactionType;
            GameId = gameId;
            Reference = reference;
            PlayerId = playerId;
            StakeAmount = stakeAmount;
            OutcomeAmount = outcomeAmount;
            CreatedDate = createdDate;
            OutcomeDate = outcomeDate;
        }
    }
}
