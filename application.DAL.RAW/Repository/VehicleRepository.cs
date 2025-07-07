using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application.DAL.RAW.Entities;
using application.DAL.RAW.Repository.Interfaces;

namespace application.DAL.RAW.Repository
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(SqlConnection connection) : base(connection) { }
        protected override string GetTableName() => "Vehicle";
        protected override string GetQuery() => $"SELECT * FROM dbo.{GetTableName()}";
        protected override string GetIdColumnName() => "id"; // Assumed identity, but still primary key

        protected override Vehicle MapFromReader(SqlDataReader reader)
        {
            return new Vehicle
            {
                Id = (int)reader["id"],
                VehicleName = reader["vehicleName"].ToString()
            };
        }

        protected override void AddParameters(SqlCommand command, Vehicle vehicle)
        {
            // For IDENTITY columns, you typically don't set the ID on INSERT.
            // But for UPDATE/DELETE, it's needed.
            command.Parameters.AddWithValue("@id", vehicle.Id);
            command.Parameters.AddWithValue("@vehicleName", vehicle.VehicleName);
        }

        public override void Add(Vehicle vehicle)
        {
            // Note: 'id' is IDENTITY(1,1), so we typically don't include it in INSERT.
            string query = $"INSERT INTO dbo.Vehicle (vehicleName) VALUES (@vehicleName); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@vehicleName", vehicle.VehicleName);
                // If you need the new ID:
                // vehicle.Id = Convert.ToInt32(command.ExecuteScalar());
                command.ExecuteNonQuery();
            }
        }

        public override void Update(Vehicle vehicle)
        {
            string query = $"UPDATE dbo.Vehicle SET vehicleName = @vehicleName WHERE id = @id";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                AddParameters(command, vehicle);
                command.ExecuteNonQuery();
            }
        }
    }
}
