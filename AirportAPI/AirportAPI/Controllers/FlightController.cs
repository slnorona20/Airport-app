using AirportAPI.Classes;
using AirportAPI.Exceptions;
using AirportAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase, IFlightController
    {
        protected IAirportDatabase AirportDatabase;
        private readonly ILogger<UserController> _logger;

        public FlightController(ILogger<UserController> logger, MySqlAirportDatabase database)
        {
            _logger = logger;
            AirportDatabase = database;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> Get(int id)
        {
            try
            {
                Flight result = await AirportDatabase.GetFlightAsync(id);
                return result;
            }
            catch (Exception exc)
            {
                _logger.LogError(message: exc.Message);

                if (exc is ObjectNotFoundException)
                {
                    return NotFound(id);
                }

                return BadRequest(exc.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Flight>>> Get()
        {
            try
            {
                List<Flight> result = await AirportDatabase.GetAllFlightsAsync();
                return Ok(result);
            }
            catch (Exception exc)
            {
                _logger.LogError(message: exc.Message);


                return BadRequest(exc.Message);
            }
        }

        public async Task<ActionResult<Flight>> Post([FromBody] Flight flight)
        {
            try
            {
                Flight result = await AirportDatabase.AddFlightAsync(flight);
                return Ok(result);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        public async Task<ActionResult<Flight>> Put(int id, [FromBody] Flight flight)
        {
            try
            {
                Flight result = await AirportDatabase.UpdateFlightAsync(flight);
                return result;
            }
            catch (Exception exc)
            {
                if (exc is ObjectNotFoundException)
                {
                    return NotFound(flight);
                }

                return BadRequest("Error");
            }
        }

        public async Task<ActionResult> Delete(int flightId)
        {
            try
            {
                int deletedFlightId = await AirportDatabase.DeleteFlightAsync(flightId);
                return Ok(deletedFlightId);
            }
            catch (Exception exc)
            {
                if (exc is ObjectNotFoundException)
                {
                    return NotFound(flightId);
                }

                return BadRequest("Error");
            }
        }
    }
}
