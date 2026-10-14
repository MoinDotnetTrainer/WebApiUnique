using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApiUnique.Models;

namespace WebApiUnique.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpOpsSpController : ControllerBase
    {
        public Appdb _db;
        public EmpOpsSpController()
        {
            _db = new Appdb();
        }

        // get EMplyees
        [HttpGet("GetEmp")]
        public IActionResult GetEMp()
        {
            try
            {
                string Query = "Exec Sp_GetEmp";
                // run this proc from UI at backend
                var res = _db.Employees.FromSqlRaw(Query).ToList();
                // var res1 = _db.Employees
                //.FromSqlRaw(Query)
                //.Select(e => new Emp
                //{
                //    EmpName = e.EmpName,
                //    EMail = e.EMail,
                //    Dept = e.Dept
                //}).AsEnumerable()
                //.ToList();
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
                string Query = "Exec Sp_InsertEmp @EmpName,@Email,@Dept";
                List<SqlParameter> sqlpara = new List<SqlParameter>() {
                new SqlParameter{ ParameterName="@EmpName",Value=data.EmpName},
                new SqlParameter{ ParameterName="@Email",Value=data.EMail},
                new SqlParameter{ ParameterName="@Dept",Value=data.Dept}
                };

                _db.Database.ExecuteSqlRaw(Query, sqlpara);
                return CreatedAtAction(nameof(GetEMp), new { id = data.Id }, data);
                // inserted data  201
            }
            catch (Exception ex)
            {
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
                string Query = "Exec Sp_GetEmpByID @Id";
                List<SqlParameter> sqlpara = new List<SqlParameter>() {
                new SqlParameter{ ParameterName="@Id",Value=Id}
                };
                var res = _db.Employees.FromSqlRaw(Query, sqlpara.ToArray());
                return Ok(res);
            }
            catch (Exception ex)
            {

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
                string Query = "Exec Sp_UpdateEmp @EmpName,@Email,@Dept,@Id";
                List<SqlParameter> sqlpara = new List<SqlParameter>() {
                new SqlParameter{ ParameterName="@EmpName",Value=data.EmpName},
                new SqlParameter{ ParameterName="@Email",Value=data.EMail},
                new SqlParameter{ ParameterName="@Dept",Value=data.Dept},
                new SqlParameter{ ParameterName="@Id",Value=data.Id},
                };

                _db.Database.ExecuteSqlRaw(Query, sqlpara);
                return CreatedAtAction(nameof(GetEMp), new { id = data.Id }, data);
                // return NoContent();
            }
            catch (Exception ex)
            {

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
                string Query = "Exec Sp_DeleteEmp @Id";
                List<SqlParameter> sqlpara = new List<SqlParameter>() {
                new SqlParameter{ ParameterName="@Id",Value=Id}
                };

                _db.Database.ExecuteSqlRaw(Query, sqlpara);

                return NoContent();

            }
            catch (Exception ex)
            {

                return StatusCode(500, new { msg = "Internal server error" });

            }
        }
    }
}
