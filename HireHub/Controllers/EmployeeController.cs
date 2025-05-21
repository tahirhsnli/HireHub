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
