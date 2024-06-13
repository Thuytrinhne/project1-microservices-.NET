
namespace Basket.API.Data
{
    public class BasketRepository (IDocumentSession session) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            session.Delete<ShoppingCart>(userName);
            await  session.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);
            return basket is null ? throw new  BasketNotFoundException(userName): basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            // upsert function from marten
            // -> if cart doesn't exist into database, Marten will insert it
            // if cart is already existing, Marten will be updated

            

            var cartFrmDb = await session.Query<ShoppingCart>()
                                 .FirstOrDefaultAsync(p => p.UserName == basket.UserName, cancellationToken);
            if (cartFrmDb is  null) {

                session.Store(basket);
                await session.SaveChangesAsync(cancellationToken);

                return basket;
            }
            else

            {
                foreach (var item in basket.Items)
                {
                   var ItemFrmDb= cartFrmDb.Items.FirstOrDefault (p => p.ProductId == item.ProductId);
                    if (ItemFrmDb is null)
                    {
                        cartFrmDb.Items.Add(item);
                    }
                    else
                    {
                       
                        ItemFrmDb.Quantity += item.Quantity;
                    }
                }
                session.Update(cartFrmDb);
                await session.SaveChangesAsync(cancellationToken);
                return cartFrmDb;

            }



           
        }
    }
}
