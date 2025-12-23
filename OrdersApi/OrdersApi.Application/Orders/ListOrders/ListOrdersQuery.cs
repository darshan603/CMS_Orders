using MediatR;
using OrdersApi.Application.Common.Dtos;
using OrdersApi.Application.Common.Models;

namespace OrdersApi.Application.Orders.ListOrders
{
    public class ListOrdersQuery : IRequest<PagedResult<OrderListItemDto>>
    {
        // Filtering
        public string? Status { get; init; }          
        public DateTime? FromDate { get; init; }      
        public DateTime? ToDate { get; init; }       
        public decimal? MinTotal { get; init; }       

        // Sorting
        public string? Sort { get; init; }            
        public string? Dir { get; init; }            

        // Pagination
        public int Page { get; init; } = 1;         
        public int PageSize { get; init; } = 10;     
    }
}
