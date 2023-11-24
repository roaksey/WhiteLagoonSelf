using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo;
        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;    
        }
        public IActionResult Index()
        {
            var villaList = _villaRepo.GetAll();
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
                TempData["success"] = "Villa created successfully";
                _villaRepo.Add(obj);
                _villaRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj =_villaRepo.Get(x => x.Id == villaId);
            if(obj == null)
            {
                TempData["error"] = "Could not find Villa";
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                TempData["success"] = "Villa updated successfully.";
                _villaRepo.Update(obj);
                _villaRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Update failed.";
            return View(obj);
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaRepo.Get(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var objFromDb = _villaRepo.Get(x=>x.Id ==  obj.Id);
            if (objFromDb is not null)
            {
                TempData["success"] = "Villa deleted successfully.";
                _villaRepo.Remove(objFromDb);
                _villaRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
