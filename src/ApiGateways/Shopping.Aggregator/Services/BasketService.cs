﻿using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;

namespace Shopping.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient client;

        public BasketService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await client.GetAsync($"/api/v1/Basket/{userName}");
            return await response.ReadContentAs<BasketModel>();
        }
    }
}
