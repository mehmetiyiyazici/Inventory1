using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory1.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Quantity { get; set; } = 1;
    public DateTime? ExpirationDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; } 
    
    [NotMapped]
    public IFormFile? Photo { get; set; }
    public string? ImgPath { get; set; }

    public int AreaId { get; set; }
    public Area? Area { get; set; }
}