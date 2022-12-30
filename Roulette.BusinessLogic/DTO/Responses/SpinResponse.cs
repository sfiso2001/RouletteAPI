using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.BusinessLogic.DTO.Responses
{
    public class SpinResponse : ResponseBase
    {
        public string GameId { get; set; }
        public int PlayerId { get; set; }
        public double WinAmount { get; set; }
    }
}
