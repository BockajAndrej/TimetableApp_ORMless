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
    public class CpRepository : BaseRepository<Cp>, ICPRepository
    {
        public CpRepository(SqlConnection connection) : base(connection) { }

        protected override string GetQuery() =>
            "Select DISTINCT CP.*, E.firstName, E.lastName, C1.cityName as startCity, C2.cityName as endCity " +
            "From dbo.Cp AS CP LEFT JOIN dbo.Transport AS T ON T.id_cp = CP.id " +
            "LEFT JOIN dbo.Employee AS E ON E.id = CP.id_employee " +
            "LEFT JOIN dbo.City AS C1 ON C1.id = CP.id_startCity " +
            "LEFT JOIN dbo.City AS C2 ON C2.id = CP.id_endCity ";

        protected override string GetTableName() => "Cp";

        protected override string GetIdColumnName() => "id"; // Assumed identity

        protected override Cp MapFromReader(SqlDataReader reader)
        {
            return new Cp
            {
                Id = (int)reader["id"],
                IdEmployee = reader["id_employee"].ToString(),
                IdStartCity = (int)reader["id_startCity"],
                IdEndCity = (int)reader["id_endCity"],
                CreationDate = (DateTime)reader["creationDate"],
                StartTime = (DateTimeOffset)reader["startTime"],
                EndTime = (DateTimeOffset)reader["endTime"],
                CpState = reader["CpState"].ToString(),
                EmployeeName = $"{reader["firstName"]} {reader["lastName"]} ",
                StartCityName = reader["startCity"].ToString(),
                EndCityName = reader["endCity"].ToString()
            };
        }

        public override IEnumerable<Cp> GetByName(string name)
        {
            List<Cp> entities = new List<Cp>();
            string query = GetQuery();

            if (!string.IsNullOrEmpty(name))
            {
                query += " WHERE CP.id LIKE @name";
                query += " OR E.firstName LIKE @name";
                query += " OR C1.cityName LIKE @name";
                query += " OR C2.cityName LIKE @name";
                query += " OR CP.cpState LIKE @name";
            }

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    command.Parameters.AddWithValue("@name", "%" + name + "%");
                }

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
        protected override void AddParameters(SqlCommand command, Cp Cp)
        {
            command.Parameters.AddWithValue("@id", Cp.Id); // For update/delete
            command.Parameters.AddWithValue("@id_employee", Cp.IdEmployee);
            command.Parameters.AddWithValue("@id_startCity", Cp.IdStartCity);
            command.Parameters.AddWithValue("@id_endCity", Cp.IdEndCity);
            command.Parameters.AddWithValue("@creationDate", Cp.CreationDate);
            command.Parameters.AddWithValue("@startTime", Cp.StartTime);
            command.Parameters.AddWithValue("@endTime", Cp.EndTime);
            command.Parameters.AddWithValue("@CpState", Cp.CpState);
        }

        public virtual IEnumerable<Cp> GetByFilters(List<Employee> employees, List<City> cities, List<Vehicle> Vehicles)
        {
            List<Cp> travelOrders = new List<Cp>();
            bool inserterToQuery = false;
            string query = GetQuery();

            if (employees.Count > 0 || cities.Count > 0 || Vehicles.Count > 0)
                query += "WHERE ";

            foreach (var employee in employees)
            {
                if (employee.Id == employees[0].Id)
                    query += "( ";

                query += $"CP.id_employee = @employee{employee.Id} ";

                if (employee.Id == employees[^1].Id)
                    query += ") ";
                else
                    query += "OR ";
                inserterToQuery = true;
            }

            foreach (var vehicle in Vehicles)
            {
                if (vehicle.Id == Vehicles[0].Id)
                {
                    if (inserterToQuery)
                        query += "AND ";
                    query += "( ";
                }

                query += $"T.id_vehicle = @vehicle{vehicle.Id} ";

                if (vehicle.Id == Vehicles[^1].Id)
                    query += ") ";
                else
                    query += "OR ";
                inserterToQuery = true;
            }

            foreach (var city in cities)
            {
                if (city.Id == cities[0].Id)
                {
                    if (inserterToQuery)
                        query += "AND ";
                    query += "( ";
                }

                query += $"CP.id_startCity = @city{city.Id} OR CP.id_endCity = @city{city.Id} ";

                if (city.Id == cities[^1].Id)
                    query += ") ";
                else
                    query += "OR ";
                inserterToQuery = true;
            }

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                foreach (var item in employees)
                    command.Parameters.AddWithValue($"@employee{item.Id}", item.Id);
                foreach (var item in cities)
                    command.Parameters.AddWithValue($"@city{item.Id}", item.Id);
                foreach (var item in Vehicles)
                    command.Parameters.AddWithValue($"@vehicle{item.Id}", item.Id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        travelOrders.Add(MapFromReader(reader));
                    }
                }
            }
            return travelOrders;
        }

        public override void Add(Cp Cp)
        {
            string query = $"INSERT INTO dbo.Cp (id_employee, id_startCity, id_endCity, creationDate, startTime, endTime, CpState) " +
                           $"VALUES (@id_employee, @id_startCity, @id_endCity, @creationDate, @startTime, @endTime, @CpState); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@id_employee", Cp.IdEmployee);
                command.Parameters.AddWithValue("@id_startCity", Cp.IdStartCity);
                command.Parameters.AddWithValue("@id_endCity", Cp.IdEndCity);
                command.Parameters.AddWithValue("@creationDate", Cp.CreationDate);
                command.Parameters.AddWithValue("@startTime", Cp.StartTime);
                command.Parameters.AddWithValue("@endTime", Cp.EndTime);
                command.Parameters.AddWithValue("@CpState", Cp.CpState);

                command.ExecuteNonQuery();
            }
        }

        public override void Update(Cp Cp)
        {
            string query = $"UPDATE dbo.Cp SET id_employee = @id_employee, id_startCity = @id_startCity, " +
                           $"id_endCity = @id_endCity, creationDate = @creationDate, startTime = @startTime, " +
                           $"endTime = @endTime, CpState = @CpState WHERE id = @id";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                AddParameters(command, Cp); // Uses the base helper which includes @id
                command.ExecuteNonQuery();
            }
        }
    }
}
