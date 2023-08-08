using Microsoft.EntityFrameworkCore;

namespace Application.Storage;

public class BeerXChangeDbContext : DbContext
{
    public BeerXChangeDbContext (DbContextOptions<BeerXChangeDbContext> options)
        : base(options)
    {
    }
}