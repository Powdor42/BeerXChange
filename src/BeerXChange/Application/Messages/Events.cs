namespace Application.Messages;

public record BeerAddedToFridgeEvent(int UserId, string BeerName) : IUserEvent;

public record BeerTakenFromFridgeEvent(int UserId, string BeerName) : IUserEvent;

public record UserRegisteredEvent(int UserId, string Name) : IUserEvent;