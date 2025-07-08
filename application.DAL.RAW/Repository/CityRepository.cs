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
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(SqlConnection connection) : base(connection) { }

        protected override string GetTableName() => "City";

        protected override string GetIdColumnName() => "id";
        protected override string GetAssignTable() => "C";
        protected override string GetQuery() => $"SELECT * FROM dbo.{GetTableName()} AS {GetAssignTable()}";

        protected override City MapFromReader(SqlDataReader reader)
        {
            return new City
            {
                Id = (int)reader["id"],
                Latitude = (decimal)reader["latitude"],
                Longitude = (decimal)reader["longitude"],
                CityName = reader["cityName"].ToString(),
                StateName = reader["stateName"].ToString()
            };
        }

        protected override void AddParameters(SqlCommand command, City city)
        {
            command.Parameters.AddWithValue("@id", city.Id);
            command.Parameters.AddWithValue("@latitude", city.Latitude);
            command.Parameters.AddWithValue("@longitude", city.Longitude);
            command.Parameters.AddWithValue("@cityName", city.CityName);
            command.Parameters.AddWithValue("@stateName", city.StateName);
        }

        public IEnumerable<City> GetByName(string name)
        {
            List<City> entities = new List<City>();
            string query = GetQuery();

            if (!string.IsNullOrEmpty(name))
            {
                query += " WHERE cityName LIKE @name";
                query += " OR id LIKE @name";
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

        public override void Add(City city)
        {
            string query = $"INSERT INTO dbo.City (id, latitude, longitude, cityName, stateName) " +
                           $"VALUES (@id, @latitude, @longitude, @cityName, @stateName)";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                AddParameters(command, city);
                command.ExecuteNonQuery();
            }
        }

        public override void Update(City city)
        {
            string query = $"UPDATE dbo.City SET latitude = @latitude, longitude = @longitude, " +
                           $"cityName = @cityName, stateName = @stateName WHERE id = @id";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                AddParameters(command, city);
                command.ExecuteNonQuery();
            }
        }
    }
}
