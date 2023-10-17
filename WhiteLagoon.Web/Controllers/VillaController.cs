using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;    
        }
        public IActionResult Index()
        {
            var villaList = _db.Villas.ToList();
            return View(villaList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if(obj.Name == obj.Description) {
                ModelState.AddModelError("name", "Name and Description should not be same");
            } 
            if (ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);
            if(obj == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
