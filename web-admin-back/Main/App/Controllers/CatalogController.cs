using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Main.App.Domain.Bike;

[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly IBikeService service;

    public CatalogController(IBikeService _service)
    {
        service = _service;
    }

    [HttpPost]
    [Route("add")]
    public IResult Add(Bike bike)
    {
        try 
        {
            service.Add(bike);
            return Results.Created("", bike);
        } 
        catch (ValidationException ex ) 
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("list")]
    public List<BikeDto> Get([FromQuery] BikeFilterModel filter)
    {
        return this.service.Get(filter);
    }

    [HttpDelete]
    [Route("delete")]
    public IResult Delete([FromQuery] string encryptedId)
    {
        if(this.service.Delete(encryptedId))
        {
            return Results.Accepted();
        }
        else
        {
            return Results.NotFound("Bike not found");
        }
        
    }

    [HttpPut]
    [Route("edit")]
    public IResult Edit([FromQuery] string encryptedId, [FromQuery] string licensePlate)
    {
        try 
        {
            service.Edit(encryptedId, licensePlate);
            return Results.Accepted();
        } 
        catch (ValidationException ex ) 
        {
            return Results.BadRequest(ex.Message);
        }
    }



}
