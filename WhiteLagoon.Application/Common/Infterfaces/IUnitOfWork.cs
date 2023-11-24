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
    }
}
