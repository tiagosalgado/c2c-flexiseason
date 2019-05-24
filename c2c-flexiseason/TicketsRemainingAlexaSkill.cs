using c2c_flexiseason.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace c2c_flexiseason
{
    public class TicketsRemainingAlexaSkill
    {
        private readonly ITicketsService _ticketsService;
        public TicketsRemainingAlexaSkill(ITicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }
        [FunctionName("tickets_remaining_alexa_skill")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var ticketsRemaining = await _ticketsService.GetTicketsRemaining();

            return new OkObjectResult(new
            {
                version = "1.0",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = $"You have {ticketsRemaining} tickets left."
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Alexa c2c Flexi Season Tickets",
                        content = $"You have {ticketsRemaining} tickets left."
                    },
                    shouldEndSession = true
                }
            });
        }
    }
}
