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
    public IResult Add(Order order)
    {
        try
        {
            if(service.Add(order))
            {
                return Results.Created("", order);
            }
            return Results.UnprocessableEntity(null);            
        } 
        catch (ValidationException ex ) 
        {
            return Results.BadRequest(ex.Message);
        } catch (Exception)
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




}

