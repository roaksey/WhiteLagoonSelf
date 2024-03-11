using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var villaList = _villaService.GetVillas();
            return View(villaList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "Name and Description should not be same");
            }
            if (ModelState.IsValid)
            {
                _villaService.CreateVilla(obj);
                TempData["success"] = "Villa created successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _villaService.GetVillaById(villaId);
            if (obj == null)
            {
                TempData["error"] = "Could not find Villa";
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                _villaService.UpdateVilla(obj);
                TempData["success"] = "Villa updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Update failed.";
            return View(obj);
        }
        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaService.GetVillaById(villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            bool isSuccess = _villaService.DeleteVilla(obj.Id);
            if (isSuccess)
            {
                TempData["success"] = "Villa deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the villa.";
            }
            return View();
        }
    }
}
