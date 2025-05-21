using MongoDB.Driver;
using Microsoft.Extensions.Options;
using HireHub.Configuration;
using HireHub.Models;

public class EmployeeService
{
    private readonly IMongoCollection<Employee> _employees;

    public EmployeeService(IOptions<MongoDBSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        _employees = database.GetCollection<Employee>("Employees");
    }

    public List<Employee> Get() =>
        _employees.Find(emp => true).ToList();

    public Employee GetById(string id) =>
        _employees.Find(emp => emp.Id == id).FirstOrDefault();

    public void Create(Employee emp) =>
        _employees.InsertOne(emp);
    public async Task CreateAsync(Employee employee)
    {
        await _employees.InsertOneAsync(employee);
    }


    public async Task UpdateAsync(string id, Employee updatedEmp)
    {
        var updateDef = new List<UpdateDefinition<Employee>>();

        if (!string.IsNullOrWhiteSpace(updatedEmp.FullName))
            updateDef.Add(Builders<Employee>.Update.Set(e => e.FullName, updatedEmp.FullName));

        if (!string.IsNullOrWhiteSpace(updatedEmp.Position))
            updateDef.Add(Builders<Employee>.Update.Set(e => e.Position, updatedEmp.Position));

        if (!string.IsNullOrWhiteSpace(updatedEmp.Office))
            updateDef.Add(Builders<Employee>.Update.Set(e => e.Office, updatedEmp.Office));

        if (updatedEmp.Age > 0)
            updateDef.Add(Builders<Employee>.Update.Set(e => e.Age, updatedEmp.Age));

        if (updatedEmp.StartDate != default)
            updateDef.Add(Builders<Employee>.Update.Set(e => e.StartDate, updatedEmp.StartDate));

        if (updatedEmp.Salary > 0)
            updateDef.Add(Builders<Employee>.Update.Set(e => e.Salary, updatedEmp.Salary));

        if (updateDef.Count > 0)
        {
            var update = Builders<Employee>.Update.Combine(updateDef);
            await _employees.UpdateOneAsync(emp => emp.Id == id, update);
        }
    }


    public DeleteResult Remove(string id) =>
        _employees.DeleteOne(emp => emp.Id == id);
}
