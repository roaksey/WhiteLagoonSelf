﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhiteLagoon.Application.Common.Infterfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        [Authorize]
        public IActionResult FinalizeBooking(int villaId,DateOnly checkInDate,int nights)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = _unitOfWork.User.Get(u=>u.Id == userId);
            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.Get(u => u.Id == villaId, includeProperties: "VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Name = user.Name,
                Email = user.Email
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);
        }
    }
}
