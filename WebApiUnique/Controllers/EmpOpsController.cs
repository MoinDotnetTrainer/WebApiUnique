using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiUnique.Models;

namespace WebApiUnique.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpOpsController : ControllerBase
    {
        //API --< ControllerBase --> APi status codes
        // MVC --> Controller --> In mVC Views


        public Appdb _db;
        public readonly ILogger<EmpOpsController> _logger;
        public EmpOpsController(ILogger<EmpOpsController> logger)
        {
            _db = new Appdb();
               _logger = logger;
        }

        // get EMplyees
        [HttpGet("GetEmp")]
        public IActionResult GetEMp()
        {
            try
            {
                var res = _db.Employees.AsNoTracking().ToList();
                // performce 
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Failed to Load the data",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        // LINQ is very imp

        [HttpPost("AddEmp")]
        public IActionResult CreateEmp(Emp data)
        {
            try
            {
                bool existinguser = _db.Employees.Any(x => x.EMail == data.EMail);
                if (existinguser)
                {
                    return Conflict(new { msg = "Email already Exists" });
                    // 409 
                }

                _db.Employees.Add(data);
                _db.SaveChanges();
                return CreatedAtAction(nameof(GetEMp), new { id = data.Id }, data);
                // inserted data  201
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Failed to Insert");
                return StatusCode(500, new { msg = "Internal server error" });
            }
        }


        [HttpGet("GetEMpByID/{Id}")]
        public IActionResult GetEmpByID(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(new { msg = "Invalid User ID" });
            }

            try
            {
                var data = _db.Employees.FirstOrDefault(x => x.Id == Id);
                if (data == null)
                {
                    return NotFound(new { msg = "data Not Found" });
                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                 _logger.LogError(ex, "Failed to Get");
                return StatusCode(500, new { msg = "Internal server error" });

            }
        }





        [HttpPut("UpdateEmp")]
        public IActionResult UpdateEmp(int Id, Emp data)
        {
            if (Id <= 0)
            {
                return BadRequest(new { msg = "Invalid User ID" });
            }

            if (Id != data.Id)
            {
                return BadRequest(new { msg = "ID mismatched" });
            }

            try
            {
                // update ops
                // astracking & asnoTracking
                var res = _db.Employees.FirstOrDefault(x => x.Id == Id);
                // // tracking help
                // here ef core tracks the data
                res.EmpName = data.EmpName;
                res.EMail = data.EMail;
                res.Dept = data.Dept;
               // _db.Employees.Update(data);
                _db.SaveChanges();
                  return CreatedAtAction(nameof(GetEMp), new { id = data.Id }, data);
               // return NoContent();
            }
            catch (Exception ex)
            {

                  _logger.LogError(ex, "Failed to Get");
                return StatusCode(500, new { msg = "Internal server error" });

            }
        }

        [HttpDelete("DeleteEmp")]
        public IActionResult DeleteEmp(int Id)
        {

            if (Id <= 0)
            {
                return BadRequest(new { msg = "Invalid User ID" });
            }
            try
            {
                var res = _db.Employees.FirstOrDefault(x => x.Id == Id);
                if (res != null)
                {
                    _db.Employees.Remove(res);
                    _db.SaveChanges();
                }
                return NoContent();

            }
            catch (Exception ex)
            {

                   _logger.LogError(ex, "Failed to Delete");
                return StatusCode(500, new { msg = "Internal server error" });

            }
        }
    }
}
