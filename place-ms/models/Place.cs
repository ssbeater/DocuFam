using System;

namespace place_ms.models;

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }

    public Place(int id, string name, string description, string location)
    {
        Id = id;
        Name = name;
        Description = description;
        Location = location;
    }
}
