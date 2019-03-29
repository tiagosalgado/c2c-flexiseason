using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace c2c_flexiseason
{
    public class TicketsService
    {
        public async Task<int> GetTicketsRemaining()
        {
            var baseAddress = new Uri("https://tickets.c2c-online.co.uk/");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var json = new Credentials { Username = GetVariableValue("c2c_email"), Password = GetVariableValue("c2c_pwd") };
                var requestContent = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

                var authResult = await client.PostAsync("pico/v1/auth", requestContent);
                var response = await authResult.Content.ReadAsStringAsync();

                var loginPageResult = await client.GetAsync("/pico/v1/user");
                loginPageResult.EnsureSuccessStatusCode();
                var loginResponse = await loginPageResult.Content.ReadAsStringAsync();

                var smartCardsResponse = await client.GetAsync("/pico/v1/card/smartcards");
                var smartCardsContent = await smartCardsResponse.Content.ReadAsStringAsync();

                var smartCards = JsonConvert.DeserializeObject<IEnumerable<Smartcard>>(smartCardsContent);
                var activeSmartCard = smartCards.FirstOrDefault();
                var productsResponse = await client.GetAsync($"/pico/v1/card/{activeSmartCard.SerialNumber}/products");
                productsResponse.EnsureSuccessStatusCode();

                var productsContent = await productsResponse.Content.ReadAsStringAsync();

                var products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(productsContent);

                var currentFlexiSeason = products.Where(p => p.pTyp.Equals("5")).OrderByDescending(p => p.Product.ExpiryDateCurrent).FirstOrDefault();
                if (int.TryParse(currentFlexiSeason.Product.NumberRemainingPasses, out var numberOfTickets))
                {
                    return numberOfTickets;
                }
                return 0;
            }
        }

        bool IsLocal => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
        string GetVariableValue(string name)
            => IsLocal ? ConfigurationManager.AppSettings[name] : Environment.GetEnvironmentVariable(name);
    }
}
