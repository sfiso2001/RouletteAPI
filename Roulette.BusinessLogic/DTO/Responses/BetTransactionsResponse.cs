using Roulette.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.BusinessLogic.DTO.Responses
{
    public class BetTransactionsResponse : ResponseBase
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public string GameId { get; set; }
        public string Reference { get; set; }
        public string SpinReference { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public double StakeAmount { get; set; }
        public double OutcomeAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? OutcomeDate { get; set; }
    }
}
