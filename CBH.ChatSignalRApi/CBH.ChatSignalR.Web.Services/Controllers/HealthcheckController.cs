using Microsoft.AspNetCore.Mvc;

namespace CBH.ChatSignalR.Web.Services.Controllers
{
    [Route("HealthCheck")]
    public class HealthcheckController : Controller
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Get()
        {
            _logger.Info("Health Check - OK");

            return Ok("OK");
        }
    }
}