using AirportAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace AirportAPI.Controllers
{
    public interface IUserController
    {
        Task<ActionResult<User>> Get(int id);
        Task<ActionResult<User>> Post([FromBody] User user);
        Task<ActionResult<User>> Put(int id, [FromBody] User user);
        Task<ActionResult> Delete(int userId);
    }
}