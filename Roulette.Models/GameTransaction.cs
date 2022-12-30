using Roulette.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Roulette.Models
{
    public class GameTransaction
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string GameId { get; set; }
        public string Reference { get; set; }
        public string SpinReference { get; set; }
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
            string spinReference,
            int playerId,
            double stakeAmount,
            double outcomeAmount,
            DateTime createdDate)
        {
            TransactionType = transactionType.AssertIsNotNull(nameof(transactionType));
            GameId = gameId.AssertIsNotNull(nameof(gameId));
            Reference = reference.AssertIsNotNull(nameof(reference));
            SpinReference = spinReference.AssertIsNotNull(nameof(spinReference));
            PlayerId = playerId.AssertIsNotNull(nameof(playerId));
            StakeAmount = stakeAmount.AssertIsNotNull(nameof(stakeAmount));
            OutcomeAmount = outcomeAmount;
            CreatedDate = createdDate.AssertIsNotNull(nameof(createdDate));
        }

        public void SetGameTransactions(
            string transactionType,
            string gameId,
            string reference,
            string spinReference,
            int playerId,
            double stakeAmount,
            double outcomeAmount,
            DateTime? outcomeDate
            )
        {
            TransactionType = transactionType;
            GameId = gameId;
            Reference = reference;
            SpinReference = spinReference;
            PlayerId = playerId;
            StakeAmount = stakeAmount;
            OutcomeAmount = outcomeAmount;
            OutcomeDate = outcomeDate;
        }
    }
}
