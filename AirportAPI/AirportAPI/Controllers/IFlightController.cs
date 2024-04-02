using AirportAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirportAPI.Controllers
{
    public interface IFlightController
    {
        Task<ActionResult<Flight>> Get(int id);
        Task<ActionResult<List<Flight>>> Get();
        Task<ActionResult<Flight>> Post([FromBody] Flight flight);
        Task<ActionResult<Flight>> Put(int id, [FromBody] Flight flight);
        Task<ActionResult> Delete(int userId);
    }
}
