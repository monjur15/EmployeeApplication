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

        [HttpPut]

        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.employeeId)
            {
                return BadRequest();
            }
            _dbContext.Entry(employee).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!EmployeeAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            
            }
            return Ok();
        }

        private bool EmployeeAvailable(int id)
        {
            return(_dbContext.Employees?.Any(x=>x.employeeId== id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }
            var employee =await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }







        [HttpGet("third-highest-salary")]
        public async Task<ActionResult<Employee>> GetEmployeeWithThirdHighestSalary()
        {
            var employees = await _dbContext.Employees.ToListAsync();

            // Check if there are at least 3 employees
            if (employees.Count < 3)
            {
                return NotFound("There are not enough employees to find the third-highest salary.");
            }

            // Find the employee with the third-highest salary
            var thirdHighestSalaryEmployee = employees.OrderByDescending(e => e.employeeSalary).Skip(2).FirstOrDefault();

            if (thirdHighestSalaryEmployee == null)
            {
                return NotFound();
            }

            return thirdHighestSalaryEmployee;
        }

    }
}
