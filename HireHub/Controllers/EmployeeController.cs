using Bogus;
using HireHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace HireHub.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeService.Get();
            return View(employees);
        }
        [HttpGet("search")]
        public IActionResult Index(string searchTerm)
        {
            var employees = _employeeService.Get();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                employees = employees.Where(e =>
                    (e.FullName != null && e.FullName.ToLower().Contains(searchTerm)) ||
                    (e.Position != null && e.Position.ToLower().Contains(searchTerm)) ||
                    (e.Office != null && e.Office.ToLower().Contains(searchTerm))
                ).ToList();
            }

            ViewData["searchTerm"] = searchTerm;

            return View(employees);
        }
        [HttpGet("{id}")]
        public IActionResult Details (string id)
        {
            var employee = _employeeService.GetById(id);
            return View(employee);
        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            try
            {
                await _employeeService.CreateAsync(emp); // burada exception ola bilər
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Bura log və ya sadə error message üçün
                ModelState.AddModelError("", "Xəta baş verdi: " + ex.Message);
                return View(emp);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMultiple()
        {
            try
            {
                var faker = new Faker<Employee>()
                    .RuleFor(e => e.FullName, f => f.Name.FullName())
                    .RuleFor(e => e.Position, f => f.Name.JobTitle())
                    .RuleFor(e => e.Office, f => f.Address.City())
                    .RuleFor(e => e.Age, f => f.Random.Int(20, 65))
                    .RuleFor(e => e.StartDate, f => f.Date.Past(10)) // Son 10 yıl içinde
                    .RuleFor(e => e.Salary, f => f.Finance.Amount(30000, 120000));

                var employees = faker.Generate(100); // 100 sahte Employee

                foreach (var emp in employees)
                {
                    await _employeeService.CreateAsync(emp); // MongoDB'ye ekle
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Xəta baş verdi: " + ex.Message);
                return View(); // Hata sayfası
            }
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            await _employeeService.UpdateAsync(id, employee);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            _employeeService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
