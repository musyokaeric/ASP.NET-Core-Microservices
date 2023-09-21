using Basket.API.Entities;
using Basket.API.Repositories.Interface;
using Discount.GRPC.GrpcServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly DisccountGrpcService disccountGrpcService;

        public BasketController(IBasketRepository basketRepository, DisccountGrpcService disccountGrpcService)
        {
            this.basketRepository = basketRepository;
            this.disccountGrpcService = disccountGrpcService;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await basketRepository.GetBasket(username);

            return Ok(basket ?? new ShoppingCart(username));
        }

        /// <summary>
        /// Since we are using No-SQL Redis distributed cache as a database for Basket microservices,
        /// our basket data is always replacing with full json of basket data. So when we update the basket,
        /// actually we re-write the key-value pair of that user for overriding value part of full json data.
        /// 
        /// So this operation is more suitable for POST operation instead of sending PUT verb,
        /// because PUT is more suitable for RDMBS database row column updates.
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // Communicate with Discount.gRPC and calculate the latest product prices into shopping cart
            foreach (var item in basket.Items)
            {
                var coupon = await disccountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await basketRepository.UpdateBasket(basket));
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await basketRepository.DeleteBasket(username);
            return Ok();
        }
    }
}