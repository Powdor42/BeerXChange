namespace Application.Messages;

public record BeerAddedToFridgeEvent(int UserId, string BeerName);

public record BeerTakenFromFridgeEvent(int UserId, string BeerName);