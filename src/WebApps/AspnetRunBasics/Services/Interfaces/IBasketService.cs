﻿using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
        Task<BasketModel> UpdateBasket(BasketModel model);
        Task CheckoutBasket(BasketCheckoutModel model);
    }
}