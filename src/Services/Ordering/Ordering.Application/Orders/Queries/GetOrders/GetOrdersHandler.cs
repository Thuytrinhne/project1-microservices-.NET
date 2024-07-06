using BuildingBlocks.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IAppDbContext dbContext)
        : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            // get order with pagination
            // return result
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);
            List<Order> orders = new();
            if (query.StatusOrder != -1)
            {
                 orders = await dbContext.Orders
                                    .Include(o => o.OrderItems)
                                    .Where(o => o.Status == (OrderStatus)query.StatusOrder)
                                    .OrderByDescending(o => o.CreatedAt)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize)
                                    .ToListAsync(cancellationToken);
            }
            else
            {
                 orders = await dbContext.Orders
                                   .Include(o => o.OrderItems)                                 
                                   .OrderByDescending(o => o.CreatedAt)
                                   .Skip(pageSize * pageIndex)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);
            }


            return new GetOrdersResult(
                    new PaginationResult<OrderDto>(
                        pageIndex,
                        pageSize,
                        totalCount,
                        orders.ToOrderDtoList()));
        }
    }
}
