namespace Inventory1.Models;

public class Area
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Photo  { get; set; }
    public DateTime CreatedDate { get; set; } 
    public DateTime UpdatedDate { get; set; }
    public ICollection<Item> Items { get; set; } = new List<Item>();
    
    public int SectionId { get; set; }
    public Section? Section { get; set; }
}