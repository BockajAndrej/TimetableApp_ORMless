using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application.DAL.RAW.Entities;
using application.DAL.RAW.UnitOfWork;

namespace application.BL.RAW.Facades
{
    public abstract class BaseFacade<TEntity, TIdValue> : IFacade<TEntity, TIdValue>, IDisposable
        where TEntity : class, IEntity<TIdValue>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseFacade(IUnitOfWork ofw)
        {
            _unitOfWork = ofw ?? throw new ArgumentNullException("UnitOfWork was not be defined");
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

        public async Task<TEntity> SaveAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var repository = _unitOfWork.GetRepository<TEntity>();

            var existingEntity = repository.GetById(entity.Id);

            if (existingEntity == null)
                repository.Add(entity);
            else
                repository.Update(entity);

            TEntity result = repository.GetById(entity.Id);

            _unitOfWork.Complete();
            return await Task.FromResult(result);
        }

        public async Task<TEntity> DeleteAsync(TIdValue entityId)
        {
            if (entityId == null) throw new ArgumentNullException(nameof(entityId));

            var result = await GetByIdAsync(entityId);
            if (result == null) throw new KeyNotFoundException($"Entity with ID {entityId} not found.");

            var repository = _unitOfWork.GetRepository<TEntity>();
            repository.Delete(entityId);
            _unitOfWork.Complete();
            return await Task.FromResult(result);
        }

        public async Task<TEntity> GetByIdAsync(TIdValue entityId)
        {
            if (entityId == null) throw new ArgumentNullException(nameof(entityId));

            var repository = _unitOfWork.GetRepository<TEntity>();
            var entity = repository.GetById(entityId);
            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var repository = _unitOfWork.GetRepository<TEntity>();
            var entities = repository.GetAll();
            return await Task.FromResult(entities);
        }

        public async Task<IEnumerable<TEntity>> GetByNameAsync(string name)
        {
            if (name == String.Empty) throw new ArgumentNullException(nameof(name));

            var repository = _unitOfWork.GetRepository<TEntity>();
            var entity = repository.GetByName(name);
            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<TEntity>> GetByFilterAsync(List<Employee> emps, List<City> cities, List<Vehicle> vehicles, List<Cp> cps)
        {
            var repository = _unitOfWork.GetRepository<TEntity>();
            var entity = repository.GetByFilters(emps, cities, vehicles, cps);
            return await Task.FromResult(entity);
        }
    }
}
