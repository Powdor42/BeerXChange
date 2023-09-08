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
    private readonly IDocumentSession _documentSession;
    private readonly IQuerySession _querySession;
    private readonly ILogger<BeerController> _logger;
    
    public UserController(
        IDocumentSession documentSession,
        IQuerySession querySession,
        ILogger<BeerController> logger)
    {
        _documentSession = documentSession;
        _querySession = querySession;
        _logger = logger;
    }

    [HttpPost(nameof(RegisterUser))]
    public async Task<IActionResult> RegisterUser(int userId, string userName)
    {
        //add to fridge
        _ = _documentSession.Events.Append(
            Guid.NewGuid(),
            new UserRegisteredEvent(userId, userName)
        );
        
        
        await _documentSession.SaveChangesAsync();

        return Ok("user registered!");
    }
    
    [HttpGet("{userid}/credits")]
    public async Task<IActionResult> Get(int userid)
    {
        var userCredits = _querySession.Query<UserCreditView>().First(i => i.Id == userid);
        return Ok(userCredits);
    }
}