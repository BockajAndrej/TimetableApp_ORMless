using application.DAL.RAW.Entities;
using System.Collections.Generic;

namespace application.DAL.RAW.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);

        //Specific
        public IEnumerable<TEntity> GetByName(string name);
        public IEnumerable<TEntity> GetByFilters(List<Employee> EmployeeId, List<City> CityId, List<Vehicle> Vehicles, List<Cp> Cps);
    }
}
