using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Views.Home
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(x => x.Villa).ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> list = _db.Villas.Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.Name
            //});
            //ViewData["VillaList"] = list; 
            VillaNumberVM vm = new()
            {
                VillaList = _db.Villas.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
            };
            //ViewBag.VillaList = list;    
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
            //ModelState.Remove("Villa");
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
