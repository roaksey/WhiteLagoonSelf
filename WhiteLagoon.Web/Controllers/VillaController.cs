using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaController(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaList = _unitOfWork.Villa.GetAll();
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
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Villa.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaId);
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
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Villa.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Update failed.";
            return View(obj);
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            var objFromDb = _unitOfWork.Villa.Get(x=>x.Id ==  obj.Id);
            if (objFromDb is not null)
            {
                TempData["success"] = "Villa deleted successfully.";
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Villa.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
    }
}
