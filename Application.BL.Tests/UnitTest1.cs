using application.BL.RAW.Facades;
using application.DAL.RAW.Entities;
using Microsoft.Data.SqlClient;
using Xunit.Abstractions;

namespace Application.BL.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Test1()
        {
            const string connectionString =
                @"Server=(localdb)\MSSQLLocalDB;Database=applicationDb;Trusted_Connection=True;";

            using (var facade = new BaseFacade(connectionString))
            {
                try
                {
                    _testOutputHelper.WriteLine("--- All Employees (using Facade) ---");
                    var employees = await facade.GetAllAsync<Employee>();
                    if (employees.Any())
                    {
                        foreach (var emp in employees)
                        {
                            _testOutputHelper.WriteLine($"ID: {emp.Id}, Name: {emp.FirstName} {emp.LastName}, BirthDay: {emp.BirthDay:yyyy-MM-dd}");
                        }
                    }
                    Employee newEmployee = new Employee
                    {
                        Id = "EMP999",
                        FirstName = "Jan",
                        LastName = "Novak",
                        BirthNumber = "123456/7890",
                        BirthDay = new DateTime(1990, 5, 15)
                    };
                    await facade.SaveAsync(newEmployee, newEmployee.Id);
                    _testOutputHelper.WriteLine($"Added employee: {newEmployee.FirstName} {newEmployee.LastName}");

                    employees = await facade.GetAllAsync<Employee>();
                    foreach (var emp in employees)
                    {
                        _testOutputHelper.WriteLine($"ID: {emp.Id}, Name: {emp.FirstName} {emp.LastName}, BirthDay: {emp.BirthDay:yyyy-MM-dd}");
                    }


                    _testOutputHelper.WriteLine("\n--- Employee by ID (EMP999) (using Facade) ---");
                    Employee specificEmployee = await facade.GetByIdAsync<Employee>("EMP999");
                    if (specificEmployee != null)
                    {
                        _testOutputHelper.WriteLine($"Found Employee: ID: {specificEmployee.Id}, Name: {specificEmployee.FirstName} {specificEmployee.LastName}");

                        specificEmployee.FirstName = "Jana";
                        await facade.SaveAsync(specificEmployee, specificEmployee.Id);
                        _testOutputHelper.WriteLine($"\nUpdated employee EMP999 to {specificEmployee.FirstName} {specificEmployee.LastName}");

                        specificEmployee = await facade.GetByIdAsync<Employee>("EMP999");
                        _testOutputHelper.WriteLine($"Verified: ID: {specificEmployee.Id}, Name: {specificEmployee.FirstName} {specificEmployee.LastName}");
                    }
                    else
                    {
                        _testOutputHelper.WriteLine("Employee with ID 'EMP999' not found.");
                    }

                    _testOutputHelper.WriteLine("\n--- Deleting Employee EMP999 (using Facade) ---");
                    await facade.DeleteAsync<Employee>("EMP999");
                    specificEmployee = await facade.GetByIdAsync<Employee>("EMP999");
                    if (specificEmployee == null)
                    {
                        _testOutputHelper.WriteLine("Employee EMP999 successfully deleted.");
                    }
                }
                catch (SqlException ex)
                {
                    _testOutputHelper.WriteLine($"Database Error: {ex.Message}");
                    _testOutputHelper.WriteLine($"SQL Error Code: {ex.ErrorCode}");
                }
                catch (Exception ex)
                {
                    _testOutputHelper.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }

        }
    }
}
