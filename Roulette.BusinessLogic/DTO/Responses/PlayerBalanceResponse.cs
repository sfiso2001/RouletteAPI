using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.BusinessLogic.DTO.Responses
{
    public class PlayerBalanceResponse : ResponseBase
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public double Balance { get; set; }
    }
}
