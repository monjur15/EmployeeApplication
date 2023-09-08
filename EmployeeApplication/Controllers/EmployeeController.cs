using EmployeeApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _dbContext;
        public EmployeeController(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if(_dbContext.Employees==null)
            {
                return NotFound();
            }
            return await _dbContext.Employees.ToListAsync();
        }  
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int employeeID)
        {
            if(_dbContext.Employees==null)
            {
                return NotFound();
            }
            var employee = await _dbContext.Employees.FindAsync(employeeID);
            if(employee==null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new {id=employee.employeeId}, employee);
        }
    }
}
