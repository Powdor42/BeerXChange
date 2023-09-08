using Application.Messages;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

namespace Application.Projections;

public class UserCreditProjection  : MultiStreamProjection<UserCreditView, int>
{
    public UserCreditProjection()
    {
        // This is just specifying the aggregate document id
        // per event type. This assumes that each event
        // applies to only one aggregated view document

        // The easiest possible way to do this is to use
        // a common interface or base type, and specify
        // the identity rule on that common type
        Identity<IUserEvent>(x => x.UserId);
    }

    public void Apply(UserRegisteredEvent @event, UserCreditView view)
    {
        view.Id = @event.UserId;
        view.Name = @event.Name;
    }

    public void Apply(BeerAddedToFridgeEvent @event, UserCreditView view)
        => view.Credits++;
    
    public void Apply(BeerTakenFromFridgeEvent @event, UserCreditView view)
        => view.Credits--;
}

public class UserCreditView
{
    public int Id { get; set; }

    public string Name { get; set; }
    public int Credits { get; set; }
}