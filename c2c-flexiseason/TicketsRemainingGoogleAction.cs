using c2c_flexiseason.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace c2c_flexiseason
{
    public class TicketsRemainingGoogleAction
    {
        private readonly ITicketsService _ticketsService;
        public TicketsRemainingGoogleAction(ITicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }

        [FunctionName("TicketsRemainingGoogleAction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "google/tickets")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var ticketsRemaining = await _ticketsService.GetTicketsRemaining();

            return new JsonResult(
                new { 
                    payload = new
                    {
                        google = new
                        {
                            richResponse = new
                            {
                                items = new[]
                                { 
                                    new {
                                        simpleResponse = new
                                        {
                                            textToSpeach = $"You have {ticketsRemaining} tickets left."
                                        }
                                }
                                }
                            }
                        }
                    }
                }
            );
        }
    }
}
