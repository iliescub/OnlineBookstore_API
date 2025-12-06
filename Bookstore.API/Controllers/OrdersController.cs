using BookstoreAPI.Application.DTOs.Requests;
using BookstoreAPI.Application.UseCases.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookstoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly GetAllOrdersUseCase _getAllOrdersUseCase;
    private readonly GetOrdersByUserIdUseCase _getOrdersByUserIdUseCase;
    private readonly CreateOrderUseCase _createOrderUseCase;

    public OrdersController(
        GetAllOrdersUseCase getAllOrdersUseCase,
        GetOrdersByUserIdUseCase getOrdersByUserIdUseCase,
        CreateOrderUseCase createOrderUseCase)
    {
        _getAllOrdersUseCase = getAllOrdersUseCase;
        _getOrdersByUserIdUseCase = getOrdersByUserIdUseCase;
        _createOrderUseCase = createOrderUseCase;
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

        var order = await _createOrderUseCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetUserOrders), new { userId = order.UserId }, order);
    }
}
