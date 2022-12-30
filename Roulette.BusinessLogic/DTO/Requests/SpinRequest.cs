using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.BusinessLogic.DTO.Requests
{
    public class SpinRequest
    {
        public string GameId { get; set; }
        public string Reference { get; set; }
        public int PlayerId { get; set; }
        public double WinAmount { get; set; }
    }
}
