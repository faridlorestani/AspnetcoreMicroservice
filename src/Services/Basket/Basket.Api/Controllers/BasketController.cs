using Basket.Api.Entities;
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

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
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
