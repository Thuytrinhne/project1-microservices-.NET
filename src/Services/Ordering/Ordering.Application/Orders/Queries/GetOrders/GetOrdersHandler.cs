using BuildingBlocks.Pagination;
using Ordering.Application.Dtos;
using Ordering.Domain.ValueObjects;
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
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            // Get the total count of orders
            var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

            // Initialize queryable orders
            var ordersQuery = dbContext.Orders
                .Include(o => o.OrderItems)
                .AsQueryable();

            // Apply filters if CustomerId is provided
            if (query.CustomerId != null && query.CustomerId != Guid.Empty)
            {
                var customerId = CustomerId.Of(query.CustomerId);
                ordersQuery = ordersQuery.Where(o => o.CustomerId == customerId);
            }

            // Apply filters if StatusOrder is provided
            if (query.StatusOrder != -1)
            {
                ordersQuery = ordersQuery.Where(o => o.Status == (OrderStatus)query.StatusOrder);
            }

            // Apply sorting, pagination, and retrieve the result
            var orders = await ordersQuery
                .OrderByDescending(o => o.CreatedAt)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // Return the paginated result
            return new GetOrdersResult(
                new PaginationResult<OrderDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    orders.ToOrderDtoList()));
        }
    }
}
