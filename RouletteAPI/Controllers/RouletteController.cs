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
        public async Task<IActionResult> GetPlayerBalance(PlayerBalanceRequest playerBalanceRequest)
        {
            var playerBalanceResult = await _transactionsBL.PlayerBalanceAsync(playerBalanceRequest);

            return Ok(JsonConvert.SerializeObject(playerBalanceResult));
        }

        /// <summary>
        /// Method to place player bet.
        /// </summary>
        /// <param name="placeBetRequest"></param>
        /// <returns>PlaceBetResponse</returns>
        [HttpPost("PlaceBet")]
        public async Task<IActionResult> PlaceBet(PlaceBetRequest placeBetRequest)
        {
            var placeBetResult = await _transactionsBL.DebitTransactionAsync(placeBetRequest);

            return Ok(JsonConvert.SerializeObject(placeBetResult));
        }

        /// <summary>
        /// Method to play Spin for an intial Bet
        /// </summary>
        /// <param name="spinRequest"></param>
        /// <returns>SpinResponse Object</returns>
        [HttpPost("Spin")]
        public async Task<IActionResult> Spin(SpinRequest spinRequest)
        {
            var spinResult = await _transactionsBL.PlaySpinAsync(spinRequest);

            return Ok(JsonConvert.SerializeObject(spinResult));
        }

        /// <summary>
        /// Method to Payout Player
        /// </summary>
        /// <param name="payoutRequest"></param>
        /// <returns>PayoutResponse Object</returns>
        [HttpPost("Payout")]
        public async Task<IActionResult> Payout(PayoutRequest payoutRequest)
        {
            var payoutResult = await _transactionsBL.CreditPlayerAsync(payoutRequest);

            return Ok(JsonConvert.SerializeObject(payoutResult));
        }

        [HttpGet("PreviousSpins")]
        public async Task<IActionResult> ShowPreviousSpins(string reference)
        {
            var previousSpins = await _transactionsBL.ShowPreviousSpinsAsync(reference);

            return Ok(JsonConvert.SerializeObject(previousSpins));
        }
    }
}
