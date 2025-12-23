using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersApi.Application.Common.Dtos;
using OrdersApi.Application.Common.Models;
using OrdersApi.Application.Interfaces;
using OrdersApi.Application.Orders.CreateOrder;
using OrdersApi.Application.Orders.DeleteOrder;
using OrdersApi.Application.Orders.GetOrderById;
using OrdersApi.Application.Orders.ListOrders;
using OrdersApi.Application.Orders.UpdateOrderStatus;

namespace OrdersApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IIdempotencyStore _idempotency;
        public OrdersController(IMediator mediator, IIdempotencyStore idempotency)
        {
            _mediator = mediator;
            _idempotency = idempotency;
        }
        /// <summary>
        /// Creates a new order with status Pending.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderDetailsDto>> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
        {
            // Read Idempotency-Key header
            if (Request.Headers.TryGetValue("Idempotency-Key", out var keyValues))
            {
                var key = keyValues.ToString();

                if (!string.IsNullOrWhiteSpace(key))
                {
                    // If key already exists, return the existing order (no duplicate)
                    var existingOrderId = await _idempotency.GetOrderIdAsync(key, ct);
                    if (existingOrderId.HasValue)
                    {
                        var existing = await _mediator.Send(new GetOrderByIdQuery { Id = existingOrderId.Value }, ct);

                        // acceptable for idempotent replay
                        return Ok(existing);
                    }

                    // Otherwise create a new order
                    var created = await _mediator.Send(command, ct);

                    // Save idempotency mapping
                    await _idempotency.SaveAsync(key, created.Id, ct);

                    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
                }
            }
            var createdNoKey = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = createdNoKey.Id }, createdNoKey);
        }
        /// <summary>
        /// Get order Details by Id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDetailsDto>> GetById(Guid id, CancellationToken ct)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery { Id = id }, ct);
            return Ok(order);
        }

        /// <summary>
        /// Update order status.
        /// </summary>
        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult<OrderStatusUpdatedDto>> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest body, CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdateOrderStatusCommand
            {
                Id = id,
                Status = body.Status
            }, ct);

            return Ok(result);
        }

        /// <summary>
        /// Delete an order by Id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteOrderCommand { Id = id }, ct);
            return NoContent();
        }

        /// <summary>
        /// Get a paginated list of orders with optional filtering and sorting.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderListItemDto>>> List(
        [FromQuery] string? status,
        [FromQuery] DateOnly? fromDate,
        [FromQuery] DateOnly? toDate,
        [FromQuery] decimal? minTotal,
        [FromQuery] string? sort,
        [FromQuery] string? dir,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
        {
            var result = await _mediator.Send(new ListOrdersQuery
            {
                Status = status,
                // Convert DateOnly to DateTime range
                FromDate = fromDate?.ToDateTime(TimeOnly.MinValue), 
                ToDate = toDate?.ToDateTime(TimeOnly.MaxValue),
                MinTotal = minTotal,
                Sort = sort,
                Dir = dir,
                Page = page,
                PageSize = pageSize
            }, ct);

            return Ok(result);
        }
        public class UpdateStatusRequest
        {
            public string Status { get; set; } = string.Empty;
        }
    }
}
