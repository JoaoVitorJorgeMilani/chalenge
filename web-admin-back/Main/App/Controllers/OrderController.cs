using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Main.App.Domain.Order;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService service;

    public OrderController(IOrderService _service)
    {
        service = _service;
    }

    [HttpPost]
    [Route("add")]
    public IResult Add(OrderEntity order)
    {
        try
        {
            if (service.Add(order))
            {
                return Results.Created("", order);
            }
            return Results.UnprocessableEntity(null);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("list")]
    public List<OrderDto> Get([FromQuery] OrderFilterModel filter)
    {
        return service.Get(filter);
    }

    [HttpGet]
    [Route("available")]
    public async Task<List<OrderDto>> GetAvailableOrders()
    {
        return await service.GetAvailableOrders();
    }

    [HttpPut]
    [Route("accept")]
    public async Task<ActionResult<OrderDto>> AcceptOrder([FromQuery] string orderId, [FromQuery] string userId)
    {
        try
        {
            OrderDto order = await service.AcceptOrder(orderId, userId);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpPut]
    [Route("decline")]
    public async Task<IResult> DeclineOrder([FromQuery] string orderId, [FromQuery] string userId)
    {
        try
        {
            await service.DeclineOrder(orderId, userId);
            return Results.Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);

        }
        catch (Exception)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpGet]
    [Route("delivering")]
    public async Task<ActionResult<OrderDto>> GetDeliveringOrder([FromQuery] string encryptedOrderId)
    {
        try
        {
            OrderDto order = await service.GetById(encryptedOrderId);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }


}

