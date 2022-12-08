using Basket.Api.Entities;

namespace Basket.Api.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetShoppingCart(string userName);
        Task<ShoppingCart?> UpdateShoppingCart(ShoppingCart basket);
        Task DeleteShoppingCart(string userName);
    }
}
