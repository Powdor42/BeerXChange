using Application.Messages;

namespace Application.Storage.AggregateRoots;

public class FridgeAggregate
{
    public Guid Id { get; set; }
    
    public List<string> Beers { get; set; } = new();
    
    public void Apply(BeerAddedToFridgeEvent @event)
    {
        Beers.Add(@event.BeerName);
    }
    
    public void Apply(BeerTakenFromFridgeEvent @event)
    {
        Beers.Remove(@event.BeerName);
    }
}