using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace c2c_flexiseason
{
    public static class TicketsRemaining
    {   
        [FunctionName("tickets_remaining")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var service = new TicketsService();
            var ticketsRemaining = await service.GetTicketsRemaining();

            return new JsonResult(ticketsRemaining.ToString());
        }

        
    }
}
