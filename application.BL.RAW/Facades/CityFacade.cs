using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application.DAL.RAW.Entities;
using application.DAL.RAW.UnitOfWork;

namespace application.BL.RAW.Facades
{
    public class CityFacade(IUnitOfWork uow) : BaseFacade<City, int>(uow)
    {
    }
}
