using AirportAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace AirportAPI.Controllers
{
    public interface IUserController
    {
        Task<ActionResult<User>> Get(int id);
        Task<ActionResult<List<User>>> Get();
        Task<ActionResult<User>> Post([FromBody] User user);
        Task<ActionResult<User>> Put([FromBody] User user);
        Task<ActionResult> Delete(int userId);
    }
}