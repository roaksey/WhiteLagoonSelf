using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        public IActionResult Create()
        {
            var amenity = new AmenityVM
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            };
            return View(amenity);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj) {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity added successfully.";
                return RedirectToAction("Index");
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(obj);
        }

        public IActionResult Update(int amenityId)
        {
            var amenityVm = new AmenityVM
            {
                Amenity = _unitOfWork.Amenity.Get(x => x.Id == amenityId, includeProperties: "Villa"),
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            };
            if(amenityVm.Amenity is null)
            {
                TempData["error"] = "Could not find amneity";
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVm);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(obj);
        }
        public IActionResult Delete(int amenityId)
        {
            AmenityVM model = new()
            {
                Amenity = _unitOfWork.Amenity.Get(x => x.Id == amenityId, includeProperties: "Villa"),
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            };
            if (model.Amenity is null)
            {
                TempData["error"] = "Could not find Amenity.";
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM model)
        {
            Amenity? objFromDb = _unitOfWork.Amenity.Get(u => u.Id == model.Amenity.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Amenity could not be deleted.";
            return View();
        }
    }
}
