using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.BusinessLogic.Interfaces;

namespace RouletteAPI.Controllers
{
    public class RouletteController : Controller
    {
        private readonly ITransactionsBL _transactionsBL;
        public RouletteController(ITransactionsBL transactionsBL)
        {
            _transactionsBL = transactionsBL;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method to get player details including current balance, player name and unique Id
        /// </summary>
        /// <param name="playerBalanceRequest"></param>
        /// <returns>PLayerBalance</returns>
        [HttpGet("PlayerBalance")]
        public IActionResult GetPlayerBalance(PlayerBalanceRequest playerBalanceRequest)
        {
            var playerBalanceResult = _transactionsBL.PlayerBalance(playerBalanceRequest);

            return Ok(JsonConvert.SerializeObject(playerBalanceResult));
        }

        /// <summary>
        /// Method to place player bet.
        /// </summary>
        /// <param name="placeBetRequest"></param>
        /// <returns>PlaceBetResponse</returns>
        [HttpPost("PlaceBet")]
        public IActionResult PlaceBet(PlaceBetRequest placeBetRequest)
        {
            var placeBetResult = _transactionsBL.DebitTransaction(placeBetRequest);

            return Ok(JsonConvert.SerializeObject(placeBetResult));
        }
    }
}
