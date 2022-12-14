using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString")))
            {
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                    ("select * from Coupon where ProductName = @ProductName", new { ProductName = productName });
                if (coupon == null)
                {
                    return new Coupon() { ProductName = "No Discount", Description = "No Discount Available", Amount= 0 };
                }
                else { return coupon; }
            }
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString")))
            {
                var affected = await connection.ExecuteAsync
                    ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", 
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

                if (affected == 0)
                {
                    return false;
                }
                else { return true; }
            }
        }

         public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString")))
            {
                var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE ProductName=@ProductName",
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

                if (affected == 0)
                {
                    return false;
                }
                else { return true; }
            }
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString")))
            {
                var affected = await connection.ExecuteAsync
                    ("delete from Coupon where ProductName = @ProductName", new { ProductName = productName });
                if (affected == 0)
                {
                    return false;
                }
                else { return true; }
            }
        }

       
    }
}
