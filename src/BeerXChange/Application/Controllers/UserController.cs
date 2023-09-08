using Application.Messages;
using Application.Projections;
using Application.Storage.AggregateRoots;
using JasperFx.RuntimeCompiler.Scenarios;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IQuerySession _session;
    private readonly ILogger<BeerController> _logger;
    
    public UserController(
        IQuerySession session,
        ILogger<BeerController> logger)
    {
        _session = session;
        _logger = logger;
    }

    [HttpGet("{userid}/credits")]
    public async Task<IActionResult> Get(int userid)
    {
        var userCredits = _session.Query<UserCreditView>().First(i => i.Id == userid);
        return Ok(userCredits);
    }
}