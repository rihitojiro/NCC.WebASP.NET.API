using ASP.NETCore_API.Entities;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        [HttpGet("")]
        
        public IActionResult GetAllEmployee()
        {
            try
            {
                using (var db = new Startup())
                {

                    List<Employee> users = db.Employee.ToList(); 

                    if (users == null || users.Count == 0)
                    {
                        return NotFound();
                    }



                    return Ok(users);
                }
                


            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        /// <summary>
        /// API lấy ra thông tin 1 nhân viên
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        [HttpGet("{EmployeeID}")]
        public IActionResult GetEmployeeByID(int EmployeeID)
        {
            try
            {
                using (var db = new Startup())
                {
                    var EmployeeToGet = db.Employee.FirstOrDefault(e => e.EmployeeID == EmployeeID);
                    if (EmployeeToGet == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                    var employeeDto = new EmployeeDto
                    {

                        EmployeeCode = EmployeeToGet.EmployeeCode,
                        EmployeeName = EmployeeToGet.EmployeeName,
                        DateOfBirth = EmployeeToGet.DateOfBirth,
                        IdentityNumber = EmployeeToGet.IdentityNumber,
                        IdentityDate = EmployeeToGet.IdentityDate,
                        IdentityPlace = EmployeeToGet.IdentityPlace

                    };
                    return StatusCode(StatusCodes.Status200OK, employeeDto);
                }


            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("test")]
        public IActionResult AddProduct([FromBody] Employee Employees)
        {
            using (var db = new Startup())
            {
                db.Employee.Add(Employees);
                db.SaveChanges();

            }
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpPut("{EmployeeID}")]
        

        public IActionResult UpdateEmployee(int EmployeeID, [FromBody] Employee Employee)
        {
            try
            {
                using (var db = new Startup())
                {

                    var EmployeeToUpDate = db.Employee.FirstOrDefault(e => e.EmployeeID == EmployeeID);
                    if (EmployeeToUpDate == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                    EmployeeToUpDate.EmployeeCode = Employee.EmployeeCode;
                    EmployeeToUpDate.EmployeeName = Employee.EmployeeName;
                    EmployeeToUpDate.DateOfBirth = Employee.DateOfBirth;
                    EmployeeToUpDate.IdentityNumber = Employee.IdentityNumber;
                    EmployeeToUpDate.IdentityDate = Employee.IdentityDate;
                    EmployeeToUpDate.IdentityPlace = Employee.IdentityPlace;




                    db.SaveChanges();

                }
                
                    return Ok();
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }


        }

        [HttpDelete("{EmployeeID}")]
        

        public IActionResult DeleteEmployee(int EmployeeID)
        {
            try
            {
                using (var db = new Startup())
                {

                    var EmployeeToDelete = db.Employee.FirstOrDefault(e => e.EmployeeID == EmployeeID);
                    if (EmployeeToDelete == null)
                    {
                        return NotFound();
                    }




                    db.Employee.Remove(EmployeeToDelete);
                    db.SaveChanges();

                }
                
                    return Ok();
            }
            catch (Exception exception)
            {
                
                return BadRequest(exception.Message);
            }


            
        }
    }
}

