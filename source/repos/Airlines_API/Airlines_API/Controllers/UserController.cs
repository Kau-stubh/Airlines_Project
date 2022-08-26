using Airlines_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airlines_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public AppDbContext _context { get; }
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDetails>> Get()
        {
            // var data= _context.Category.ToList();

            return Ok(_context.UserDetails.ToList());
        }


        [HttpGet("{id}")]
        public ActionResult<UserDetails> Get(int id)
        {
            var data = _context.UserDetails.FirstOrDefault(u => u.UserId == id);
            if (data == null)
            {
                return BadRequest("User has Not Registered ");
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("registration")]
        public ActionResult Post(UserDetails newuser)
        {
            var user_exist = _context.UserDetails.FirstOrDefault(u => u.Email == newuser.Email);
            if (user_exist == null)
            {
                _context.UserDetails.Add(newuser);
                _context.SaveChanges();

                return Ok("Registration Successful");
            }
            return BadRequest("Already exist");
                //return Ok();
      }
        [HttpPost]
        [Route("Userlogin")]
        public ActionResult UserLogin(UserLogin login)
        {

                var user = _context.UserDetails.SingleOrDefault(u => u.Email == login.Email && u.Password == login.Password);
                
                if (user == null)
                {
                return NotFound("Not found ");

                }
                else
                 {
                return Ok(user);

                 }


        }
        
        [HttpPost("flight/search")]
        public ActionResult<IEnumerable<SearchData>> SearchFlight([FromBody] SearchQuery query)

        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = db.FilteredFlights.FromSqlInterpolated($"exec dbo.SP_Search_Flight {query.Booking_Type}, {query.Depart_airport_Id}, {query.Arrival_airport_Id}, {query.Departure_Time}, {query.Arrival_Time}, {query.adults},{query.childs} , {query.infants}, {query.Class_Type}");
                if (result != null)
                {
                    return Ok(result);
                }
                
                 return StatusCode(StatusCodes.Status500InternalServerError, "Failed"); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }
        [HttpGet("flight/{id}")]
        public ActionResult<IEnumerable<Seat>> GetSeatsByFlightId(int id)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = db.GetSeatsByFId.FromSqlInterpolated($"exec dbo.SP_Get_Seats_By_FlightId {id}");
                if (result != null)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid id");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }


      [HttpPut("{id}")]
        public ActionResult Put(int id, UserDetails modifieduser)
        {
            var data = _context.UserDetails.FirstOrDefault(u =>u.UserId  == id);
            if (data == null)
                return BadRequest();
            else
            {
                data.Firstname = modifieduser.Firstname;
                data.Lastname = modifieduser.Lastname;
                data.Title = modifieduser.Title;
                data.DateofBirth = modifieduser.DateofBirth;
                data.Email = modifieduser.Email;
                data.Password = modifieduser.Password;
                data.Mobile = modifieduser.Mobile;
                _context.SaveChanges();

                return Ok();
            }

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var temp = _context.UserDetails.FirstOrDefault(u => u.UserId == id);
            _context.UserDetails.Remove(temp);
            _context.SaveChanges();
            return Ok();
        }
        
        [HttpGet]
        [Route("airports")]
        public ActionResult GetAirports()
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);

                }
                var result = _context.Airports.ToList();

                if (result != null)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed");
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
===================================================================================================================
using Airlines_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Airlines_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public AppDbContext db { get; set; }

        public UserController(AppDbContext _db)
        {
            db = _db;
        }

        //login user
        [HttpPost]
        [Route("login")]
        public ActionResult UserLogin(Login u)
        {
            try
            {

                var user = db.UserDetails.FirstOrDefault(x => x.Email == u.Email && x.Password == u.Password);

                if (!ModelState.IsValid)
                { 
                    return BadRequest(ModelState);
                }
                if (user == null)
                {
                    return BadRequest("Invalid Credentials");
                }
                return Ok("Valid");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());

            }
        }

        [HttpPost]
        [Route("changepassword")]
        public ActionResult ChangePassword([FromBody] ChangePasswordModel model)
        {

            try
            {
                User u = db.Users.FirstOrDefault(user => user.email == model.email && user.password == model.old_password);
                if (u == null)
                {
                    return BadRequest("Invalid User id");
                }
                var res = db.Database.ExecuteSqlInterpolated($"exec dbo.SP_Change_Password {u.user_id}, {model.new_password}");
                if (res != 0)
                {
                    return Ok("Password Updated Successfully");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed");


            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }






    }
}



