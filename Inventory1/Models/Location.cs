namespace Inventory1.Models;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; } 
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}