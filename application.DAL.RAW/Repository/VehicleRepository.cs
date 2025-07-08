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
        public VehicleRepository(SqlConnection connection) : base(connection)
        {
        }

        protected override string GetTableName() => "Vehicle";
        protected override string GetIdColumnName() => "id";
        protected override string GetQuery() => $"SELECT DISTINCT V.* FROM dbo.{GetTableName()} AS V LEFT JOIN dbo.Transport AS T ON T.id_vehicle = V.id ";

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
            command.Parameters.AddWithValue("@id", vehicle.Id);
            command.Parameters.AddWithValue("@vehicleName", vehicle.VehicleName);
        }

        public override void Add(Vehicle vehicle)
        {
            string query = $"INSERT INTO dbo.Vehicle (vehicleName) VALUES (@vehicleName); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@vehicleName", vehicle.VehicleName);
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

        public virtual IEnumerable<Vehicle> GetByFilters(List<Employee> employees, List<City> cities, List<Vehicle> Vehicles,
            List<Cp> Cps)
        {
            List<Vehicle> items = new List<Vehicle>();
            string query = GetQuery();

            if (Cps.Count > 0)
                query += "WHERE ";

            foreach (var cp in Cps)
            {
                if (cp.Id == Cps[0].Id)
                    query += "( ";

                query += $"T.id_cp = @cp{cp.Id} ";

                if (cp.Id == Cps[^1].Id)
                    query += ") ";
                else
                    query += "OR ";
            }

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                foreach (var item in Cps)
                    command.Parameters.AddWithValue($"@cp{item.Id}", item.Id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(MapFromReader(reader));
                    }
                }
            }
            return items;
        }
    }
}
