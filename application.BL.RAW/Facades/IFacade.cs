using application.DAL.RAW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.BL.RAW.Facades
{
    public interface IFacade<TEntity, TIdValue> where TEntity : class, IEntity<TIdValue>
    {
        public Task<TEntity> SaveAsync(TEntity entity);
        public Task<TEntity> DeleteAsync(TIdValue entityId);
        public Task<TEntity> GetByIdAsync(TIdValue entityId);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<IEnumerable<TEntity>> GetByNameAsync(string name);
    }
}
