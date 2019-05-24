﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace c2c_flexiseason.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly ApiSettings _settings;
        public TicketsService(IOptions<ApiSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<int> GetTicketsRemaining()
        {
            var baseAddress = new Uri(_settings.BaseUrl);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var json = new Credentials { Username = _settings.Username, Password = _settings.Password };
                var requestContent = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

                var authResult = await client.PostAsync("pico/v1/auth", requestContent);
                await authResult.Content.ReadAsStringAsync();

                var loginPageResult = await client.GetAsync("/pico/v1/user");
                loginPageResult.EnsureSuccessStatusCode();
                await loginPageResult.Content.ReadAsStringAsync();

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
    }
}
