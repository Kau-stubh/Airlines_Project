﻿using Airlines_API.Models;
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
    }
}
