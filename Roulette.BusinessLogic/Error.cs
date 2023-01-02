using Newtonsoft.Json;
using Roulette.BusinessLogic.DTO.Responses;

namespace Roulette.BusinessLogic
{
    public class Error : ErrorResponse
    {
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
