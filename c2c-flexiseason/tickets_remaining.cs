using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace c2c_flexiseason
{
    public static class TicketsRemaining
    {
        [FunctionName("tickets_remaining")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var baseAddress = new Uri("https://tickets.c2c-online.co.uk/");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    var loginPageResult = await client.GetAsync("/c2c/en/account/Login.aspx");
                    loginPageResult.EnsureSuccessStatusCode();

                    Stream stream = await loginPageResult.Content.ReadAsStreamAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(stream);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$loginControl$EmailAddress", Environment.GetEnvironmentVariable("c2c_email")),
                        new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$loginControl$Password", Environment.GetEnvironmentVariable("c2c_pwd")),
                        new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$returnUrl","https://tickets.c2c-online.co.uk/c2c/en/account/ViewSmartcardHistory?selectedCardIndex=0"),
                        new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$loginControl$ssoatreq",""),
                        new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$loginControl$fwdu",""),
                        new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$loginControl$userLogonControlState","Recognised"),
                        new KeyValuePair<string, string>("JavaScriptEnabled","true"),
                        new KeyValuePair<string, string>("__EVENTTARGET","ctl00$mainContentPlaceHolder$loginControl$SignInButton"),
                        new KeyValuePair<string, string>("__EVENTARGUMENT",""),
                        new KeyValuePair<string, string>("__LASTFOCUS",""),
                        new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", GetHiddenValue(doc,"__VIEWSTATEGENERATOR")),
                        new KeyValuePair<string, string>("__VIEWSTATE", GetHiddenValue(doc, "__VIEWSTATE")),
                        new KeyValuePair<string, string>("__EVENTVALIDATION",GetHiddenValue(doc,"__EVENTVALIDATION"))
                    });

                    var smartCardPageResult = await client.PostAsync("c2c/en/account/Login.aspx", content);
                    smartCardPageResult.EnsureSuccessStatusCode();
                    stream = await smartCardPageResult.Content.ReadAsStreamAsync();
                    doc.Load(stream);

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(GetPassesRemaining(doc), Encoding.UTF8, "application/json")
                    };
                }
            }
        }
        static string GetHiddenValue(HtmlDocument doc, string fieldName)
        {
            var htmlField = doc.DocumentNode.SelectSingleNode($"//input[@type=\"hidden\"][@name=\"{fieldName}\"]/@value");
            return htmlField.Attributes["value"].Value;
        }
        static string GetPassesRemaining(HtmlDocument doc)
        {
            var passesRemainingDiv = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'PassesRemaining')]");
            return passesRemainingDiv.InnerText.Replace(" tickets remaining", "");
        }
    }
}
