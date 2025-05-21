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

    public Employee Get(string id) =>
        _employees.Find(emp => emp.Id == id).FirstOrDefault();

    public void Create(Employee emp) =>
        _employees.InsertOne(emp);
    public async Task CreateAsync(Employee employee)
    {
        await _employees.InsertOneAsync(employee);
    }


    public void Update(string id, Employee empIn) =>
        _employees.ReplaceOne(emp => emp.Id == id, empIn);

    public void Remove(string id) =>
        _employees.DeleteOne(emp => emp.Id == id);
}
