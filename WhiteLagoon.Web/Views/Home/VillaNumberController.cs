using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
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
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumExist = _db.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
            //ModelState.Remove("Villa");
            if (ModelState.IsValid && !roomNumExist)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumExist)
            {
                TempData["error"] = "Villa Number " + obj.VillaNumber.Villa_Number + " already exists.";
            }
            obj.VillaList = _db.Villas.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View(obj);
        }

        public IActionResult Update(int villaNumId)
        {
            VillaNumberVM model = new() {
                VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumId),
                VillaList = _db.Villas.Select(x=> new SelectListItem { Value = x.Id.ToString(), Text = x.Name })   
            };
            if (model.VillaNumber is null)
            {
                TempData["error"] = "Could not find villa Number";
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM model)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(model.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number updated successfully";
                return RedirectToAction(nameof(Index));
            }
            model.VillaList = _db.Villas.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View(model);
        }
        public IActionResult Delete(int villaNumId)
        {
            VillaNumberVM model = new()
            {
                VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumId),
                VillaList = _db.Villas.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            };
            if (model.VillaNumber is null)
            {
                TempData["error"] = "Could not find villa Number.";
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM model)
        {
            VillaNumber? objFromDb = _db.VillaNumbers
                 .FirstOrDefault(u => u.Villa_Number == model.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
