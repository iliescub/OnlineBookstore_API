using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.UseCases.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace BookstoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableRateLimiting("api")]
public class OrdersController : ControllerBase
{
    private readonly GetAllOrdersUseCase _getAllOrdersUseCase;
    private readonly GetOrdersByUserIdUseCase _getOrdersByUserIdUseCase;
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly CancelOrderUseCase _cancelOrderUseCase;
    private readonly CompleteOrderUseCase _completeOrderUseCase;
    private readonly CloseOrderUseCase _closeOrderUseCase;

    public OrdersController(
        GetAllOrdersUseCase getAllOrdersUseCase,
        GetOrdersByUserIdUseCase getOrdersByUserIdUseCase,
        CreateOrderUseCase createOrderUseCase,
        CancelOrderUseCase cancelOrderUseCase,
        CompleteOrderUseCase completeOrderUseCase,
        CloseOrderUseCase closeOrderUseCase)
    {
        _getAllOrdersUseCase = getAllOrdersUseCase;
        _getOrdersByUserIdUseCase = getOrdersByUserIdUseCase;
        _createOrderUseCase = createOrderUseCase;
        _cancelOrderUseCase = cancelOrderUseCase;
        _completeOrderUseCase = completeOrderUseCase;
        _closeOrderUseCase = closeOrderUseCase;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _getAllOrdersUseCase.ExecuteAsync();
        return Ok(orders);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(string userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("admin");

        if (currentUserId != userId && !isAdmin)
        {
            return Forbid();
        }

        var orders = await _getOrdersByUserIdUseCase.ExecuteAsync(userId);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("admin");

        if (currentUserId != request.UserId && !isAdmin)
        {
            return Forbid();
        }

        try
        {
            var order = await _createOrderUseCase.ExecuteAsync(request);

            if (order == null)
            {
                return BadRequest(new { error = "Failed to create order" });
            }

            return CreatedAtAction(nameof(GetUserOrders), new { userId = order.UserId }, order);
        }
        catch (InvalidOperationException ex)
        {
            // Stock validation errors
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            // Log the error in production
            return StatusCode(500, new { error = "An error occurred while creating the order" });
        }
    }

    [HttpPatch("{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(string orderId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("admin");

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        try
        {
            var success = await _cancelOrderUseCase.ExecuteAsync(orderId, currentUserId, isAdmin);

            if (!success)
            {
                return NotFound(new { error = "Order not found" });
            }

            return Ok(new { message = "Order cancelled successfully" });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An error occurred while cancelling the order" });
        }
    }

    [HttpPatch("{orderId}/complete")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CompleteOrder(string orderId)
    {
        try
        {
            var success = await _completeOrderUseCase.ExecuteAsync(orderId);

            if (!success)
            {
                return NotFound(new { error = "Order not found" });
            }

            return Ok(new { message = "Order marked as completed" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An error occurred while completing the order" });
        }
    }

    [HttpPatch("{orderId}/close")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CloseOrder(string orderId)
    {
        try
        {
            var success = await _closeOrderUseCase.ExecuteAsync(orderId);

            if (!success)
            {
                return NotFound(new { error = "Order not found" });
            }

            return Ok(new { message = "Order marked as closed (delivery confirmed)" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An error occurred while closing the order" });
        }
    }
}
