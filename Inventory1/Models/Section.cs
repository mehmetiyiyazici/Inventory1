namespace Inventory1.Models;

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; } 
    public DateTime UpdatedDate { get; set; } 
    public ICollection<Area> Areas { get; set; } = new List<Area>();
    
    public int LocationId { get; set; }
    public Location? Location { get; set; }
}