using Application.Messages;
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

    [HttpGet("{streamId}")]
    public async Task<IActionResult> Get(Guid streamId)
    {
        var fridge = await _session.Events.AggregateStreamAsync<FridgeAggregate>(streamId);
        return Ok(fridge);
    }
}