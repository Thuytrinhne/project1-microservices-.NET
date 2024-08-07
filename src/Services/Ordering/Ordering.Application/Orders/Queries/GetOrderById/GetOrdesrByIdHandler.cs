using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public class GetOrdersByNameHandler (IAppDbContext dbContext)
        : IQueryHandler<GetOrdersByIdQuery, GetOrdersByIdResult>
    {
        public async  Task<GetOrdersByIdResult> Handle(GetOrdersByIdQuery query, CancellationToken cancellationToken)
        {
            // get orders by name using dbContext
            // return result
            var order = await dbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.Id == OrderId.Of(query.Id))
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
            return new GetOrdersByIdResult(order.ToOrderDto());
        }

      
    }
}
