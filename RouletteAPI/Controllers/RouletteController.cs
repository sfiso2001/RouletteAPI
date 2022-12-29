using Microsoft.AspNetCore.Mvc;

namespace RouletteAPI.Controllers
{
    public class RouletteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("PlayerBalance")]
        public double GetPlayerBalance(long playerId)
        {
            return 0;
        }
    }
}
