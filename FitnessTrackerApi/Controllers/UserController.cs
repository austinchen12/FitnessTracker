using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using FitnessTrackerApi.Models;
using FitnessTrackerApi.Data;

namespace FitnessTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FitnessLogDbContext _fitnessLogDbContext;

        public UserController(FitnessLogDbContext fitnessLogContext)
        {
            _fitnessLogDbContext = fitnessLogContext;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_fitnessLogDbContext.Users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            User user = _fitnessLogDbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound($"Could not find user with Id = {id}");
            }

            return Ok(user);
        }

        //POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            _fitnessLogDbContext.Add(newUser);

            _fitnessLogDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, "User successfully created.");
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User newUser)
        {
            User user = _fitnessLogDbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound($"Could not find user with Id = {id}");
            }
            user.Username = newUser.Username;
            user.Password = newUser.Password;
            user.CurrentlySubscribed = newUser.CurrentlySubscribed;

            _fitnessLogDbContext.SaveChanges();
            return Ok("User successfully updated.");
        }

        // TODO: REMOVE
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEntry(int id)
        {
            User user = _fitnessLogDbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound($"Could not find user with Id = {id}");
            }
            _fitnessLogDbContext.Remove(user);

            _fitnessLogDbContext.SaveChanges();
            return Ok("User successfully deleted.");
        }
    }
}
