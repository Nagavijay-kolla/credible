using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// THIS IS A SAMPLE CONTROLLER.  PLEASE REMOVE THIS SOURCE FILE BEFORE DEPLOYING THIS CODE TO PRODUCTION.
namespace CBH.ChatSignalR.Web.Services.Controllers
{
    [Produces("application/json")]
    [Route("Values")]
    public class ValuesController : Controller
    {
        [HttpPost("TotalRecords")]
        public async Task<IActionResult> TotalRecords([FromBody]ParametersPoco theInput)
        {
            // fake await task
            var result = await Task.Run(() => new ReturnParametersPoco
            {
                TotalRecords = 5
            });

            return Ok(result);
        }
    }

    public class ParametersPoco
    {
        public int ClientId { get; set; }
        public bool IsNew { get; set; }
    }

    public class ReturnParametersPoco
    {
        public int TotalRecords { get; set; }
    }
}