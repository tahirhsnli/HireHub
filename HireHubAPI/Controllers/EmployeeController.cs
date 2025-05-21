using HireHub.Models;
using HireHubAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HireHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(employees);
        }
        [HttpPost]
        public IActionResult Create([FromBody] Employee emp)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _employeeService.Create(emp);
            return Ok();
        }
    }

}
