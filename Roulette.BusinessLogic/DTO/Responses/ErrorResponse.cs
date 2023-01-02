using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.BusinessLogic.DTO.Responses
{
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
