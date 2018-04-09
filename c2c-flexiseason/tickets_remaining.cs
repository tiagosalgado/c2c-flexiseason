using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace c2c_flexiseason
{
    public static class TicketsRemaining
    {   
        [FunctionName("tickets_remaining")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            var helper = new WebPageHelper();
            HtmlDocument doc = await helper.GetTicketsPageHtmlDocument();
            var ticketsRemaining = helper.GetTicketsRemaining(doc);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(ticketsRemaining, Encoding.UTF8, "application/json")
            };
        }

        
    }
}
