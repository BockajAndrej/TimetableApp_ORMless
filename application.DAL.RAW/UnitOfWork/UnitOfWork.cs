using application.DAL.RAW.Entities;
using application.DAL.RAW.Repository;
using application.DAL.RAW.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace application.DAL.RAW.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction? _transaction;
        private bool _disposed = false;

        private readonly Dictionary<Type, object> _repositories;

        public IEmployeeRepository Employees { get; private set; }
        public ICityRepository Cities { get; private set; }
        public IVehicleRepository Vehicles { get; private set; }
        public ICPRepository CPs { get; private set; }
        public ITransportRepository Transports { get; private set; }

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _repositories = new Dictionary<Type, object>();

            // Init repos
            Employees = new EmployeeRepository(_connection);
            Cities = new CityRepository(_connection);
            Vehicles = new VehicleRepository(_connection);
            CPs = new CpRepository(_connection);
            Transports = new TransportRepository(_connection);

            // Adds repos into dictionary for generic access
            _repositories.Add(typeof(Employee), Employees);
            _repositories.Add(typeof(City), Cities);
            _repositories.Add(typeof(Vehicle), Vehicles);
            _repositories.Add(typeof(Cp), CPs);
            _repositories.Add(typeof(Transport), Transports);
        }

        // Generic method for repos
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            Console.WriteLine($"Type is {typeof(TEntity)}");
            if (_repositories.TryGetValue(typeof(TEntity), out object? repository))
            {
                return (IRepository<TEntity>)repository;
            }
            throw new InvalidOperationException($"Repository for type {typeof(TEntity).Name} not found.");
        }

        public int Complete()
        {
            if (_transaction == null)
                return 0;
            try
            {
                _transaction.Commit();
                return 1;
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already active. Complete or rollback the current transaction first.");
            }
            _transaction = _connection.BeginTransaction(isolationLevel);
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
                return;

            try
            {
                _transaction.Rollback();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during transaction rollback: {ex.Message}");
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_transaction != null)
                {
                    try
                    {
                        _transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error rolling back transaction during dispose: {ex.Message}");
                    }
                    finally
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                }

                if (_connection != null)
                {
                    if (_connection.State == ConnectionState.Open)
                    {
                        _connection.Close();
                    }
                    _connection.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
