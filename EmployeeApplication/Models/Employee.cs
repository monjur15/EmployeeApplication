namespace EmployeeApplication.Models
{
    public class Employee
    {
        public static int Count { get; internal set; }
        public int employeeId { get; set; }
        public string? employeeName { get; set; }
        public int employeeCode { get; set; }
        public int employeeSalary { get; set; }
        public int supervisorId { get; set; }
        
    }
}
