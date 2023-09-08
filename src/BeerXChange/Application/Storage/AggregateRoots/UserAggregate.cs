using Application.Messages;

namespace Application.Storage.AggregateRoots;

public class UserAggregate
{
    public int Credits{ get; set; } 

    public void Apply(BeerAddedToFridgeEvent @event)
    {
        Credits++;
    }

    public void Apply(BeerTakenFromFridgeEvent @event)
    {
        Credits--;
    }
}