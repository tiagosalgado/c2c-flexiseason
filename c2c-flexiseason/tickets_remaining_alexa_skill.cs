using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace c2c_flexiseason
{
    public static class tickets_remaining_alexa_skill
    {
        [FunctionName("tickets_remaining_alexa_skill")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            
            var helper = new WebPageHelper();
            HtmlDocument doc = await helper.GetTicketsPageHtmlDocument();
            var ticketsRemaining = helper.GetTicketsRemaining(doc);

            return req.CreateResponse(HttpStatusCode.OK, new
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
