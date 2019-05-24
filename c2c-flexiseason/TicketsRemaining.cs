using c2c_flexiseason.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace c2c_flexiseason
{
    public class TicketsRemaining
    {
        private readonly ITicketsService _ticketsService;
        public TicketsRemaining(ITicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }
        [FunctionName("tickets_remaining")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var ticketsRemaining = await _ticketsService.GetTicketsRemaining();

            return new JsonResult(ticketsRemaining.ToString());
        }

        
    }
}
