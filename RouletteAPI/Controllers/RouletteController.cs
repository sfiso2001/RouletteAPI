using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("PlayerBalance")]
        public IActionResult GetPlayerBalance(PlayerBalanceRequest playerBalanceRequest)
        {
            var playerBalanceResult = _transactionsBL.PlayerBalance(playerBalanceRequest);

            return default;
        }
    }
}
