using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CBH.Chat.Web.Services.Controllers
{
    [Route("HealthCheck")]
    public class HealthcheckController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Get()
        {
            _logger.Info("Health Check - OK");

            return Ok("OK");
        }
    }
}