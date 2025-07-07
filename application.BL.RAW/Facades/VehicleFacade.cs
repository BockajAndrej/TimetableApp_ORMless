using application.DAL.RAW.Entities;
using application.DAL.RAW.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.BL.RAW.Facades
{
    public class VehicleFacade(IUnitOfWork uow) : BaseFacade<Vehicle, int>(uow)
    {
    }
}
