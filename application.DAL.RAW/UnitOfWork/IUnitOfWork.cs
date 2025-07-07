using application.DAL.RAW.Repository;
using application.DAL.RAW.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.DAL.RAW.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employees { get; }
        ICityRepository Cities { get; }
        IVehicleRepository Vehicles { get; }
        ICPRepository CPs { get; }
        ITransportRepository Transports { get; }

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        // Represents saving changes to the database
        int Complete();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        void RollbackTransaction();
    }
}
