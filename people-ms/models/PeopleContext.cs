using Microsoft.EntityFrameworkCore;

namespace people_ms.models;


public class PeopleContext : DbContext
{
    public PeopleContext(DbContextOptions<PeopleContext> options) : base(options) { }

    public DbSet<People> Peoples { get; set; }
}
