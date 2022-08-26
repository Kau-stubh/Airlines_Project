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
    public class AdminController : Controller
    {

         public AppDbContext _context { get; }
         public AdminController(AppDbContext context)
         {
                _context = context;
          }

        [Route("Userlogin")]
        public ActionResult AdminLogin(UserLogin u)
        {

            var admin = _context.Admins.FirstOrDefault(a => a.Admin_Username == u.Email && a.Admin_password == u.Password);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (admin == null)
            {
                return BadRequest("Invalid Credentials");
            }
            return Ok("Login Successfull");

        }
        
        [HttpGet]
        [Route("flights")]
        public ActionResult GetFlights()
        {
            try
            {
                List<Flight> flights = _context.Flights.ToList();

                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("addflight")]
        public ActionResult AddFlight(FlightModel f)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (f.Departure_time > f.Arrival_time)
                {
                    return BadRequest("Departure time is later than Arrival Time");
                }

                var res = _context.Database.ExecuteSqlInterpolated($"exec dbo.SP_Add_Flight {f.FlightName},{f.Depart_airport_Id},{f.Arrival_airport_Id}, {f.Departure_time}, {f.Arrival_time}, {f.Economy_fare}, {f.Business_fare}");
                if(res != 0)
                {
                    return Ok("Flight Added Successfully");
                   
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        private ActionResult StatusCode(object status50)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("flight/{name}")]
        public ActionResult DeleteFlight(string name)
        {

            try
            {
                var result = _context.Database.ExecuteSqlInterpolated($"exec dbo.SP_Delete_Flight {name}");
                if (result != 0)
                {
                    return Ok("Flight Deleted Successfully");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
===========================================================================================================

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
    public class AdminController : Controller
    {

         public AppDbContext _context { get; }
         public AdminController(AppDbContext context)
         {
                _context = context;
          }


        //admin login
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] Login u)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var admin = _context.AdminLoginDetails.FirstOrDefault(x => x.Admin_Username == u.Email && x.Admin_password == u.Password);

                if (admin == null)
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





    }
}















    }
}
