using Application.Messages;
using Application.Storage.AggregateRoots;
using JasperFx.RuntimeCompiler.Scenarios;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class BeerController : ControllerBase
{
    private readonly IDocumentSession _session;
    private readonly ILogger<BeerController> _logger;
    
    public BeerController(
        IDocumentSession session,
        ILogger<BeerController> logger)
    {
        _session = session;
        _logger = logger;
    }

    [HttpPost(nameof(AddBeerToFridge))]
    public async Task<IActionResult> AddBeerToFridge(
        Guid fridgeId, 
        int userId,
        string beerName)
    {
        //add to fridge
        _ = _session.Events.Append(
            fridgeId,
            new BeerAddedToFridgeEvent(userId, beerName)
        );
        
        
        await _session.SaveChangesAsync();

        return Ok("beer added!");
    }
    
    [HttpPost(nameof(RemoveBeerFromFridge))]
    public async Task<IActionResult> RemoveBeerFromFridge(
        Guid fridgeId, 
        int userId,
        string beerName)
    {
        var result = _session.Events.Append(
            fridgeId,
            new BeerTakenFromFridgeEvent(userId, beerName)
        );

        await _session.SaveChangesAsync();

        return Ok("beer removed!");
    }
}