﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Infterfaces
{
    public interface IBookingRepository:IRepository<Booking>
    {
        void Update(Booking booking);
        void UpdateStatus(int bookingId, string bookingStatus,int villaNumber);
        void UpdateStripePaymentId(int bookingId,string sessionId,string paymentIntentId);
    }
}
