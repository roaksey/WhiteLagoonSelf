using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homwVm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties:"VillaAmenity"),
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                Nights = 1
            };
            return View(homwVm);
        }
        [HttpPost]
        public IActionResult Index(HomeVM homeVm)
        {
            homeVm.VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
            foreach (var villa in homeVm.VillaList)
            {
                if(villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            return View(homeVm);
        }
        public IActionResult GetVillasByDate(int nights,DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
            foreach (var villa in villaList)
            {
                if(villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }                
            }
            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                VillaList = villaList,
                Nights = nights
            };
            return PartialView("_VillaList", homeVM);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Error()
        {
            return View();
        }
    }
}