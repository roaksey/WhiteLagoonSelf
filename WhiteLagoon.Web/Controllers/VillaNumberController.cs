using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Views.Home
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties:"Villa").ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> list = _unitOfWork.Villas.Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.Name
            //});
            //ViewData["VillaList"] = list; 
            VillaNumberVM vm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
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
            bool roomNumExist = _unitOfWork.VillaNumber.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
            //ModelState.Remove("Villa");
            if (ModelState.IsValid && !roomNumExist)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumExist)
            {
                TempData["error"] = "Villa Number " + obj.VillaNumber.Villa_Number + " already exists.";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View(obj);
        }

        public IActionResult Update(int villaNumId)
        {
            VillaNumberVM model = new() {
                VillaNumber = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumId),
                VillaList = _unitOfWork.Villa.GetAll().Select(x=> new SelectListItem { Value = x.Id.ToString(), Text = x.Name })   
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
                _unitOfWork.VillaNumber.Update(model.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Villa Number updated successfully";
                return RedirectToAction(nameof(Index));
            }
            model.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
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
                VillaNumber = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumId),
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
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
            VillaNumber? objFromDb = _unitOfWork.VillaNumber
                 .Get(u => u.Villa_Number == model.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
