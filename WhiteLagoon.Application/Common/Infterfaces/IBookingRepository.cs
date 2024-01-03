using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Infterfaces
{
    public interface IBookingRepository:IRepository<Booking>
    {
        void update(Booking booking);
    }
}
