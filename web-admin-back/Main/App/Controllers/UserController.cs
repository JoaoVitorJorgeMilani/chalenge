using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Main.App.SignalR;
using Main.App.Domain.User;
using Main.App.Messaging;
using MongoDB.Bson;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMessagingService _messagingService;
    private readonly IUserService _service;
    private readonly HubService _hubService;

    public UserController(IUserService service, HubService hubService, IMessagingService messagingService)
    {
        _messagingService = messagingService;
        _service = service;
        _hubService = hubService;
    }

    [HttpPost]
    [Route("add")]
    public IResult Add(UserEntity user)
    {
        try
        {
            _service.Add(user);
            return Results.Created("", user);
        }
        catch (ValidationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("list")]
    public List<UserDto> Get([FromQuery] UserFilterModel filter)
    {
        return this._service.Get(filter);
    }

    [HttpGet]
    [Route("login")]
    public UserDto Login([FromQuery] string userName)
    {
        return _service.Login(userName);
    }

}
