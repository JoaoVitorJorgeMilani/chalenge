using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Main.App.SignalR;
using Main.App.Domain.User;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    private readonly HubService _hubService;

    public UserController(IUserService service, HubService hubService)
    {
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
        catch (ValidationException ex ) 
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

    [HttpGet]
    [Route("teste")]
    public async void TesteSSEConections([FromQuery] string testestr)
    {
        Console.WriteLine("TESTANDO signalR CONNECTIONS");
        
        await _hubService.SendMessageForAllUsers(testestr);
    }


}
