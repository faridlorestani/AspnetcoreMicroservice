using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
        {
            _repository = repository;
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        }

        [HttpGet("{userName}", Name = "GetShoppingCart")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(string userName)
        {
            var basket = await _repository.GetShoppingCart(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPut]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateShoppingCart([FromBody] ShoppingCart shoppingCart)
        {
            // TODO: Consume Discount.Grpc service to calculate total amount for our shopping cart according to 
            // available discounts
            foreach (var item in shoppingCart.Items)
            {
              var coupon = await  _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;

            }
            return Ok(await _repository.UpdateShoppingCart(shoppingCart));
        }

        [HttpDelete("{userName}", Name = "DeleteShoppingCart")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteShoppingCart(string userName)
        {
            await _repository.DeleteShoppingCart(userName);
            return Ok();
        }
    }
}
