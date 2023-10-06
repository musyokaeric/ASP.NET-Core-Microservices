using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interface;
using Discount.GRPC.GrpcServices;
using EventBus.Message.Events;
using MassTransit;
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
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;

        public BasketController(IBasketRepository basketRepository, DisccountGrpcService disccountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this.basketRepository = basketRepository;
            this.disccountGrpcService = disccountGrpcService;
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // Get existing basket with total price
            var basket = await basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // Create basketCheckoutEvent - Set the total price on the basketCheckoutEvent message
            var eventMessage = mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            // Send checkout event message to rabbitmq
            await publishEndpoint.Publish(eventMessage);

            // Clear/remove the basket
            await basketRepository.DeleteBasket(basketCheckout.UserName);
            return Accepted();
        }
    }
}