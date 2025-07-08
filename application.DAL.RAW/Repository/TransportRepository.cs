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
    public class TransportRepository : BaseRepository<Transport>, ITransportRepository
    {
        public TransportRepository(SqlConnection connection) : base(connection) { }

        protected override string GetTableName() => "Transport";
        protected override string GetAssignTable() => "T";
        protected override string GetQuery() => $"SELECT * FROM dbo.{GetTableName()} AS {GetAssignTable()}";
        protected override string GetIdColumnName() => "id"; // Assumed identity

        protected override Transport MapFromReader(SqlDataReader reader)
        {
            return new Transport
            {
                Id = (int)reader["id"],
                IdCp = (int)reader["id_cp"],
                IdVehicle = (int)reader["id_vehicle"]
            };
        }

        protected override void AddParameters(SqlCommand command, Transport transport)
        {
            command.Parameters.AddWithValue("@id", transport.Id); // For update/delete
            command.Parameters.AddWithValue("@id_cp", transport.IdCp);
            command.Parameters.AddWithValue("@id_vehicle", transport.IdVehicle);
        }

        public override void Add(Transport transport)
        {
            string query = $"INSERT INTO dbo.Transport (id_cp, id_vehicle) VALUES (@id_cp, @id_vehicle); SELECT SCOPE_IDENTITY();";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@id_cp", transport.IdCp);
                command.Parameters.AddWithValue("@id_vehicle", transport.IdVehicle);
                command.ExecuteNonQuery();
            }
        }

        public override void Update(Transport transport)
        {
            string query = $"UPDATE dbo.Transport SET id_cp = @id_cp, id_vehicle = @id_vehicle WHERE id = @id";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                AddParameters(command, transport); // Uses the base helper which includes @id
                command.ExecuteNonQuery();
            }
        }
    }
}
