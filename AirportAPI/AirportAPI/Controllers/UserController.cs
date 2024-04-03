using AirportAPI.Classes;
using AirportAPI.Exceptions;
using AirportAPI.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
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
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, MySqlAirportDatabase database)
        {
            _logger = logger;
            AirportDatabase = database;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                User result =  await AirportDatabase.GetUserAsync(id);
                return result;
            }
            catch(Exception exc)
            {
                _logger.LogError(message: exc.Message);

                if(exc is ObjectNotFoundException)
                {
                    return NotFound(id);
                }

                return BadRequest(exc.Message);
            }
        }

        // GET api/<UserController>/5
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                List<User> result = await AirportDatabase.GetAllUsersAsync();
                return Ok(result);
            }
            catch (Exception exc)
            {
                _logger.LogError(message: exc.Message);


                return BadRequest(exc.Message);
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            try
            {
                Task task = Task.Run(() => { });
                await task;

                return Ok(JsonConvert.SerializeObject(user));

                //User result = await AirportDatabase.AddUserAsync(user);
                //return Ok(result);
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
                User result = await AirportDatabase.UpdateUserAsync(user);
                return result;
            }
            catch (Exception exc)
            {
                if (exc is ObjectNotFoundException)
                {
                    return NotFound(user);
                }

                return BadRequest(exc.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                int deletedUserId = await AirportDatabase.DeleteUserAsync(id);
                return Ok(deletedUserId);
            }
            catch (Exception exc)
            {
                if (exc is ObjectNotFoundException)
                {
                    return NotFound(id);
                }

                return BadRequest(exc.Message);
            }
        }
    }
}
