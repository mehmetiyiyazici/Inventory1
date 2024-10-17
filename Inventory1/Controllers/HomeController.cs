using Microsoft.AspNetCore.Mvc;
using Inventory1.Models;
using Inventory1.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var areaId = _context.Areas.FirstOrDefault();
        ViewBag.areaId = areaId.Id;
        
        return View(_context.Items.ToList());
    }
    
    public IActionResult Details(int id)
    {
        var detail = _context.Items
            .Include(x => x.Area)
            .ThenInclude(x => x.Section)
            .ThenInclude(x => x.Location)
            .FirstOrDefault(x => x.Id == id);
        
        ViewBag.Detail = detail;
        
        var item = _context.Items.Find(id);

        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    #region items

    [HttpPost]
    public IActionResult Items(Item model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        model.CreatedDate = DateTime.Now;
        model.UpdatedDate = DateTime.Now;
        
        _context.Items.Add(model);
        _context.SaveChanges();

        return RedirectToAction("CreateItem", new {name = model.Name, id = model.Id});
    }

    [Route("/UrunEkle")]
    public IActionResult CreateItem(string name, int id, int? sectionId, int? locationId)
    {
        
        var locations = _context.Locations
            .Include(x => x.Sections)
            .ThenInclude(x => x.Areas)
            .ToList();
        
        ViewBag.Location = locations;
        
        ViewBag.Name = name;
        ViewBag.Id = id;
        return View(new Item());
    }

    [HttpPost]
    [Route("/UrunEkle")]
    public IActionResult CreateItem(Item model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var item = _context.Items.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);

        if (item != null)
        {
            if (model.Photo != null && model.Photo.Length > 0)
            {
                string uploadsFolder = Path.Combine("wwwroot", "uploads");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName; 
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }

                model.ImgPath = uniqueFileName;
            }
            
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;

            _context.Update(model);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        return View();
    }

    public IActionResult GetSectionsByLocationId(int locationId)
    {
        var sections = _context.Sections.Where(x => x.LocationId == locationId).ToList();
        return Json(sections);  
    }

    public IActionResult GetAreasBySectionId(int sectionId)
    {
        var areas = _context.Areas.Where(x => x.SectionId == sectionId).ToList();
        return Json(areas);
    }

    [Route("/UrunGuncelle")]
    public IActionResult EditItem(int id)
    {
        var area = _context.Areas.ToList();
        ViewBag.Areas = area;
        
        return View(_context.Items.Find(id));
    }

    [HttpPost]
    [Route("/UrunGuncelle")]
    public IActionResult EditItem(Item model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        _context.Update(model);
        _context.SaveChanges();
        
        return RedirectToAction("Index");
    }

    public IActionResult DeleteItem(int id)
    {
        var item = _context.Items.Find(id);

        if (item == null)
        {
            return NotFound();
        }
        
        _context.Items.Remove(item);
        _context.SaveChanges();
        
        return RedirectToAction("Index");
    }

    #endregion

    #region areas

    public IActionResult Areas()
    {
        var sections = _context.Sections.Include(x => x.Areas).ToList();
        ViewBag.Sections = sections;
        
        return View(_context.Areas.ToList());
    }

    [HttpPost]
    public IActionResult CreateArea(Area model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        _context.Areas.Add(model);
        _context.SaveChanges();
        
        return RedirectToAction("Crud");
    }
    
    public IActionResult EditArea(int id)
    {
        var area = _context.Areas.Find(id);

        if(area == null)
        {
            return NotFound();
        }

        return View(area);
    }

    [HttpPost]
    public IActionResult EditArea(Area model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _context.Areas.Update(model);
        _context.SaveChanges();

        return RedirectToAction("Crud");

    }

    public IActionResult DeleteArea(int id)
    {
        var area = _context.Areas.Find(id);

        if (area == null)
        {
            return NotFound();
        }
        
        _context.Areas.Remove(area);
        _context.SaveChanges();

        return RedirectToAction("Crud");
    }

    #endregion
    
    #region sections
    
    public IActionResult Sections()
    {
        var locations = _context.Locations.Include(x => x.Sections).ToList();
        ViewBag.Locations = locations;
        
        return View(_context.Sections.ToList());
    }

    [HttpPost]
    public IActionResult CreateSection(Section model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        _context.Sections.Add(model);
        _context.SaveChanges();
        
        return RedirectToAction("Crud");
    }
    
    public IActionResult EditSection(int id)
    {
        var section = _context.Sections.Find(id);

        if(section == null)
        {
            return NotFound();
        }

        return View(section);
    }

    [HttpPost]
    public IActionResult EditSection(Section model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _context.Sections.Update(model);
        _context.SaveChanges();

        return RedirectToAction("Crud");

    }

    public IActionResult DeleteSection(int id)
    {
        var section = _context.Sections.Find(id);

        if (section == null)
        {
            return NotFound();
        }
        
        _context.Sections.Remove(section);
        _context.SaveChanges();

        return RedirectToAction("Crud");
    }
    
    #endregion
    
    #region locations
    
    public IActionResult Locations()
    {
        return View(_context.Locations.ToList());
    }

    [HttpPost]
    public IActionResult CreateLocation(Location model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        _context.Locations.Add(model);
        _context.SaveChanges();
        
        return RedirectToAction("Crud");
    }
    
    public IActionResult EditLocation(int id)
    {
        var location = _context.Locations.Find(id);

        if(location == null)
        {
            return NotFound();
        }

        return View(location);
    }

    [HttpPost]
    public IActionResult EditLocation(Location model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _context.Locations.Update(model);
        _context.SaveChanges();

        return RedirectToAction("Crud");

    }

    public IActionResult DeleteLocation(int id)
    {
        var location = _context.Locations.Find(id);

        if (location == null)
        {
            return NotFound();
        }
        
        _context.Locations.Remove(location);
        _context.SaveChanges();

        return RedirectToAction("Crud");
    }
    
    #endregion

    [Route("/gurud")]
    public IActionResult Crud()
    {
        var locations = _context.Locations
            .Include(x => x.Sections)
            .ThenInclude(x => x.Areas)
            .ToList();
        
        var listLocations = _context.Locations.Include(x => x.Sections).ToList();
        ViewBag.ListLocations = listLocations;
        
        var sections = _context.Sections.Include(x => x.Areas).ToList();
        ViewBag.Sections = sections;
        
        var areas = _context.Areas.ToList();
        ViewBag.Areas = areas;
        
        ViewBag.Locations = locations;
        return View();
    }
}