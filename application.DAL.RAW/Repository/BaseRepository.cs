using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application.DAL.RAW.Entities;
using application.DAL.RAW.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace application.DAL.RAW.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly SqlConnection _connection;

        public BaseRepository(SqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        // Abstract methods to be implemented by concrete repositories
        protected abstract TEntity MapFromReader(SqlDataReader reader);
        protected abstract void AddParameters(SqlCommand command, TEntity entity);
        protected abstract string GetQuery();
        protected abstract string GetTableName();
        protected abstract string GetIdColumnName();

        public virtual TEntity GetById(object id)
        {
            TEntity entity = null;
            string query = $"{GetQuery()} WHERE {GetIdColumnName()} = @id";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        entity = MapFromReader(reader);
                    }
                }
            }
            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            List<TEntity> entities = new List<TEntity>();
            string query = GetQuery();
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entities.Add(MapFromReader(reader));
                    }
                }
            }
            return entities;
        }

        public virtual void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(object id)
        {
            string query = $"DELETE FROM dbo.{GetTableName()} WHERE {GetIdColumnName()} = @id";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public virtual IEnumerable<TEntity> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetByFilters(List<Employee> EmployeeId, List<City> CityId, List<Vehicle> Vehicles, List<Cp> Cps)
        {
            throw new NotImplementedException();
        }
    }
}
