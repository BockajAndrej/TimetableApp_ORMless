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
    // Concrete Repository for Employee
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(SqlConnection connection) : base(connection) { }

        protected override string GetTableName() => "Employee";
        protected override string GetQuery() => $"SELECT * FROM dbo.{GetTableName()}";
        protected override string GetIdColumnName() => "id";

        protected override Employee MapFromReader(SqlDataReader reader)
        {
            return new Employee
            {
                Id = reader["id"].ToString(),
                FirstName = reader["firstName"].ToString(),
                LastName = reader["lastName"].ToString(),
                BirthNumber = reader["birthNumber"].ToString(),
                BirthDayDateTime = (DateTime)reader["birthDay"]
            };
        }

        public override IEnumerable<Employee> GetByName(string name)
        {
            List<Employee> entities = new List<Employee>();
            string query = GetQuery();

            if (!string.IsNullOrEmpty(name))
            {
                query += " WHERE firstName LIKE @name";
                query += " OR lastName LIKE @name";
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

        protected override void AddParameters(SqlCommand command, Employee employee)
        {
            command.Parameters.AddWithValue("@id", employee.Id);
            command.Parameters.AddWithValue("@firstName", employee.FirstName);
            command.Parameters.AddWithValue("@lastName", employee.LastName);
            command.Parameters.AddWithValue("@birthNumber", employee.BirthNumber);
            command.Parameters.AddWithValue("@birthDay", employee.BirthDay);
        }

        public override void Add(Employee employee)
        {
            string query = $"INSERT INTO dbo.Employee (id, firstName, lastName, birthNumber, birthDay) " +
                           $"VALUES (@id, @firstName, @lastName, @birthNumber, @birthDay)";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                AddParameters(command, employee);
                command.ExecuteNonQuery();
            }
        }

        public override void Update(Employee employee)
        {
            string query = $"UPDATE dbo.Employee SET firstName = @firstName, lastName = @lastName, " +
                           $"birthNumber = @birthNumber, birthDay = @birthDay WHERE id = @id";
            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                AddParameters(command, employee);
                command.ExecuteNonQuery();
            }
        }
    }
}
