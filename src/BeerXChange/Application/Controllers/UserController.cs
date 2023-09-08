using Application.Messages;
using Application.Models;
using Application.Projections;
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

    private const string UserStream = "3ACF5DC6-6B6F-4A99-B55E-244EFF8FA3E2";

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
        var user = await _documentSession.LoadAsync<User>(userId)
                   ?? new User
                   {
                       Id = userId
                   };

        user.Name = userName;
        _documentSession.Store(user);

        _ = _documentSession.Events.Append(
            new Guid(UserStream),
            new UserRegisteredEvent(userId, userName)
        );

        await _documentSession.SaveChangesAsync();
        return Ok("user registered!");
    }

    [HttpPost(nameof(RemoveUser))]
    public async Task<IActionResult> RemoveUser(int userId)
    {
        var user = await _documentSession.LoadAsync<User>(userId);
        if (user != null)
        {
            _documentSession.Delete<User>(userId);
        }

        await _documentSession.SaveChangesAsync();
        return Ok("user unregistered!");
    }

    [HttpGet("{userid}")]
    public async Task<IActionResult> Get(int userid)
    {
        var user = await _documentSession.LoadAsync<User>(userid);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userList = new List<User>();
        var users = _querySession.Query<User>().ToAsyncEnumerable();
        await foreach (var user in users)
        {
            userList.Add(user);
        }
        return Ok(userList);
    }

    [HttpGet("{userid}/credits")]
    public async Task<IActionResult> GetCredits(int userid)
    {
        var userCredits = await _documentSession.LoadAsync<UserCreditView>(userid);
        return Ok(userCredits);
    }
}