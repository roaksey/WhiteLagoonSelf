using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Application.Common.Infterfaces
{
    public interface IUnitOfWork
    {
        public IVillaRepository Villa { get; }
        public IVillaNumberRepository VillaNumber { get; }
        public IAmenityRepository Amenity { get; }
        public IBookingRepository Booking { get; }
        public IApplicationUserRepository User { get; }
        void Save();    
    }
}
