using AspnetRunBasics.Models;
using AspnetRunBasics.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public IndexModel(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
        }

        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await catalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await catalogService.GetCatalog(productId);

            var username = "swn";
            var basket = await basketService.GetBasket(username);

            basket.Items.Add(new BasketItemModel
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black"
            });

            var basketUpdated = await basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}