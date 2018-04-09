using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace c2c_flexiseason
{
    public class WebPageHelper {
        string GetHiddenValue(HtmlDocument doc, string fieldName)
            =>
            doc.DocumentNode.SelectSingleNode($"//input[@type=\"hidden\"][@name=\"{fieldName}\"]/@value")?
            .Attributes["value"]?
            .Value;
        
        public async Task<HtmlDocument> GetTicketsPageHtmlDocument()
        {
            HtmlDocument doc = new HtmlDocument();
            var baseAddress = new Uri("https://tickets.c2c-online.co.uk/");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    var loginPageResult = await client.GetAsync("/c2c/en/account/Login.aspx");
                    loginPageResult.EnsureSuccessStatusCode();

                    Stream stream = await loginPageResult.Content.ReadAsStreamAsync();

                    doc.Load(stream);
                    FormUrlEncodedContent content = GetFormUrlEncodedContent(doc);

                    var smartCardPageResult = await client.PostAsync("c2c/en/account/Login.aspx", content);
                    smartCardPageResult.EnsureSuccessStatusCode();
                    stream = await smartCardPageResult.Content.ReadAsStreamAsync();
                    doc.Load(stream);
                }
            }

            return doc;
        }



        public string GetTicketsRemaining(HtmlDocument doc)
        {
            var ticketsRemainingDiv = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'PassesRemaining')]");
            return ticketsRemainingDiv.InnerText.Replace(" tickets remaining", "");
        }

        public FormUrlEncodedContent GetFormUrlEncodedContent(HtmlDocument doc)
            => new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$loginControl$EmailAddress", GetVariableValue("c2c_email")),
                    new KeyValuePair<string, string>("ctl00$mainContentPlaceHolder$loginControl$Password", GetVariableValue("c2c_pwd")),
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

        bool IsLocal => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
        string GetVariableValue(string name) 
            => IsLocal ? ConfigurationManager.AppSettings[name] : Environment.GetEnvironmentVariable(name); 
    }
}