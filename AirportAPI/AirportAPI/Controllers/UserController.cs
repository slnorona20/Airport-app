using AirportAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserController
    {
        protected IAirportDatabase AirportDatabase;

        public UserController(IAirportDatabase database)
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
                return BadRequest(exc.Message);
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
                return BadRequest(exc.Message);
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
                return BadRequest(exc.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int userId)
        {
            try
            {
                Task<int> userTask = Task.Run(() =>
                {
                    return AirportDatabase.DeleteUser(userId);
                });

                int deletedUserId = await userTask;
                return Ok(deletedUserId);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}
