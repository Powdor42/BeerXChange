using Application.Messages;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

namespace Application.Projections;

public class UserCreditProjection : MultiStreamProjection<UserCreditView, int>
{
    public UserCreditProjection()
    {
        Identity<IUserEvent>(x => x.UserId);
    }

    public void Apply(UserRegisteredEvent @event, UserCreditView view)
    {
        view.Name = @event.Name;
    }

    public void Apply(BeerAddedToFridgeEvent @event, UserCreditView view)
    {
        view.Beers.Add(@event.BeerName);
        view.Credits++;
    }

    public void Apply(BeerTakenFromFridgeEvent @event, UserCreditView view)
    {
        view.Beers.Remove(@event.BeerName);
        view.Credits--;
    }
}

public class UserCreditView
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public int Credits { get; set; }

    public List<string> Beers { get; } = new();
}