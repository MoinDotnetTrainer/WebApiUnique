using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApiUnique.Models;

namespace WebApiUnique.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersOpsController : ControllerBase
    {
        // Controller --> View 
        // ControllerBase --> StatusCodes

        public Appdb _db;
        public UsersOpsController()
        {
            _db = new Appdb();
        }

        [HttpGet]
        public IActionResult GetUsers()
        {

            var res = _db.Users.ToList();
            return Ok(res);


            // 200 status code with data 
            // returbed succesfully
        }

        [HttpPost]
        public IActionResult InsertUsers(Users data)
        {
            bool isuserexists = _db.Users.Any(x => x.Email == data.Email);
            if (isuserexists)
            {
                return Conflict("Email already exists");
                //409
            }
            _db.Users.Add(data);
            _db.SaveChanges();  // already inserted

            //201
            return CreatedAtAction(nameof(GetUsers), new { id = data.Id }, data);
        }
    }
}
