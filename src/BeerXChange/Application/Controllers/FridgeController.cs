using Application.Messages;
using Application.Models;
using Application.Storage.AggregateRoots;
using JasperFx.RuntimeCompiler.Scenarios;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class FridgeController : ControllerBase
{
    private readonly IDocumentSession _session;
    private readonly ILogger<BeerController> _logger;

    public FridgeController(
        IDocumentSession session,
        ILogger<BeerController> logger)
    {
        _session = session;
        _logger = logger;
    }

    [HttpPost(nameof(AddFridge))]
    public async Task<IActionResult> AddFridge(Guid fridgeId, string location)
    {
        var user = await _session.LoadAsync<Fridge>(fridgeId)
                   ?? new Fridge
                   {
                       Id = fridgeId
                   };

        user.Location = location;
        _session.Store(user);

        await _session.SaveChangesAsync();
        return Ok("fridge added!");
    }

    [HttpPost(nameof(RemoveFridge))]
    public async Task<IActionResult> RemoveFridge(Guid fridgeId)
    {
        var user = await _session.LoadAsync<Fridge>(fridgeId);
        if (user != null)
        {
            _session.Delete<Fridge>(fridgeId);
        }

        await _session.SaveChangesAsync();
        return Ok("fridge removed!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fridgeList = new List<Fridge>();
        var fridges = _session.Query<Fridge>().ToAsyncEnumerable();
        await foreach (var fridge in fridges)
        {
            fridgeList.Add(fridge);
        }
        
        return Ok(fridgeList);
    }

    [HttpGet("{fridgeId}")]
    public async Task<IActionResult> Get(Guid fridgeId)
    {
        var user = await _session.LoadAsync<Fridge>(fridgeId);
        return Ok(user);
    }

    [HttpGet("{fridgeId}/beers")]
    public async Task<IActionResult> GetBeers(Guid fridgeId)
    {
        var fridge = await _session.Events.AggregateStreamAsync<FridgeAggregate>(fridgeId);
        return Ok(fridge);
    }
}