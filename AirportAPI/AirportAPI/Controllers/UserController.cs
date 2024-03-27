using AirportAPI.Classes;
using AirportAPI.Exceptions;
using AirportAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserController
    {
        protected IAirportDatabase AirportDatabase;

        public UserController(JsonAirportDatabase database)
        {
            AirportDatabase = database;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                Task<User> userTask = Task.Run(() =>
                {
                    return AirportDatabase.GetUser(id);
                });

                User result = await userTask;
                return result;
            }
            catch(Exception exc)
            {
                if(exc is ObjectNotFoundException)
                {
                    return NotFound(id);
                }

                return BadRequest("Error");
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            try
            {
                Task<User> userTask = Task.Run(() =>
                {
                    return AirportDatabase.AddUser(user);
                });

                User result = await userTask;
                return result;
            }
            catch (Exception exc)
            {
                return BadRequest("Error");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] User user)
        {
            try
            {
                Task<User> userTask = Task.Run(() =>
                {
                    return AirportDatabase.UpdateUser(user);
                });

                User result = await userTask;
                return result;
            }
            catch (Exception exc)
            {
                if (exc is ObjectNotFoundException)
                {
                    return NotFound(user);
                }

                return BadRequest("Error");
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Task<int> userTask = Task.Run(() =>
                {
                    return AirportDatabase.DeleteUser(id);
                });

                int deletedUserId = await userTask;
                return Ok(deletedUserId);
            }
            catch (Exception exc)
            {
                if (exc is ObjectNotFoundException)
                {
                    return NotFound(id);
                }

                return BadRequest("Error");
            }
        }
    }
}
