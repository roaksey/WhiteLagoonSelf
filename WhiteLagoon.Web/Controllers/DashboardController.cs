using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1?12:DateTime.Now.Month -1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year,DateTime.Now.Month, 1);
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u=>u.Status == SD.StatusPending ||  u.Status == SD.StatusCancelled);
            var countByCurrentMonth = totalBookings.Count(u=>u.BookingDate >= currentMonthStartDate && u.BookingDate <= DateTime.Now);
            var countByPreviousMonth = totalBookings.Count(u=>u.BookingDate == previousMonthStartDate && u.BookingDate <= DateTime.Now);

            return Json(GetRadialChartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth));
        }
        public async Task<IActionResult> GetRevenueChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusPending || u.Status == SD.StatusCancelled);
            var totalRevenue = Convert.ToInt32(totalBookings.Sum(x => x.TotalCost));
            var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= currentMonthStartDate && u.BookingDate <= DateTime.Now).Sum(x=>x.TotalCost);
            var countByPreviousMonth = totalBookings.Where(u => u.BookingDate == previousMonthStartDate && u.BookingDate <= DateTime.Now).Sum(x=>x.TotalCost);

            return Json(GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth));
        }
        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            var totalUsers = _unitOfWork.User.GetAll();
            var countByCurrentMonth = totalUsers.Count(u => u.CreatedAt >= currentMonthStartDate && u.CreatedAt <= DateTime.Now);
            var countByPreviousMonth = totalUsers.Count(u => u.CreatedAt == previousMonthStartDate && u.CreatedAt <= DateTime.Now);

           
            return Json(GetRadialChartDataModel(totalUsers.Count(),countByCurrentMonth,countByPreviousMonth));

        }


        public async Task<IActionResult> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) && (u.Status != SD.StatusPending || u.Status != SD.StatusCancelled));
            var customerWithOneBooking = totalBookings.GroupBy(b=>b.UserId).Where(c=>c.Count() == 1).Select(x=>x.Key).ToList();
            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartVm pieChartVm = new()
            {
                Labels = new string[] { "New Customer Bookings", "Returning Customer Bookings" },
                Series = new decimal[] { bookingsByNewCustomer, bookingsByReturningCustomer }
            };
            return Json(pieChartVm);
        } 

        private static RadialBarChartVM GetRadialChartDataModel(int totalCount,double currentMonthCount,double prevMonthCount)
        {
            RadialBarChartVM radialBarChartVM = new();

            int increaseDecreaseRatio = 100;

            if (prevMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
            }

            radialBarChartVM.TotalCount =   totalCount;
            radialBarChartVM.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
            radialBarChartVM.HasIncreased = currentMonthCount > prevMonthCount;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };
            return radialBarChartVM;
        }
    }
}
