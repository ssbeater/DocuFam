using Microsoft.EntityFrameworkCore;

namespace place_ms.models;


public class PlacesContext : DbContext
{
    public PlacesContext(DbContextOptions<PlacesContext> options) : base(options) { }

    public DbSet<Place> Places { get; set; }
}
