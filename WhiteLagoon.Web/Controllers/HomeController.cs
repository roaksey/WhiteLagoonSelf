using Microsoft.AspNetCore.Mvc;
using Syncfusion.Presentation;
using System.Diagnostics;
using System.IO;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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

        //public IActionResult Index(HomeVM homeVm)
        //{
        //    homeVm.VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
        //    foreach (var villa in homeVm.VillaList)
        //    {
        //        if(villa.Id % 2 == 0)
        //        {
        //            villa.IsAvailable = false;
        //        }
        //    }
        //    return View(homeVm);
        //}
        [HttpPost]
        public IActionResult GetVillasByDate(int nights,DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");
            var villaNumberList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u=>u.Status == SD.StatusApproved || u.Status == SD.StatusCheckedIn).ToList();
            foreach (var villa in villaList)
            {
                int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumberList, checkInDate, nights, bookedVillas);
                villa.IsAvailable = roomAvailable > 0 ? true : false;   
            }
            Thread.Sleep(1000);//To check spinner
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

        public IActionResult GeneratePPTExport(int id)
        {
            var villa = _unitOfWork.Villa.GetAll(includeProperties:"VillaAmenity").FirstOrDefault(u=>u.Id == id);
            if(villa is null)
            {
                return RedirectToAction(nameof(Error));
            }

            string basePath = _webHostEnvironment.WebRootPath;
            string filePaht = basePath + @"/exports/ExportVillaDetails.pptx";

            using IPresentation presentation = Presentation.Open(filePaht); ;

            ISlide slide = presentation.Slides[0];

            IShape? shape = slide.Shapes.FirstOrDefault(u=>u.ShapeName == "txtVillaName") as IShape;
            if(shape != null)
            {
                shape.TextBody.Text = villa.Name;
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaDescription") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Description;
            }


            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtOccupancy") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Max Occupancy : {0} adults", villa.Occupancy);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaSize") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("Villa Size: {0} sqft", villa.Sqft);
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtPricePerNight") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = string.Format("USD {0}/night", villa.Price.ToString("C", new System.Globalization.CultureInfo("en-US")));
            }

            MemoryStream stream = new MemoryStream();
            presentation.Save(stream);
            stream.Position = 0;
            return File(stream, "application/pptx", "villa.pptx");
        }
    }
}